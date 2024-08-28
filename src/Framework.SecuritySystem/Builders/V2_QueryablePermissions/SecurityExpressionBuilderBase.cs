using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem.Builders._Base;
using Framework.SecuritySystem.Builders._Filter;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.V2_QueryablePermissions;

public abstract class SecurityExpressionBuilderBase<TDomainObject>(SecurityExpressionBuilderFactory factory)
    : ISecurityExpressionBuilder<TDomainObject>
    where TDomainObject : class, IIdentityObject<Guid>
{
    internal readonly SecurityExpressionBuilderFactory Factory = factory;

    public ISecurityExpressionFilter<TDomainObject> GetFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, IEnumerable<Type> securityTypes) => new SecurityExpressionFilter<TDomainObject>(this, securityRule);

    public Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(DomainSecurityRule.RoleBaseSecurityRule securityRule)
    {
        var filterExpression = this.GetSecurityFilterExpression(securityRule.SafeExpandType).ExpandConst().InlineEval();

        return this.Factory
                   .PermissionSystems
                   .Select(ps => ps.GetPermissionSource(securityRule).GetPermissionQuery())
                   .Select(
                       baseQuery => ExpressionHelper.Create(
                           (TDomainObject domainObject) => baseQuery.Any(permission => filterExpression.Eval(domainObject, permission))))
                   .BuildOr();
    }


    public abstract Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType);

    public abstract Expression<Func<IPermission, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType);
}

public abstract class SecurityExpressionBuilderBase<TDomainObject, TSecurityPath>(
    SecurityExpressionBuilderFactory factory,
    TSecurityPath path)
    : SecurityExpressionBuilderBase<TDomainObject>(factory)
    where TDomainObject : class, IIdentityObject<Guid>
    where TSecurityPath : SecurityPath<TDomainObject>
{
    protected readonly TSecurityPath Path = path;

    public abstract class SecurityPathExpressionBuilderBase<TInnerPath>(SecurityExpressionBuilderFactory factory, TInnerPath path)
        : SecurityExpressionBuilderBase<TDomainObject, TInnerPath>(factory, path)
        where TInnerPath : SecurityPath<TDomainObject>;

    public abstract class SecurityByIdentsExpressionBuilderBase<TSecurityContext, TInnerPath>(
        SecurityExpressionBuilderFactory factory,
        TInnerPath path) : SecurityPathExpressionBuilderBase<TInnerPath>(factory, path)
        where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
        where TInnerPath : SecurityPath<TDomainObject>
    {
        protected abstract IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject);

        public override Expression<Func<IPermission, bool>> GetAccessorsFilter(
            TDomainObject domainObject,
            HierarchicalExpandType expandType)
        {
            var securityObjects = this.GetSecurityObjects(domainObject).ToArray();

            var securityContextTypeId = this.Factory.SecurityContextInfoService.GetSecurityContextInfo(typeof(TSecurityContext)).Id;

            var fullAccessFilter = ExpressionHelper.Create(
                (IPermission permission) => !permission.Restrictions.Select(restriction => restriction.SecurityContextTypeId)
                                                               .Contains(securityContextTypeId));

            if (securityObjects.Any())
            {
                var securityIdents = this.Factory.HierarchicalObjectExpanderFactory
                                         .Create(typeof(TSecurityContext))
                                         .Expand(securityObjects.Select(securityObject => securityObject.Id), expandType.Reverse());

                return fullAccessFilter.BuildOr(
                    permission =>

                        permission.Restrictions
                                  .Where(restriction => securityIdents.Contains(restriction.SecurityContextId))
                                  .Select(restriction => restriction.SecurityContextTypeId)
                                  .Contains(securityContextTypeId));
            }
            else
            {
                return fullAccessFilter;
            }
        }
    }

    public class ConditionBinarySecurityPathExpressionBuilder(
        SecurityExpressionBuilderFactory factory,
        SecurityPath<TDomainObject>.ConditionPath path)
        : SecurityPathExpressionBuilderBase<SecurityPath<TDomainObject>.ConditionPath>(factory, path)
    {
        public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
        {
            var securityFilter = this.Path.SecurityFilter;

            return (domainObject, _) => securityFilter.Eval(domainObject);
        }

        public override Expression<Func<IPermission, bool>> GetAccessorsFilter(
            TDomainObject domainObject,
            HierarchicalExpandType expandType)
        {
            var hasAccess = this.Path.SecurityFilter.Eval(domainObject, LambdaCompileCache);

            return _ => hasAccess;
        }

        [SuppressMessage("SonarQube", "S2743")]
        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
    }

    public class SingleSecurityExpressionBuilder<TSecurityContext>(
        SecurityExpressionBuilderFactory factory,
        SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> path)
        : SecurityByIdentsExpressionBuilderBase<TSecurityContext, SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext>>(
            factory,
            path)
        where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
    {
        public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
        {
            var securityContextTypeId = this.Factory.SecurityContextInfoService.GetSecurityContextInfo(typeof(TSecurityContext)).Id;

            var eqIdentsExpr = ExpressionHelper.GetEquality<Guid>();

            var getIdents = ExpressionHelper.Create(
                                                (IPermission permission) =>
                                                    permission.Restrictions
                                                              .Where(
                                                                  item => eqIdentsExpr.Eval(
                                                                      item.SecurityContextTypeId,
                                                                      securityContextTypeId))
                                                              .Select(fi => fi.SecurityContextId))
                                            .ExpandConst()
                                            .InlineEval();

            var expander = this.Factory.HierarchicalObjectExpanderFactory.CreateQuery(typeof(TSecurityContext));

            var expandExpression = expander.GetExpandExpression(expandType);

            var expandExpressionQ = from idents in getIdents
                                    select expandExpression.Eval(idents);

            switch (this.Path.Mode)
            {
                case SingleSecurityMode.AllowNull:

                    return (domainObject, permission) =>

                               !getIdents.Eval(permission).Any()

                               || this.Path.SecurityPath.Eval(domainObject) == null

                               || expandExpressionQ.Eval(permission).Contains(this.Path.SecurityPath.Eval(domainObject).Id);

                case SingleSecurityMode.Strictly:

                    return (domainObject, permission) =>

                               !getIdents.Eval(permission).Any()

                               || expandExpressionQ.Eval(permission).Contains(this.Path.SecurityPath.Eval(domainObject).Id);

                default:

                    throw new ArgumentOutOfRangeException(this.Path.Mode.ToString());
            }
        }

        protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject)
        {
            var securityObject = this.Path.SecurityPath.Eval(domainObject, LambdaCompileCache);

            if (securityObject != null)
            {
                yield return securityObject;
            }
        }

        [SuppressMessage("SonarQube", "S2743")]
        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
    }

    public class ManySecurityExpressionBuilder<TSecurityContext>(
        SecurityExpressionBuilderFactory factory,
        SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> path)
        : SecurityByIdentsExpressionBuilderBase<TSecurityContext, SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext>>(
            factory,
            path)
        where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
    {
        public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
        {
            var securityContextTypeId = this.Factory.SecurityContextInfoService.GetSecurityContextInfo(typeof(TSecurityContext)).Id;

            var eqIdentsExpr = ExpressionHelper.GetEquality<Guid>();

            var getIdents = ExpressionHelper.Create(
                                                (IPermission permission) =>
                                                    permission.Restrictions
                                                              .Where(
                                                                  item => eqIdentsExpr.Eval(
                                                                      item.SecurityContextTypeId,
                                                                      securityContextTypeId))
                                                              .Select(fi => fi.SecurityContextId))
                                            .ExpandConst()
                                            .InlineEval();

            var expander =
                (IHierarchicalObjectQueryableExpander<Guid>)this.Factory.HierarchicalObjectExpanderFactory.Create(
                    typeof(TSecurityContext));

            var expandExpression = expander.GetExpandExpression(expandType);

            var expandExpressionQ = from idents in getIdents
                                    select expandExpression.Eval(idents);

            switch (this.Path.Mode)
            {
                case ManySecurityPathMode.AnyStrictly:
                    {
                        if (this.Path.SecurityPathQ != null)
                        {
                            return (domainObject, permission) =>

                                       !getIdents.Eval(permission).Any()

                                       || this.Path.SecurityPathQ.Eval(domainObject)
                                              .Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
                        }
                        else
                        {
                            return (domainObject, permission) =>

                                       !getIdents.Eval(permission).Any()

                                       || this.Path.SecurityPath.Eval(domainObject)
                                              .Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
                        }
                    }

                case ManySecurityPathMode.Any:
                    {
                        if (this.Path.SecurityPathQ != null)
                        {
                            return (domainObject, permission) =>

                                       !getIdents.Eval(permission).Any()

                                       || !this.Path.SecurityPathQ.Eval(domainObject).Any()

                                       || this.Path.SecurityPathQ.Eval(domainObject).Any(item => getIdents.Eval(permission).Contains(item.Id));
                        }
                        else
                        {
                            return (domainObject, permission) =>

                                       !getIdents.Eval(permission).Any()

                                       || !this.Path.SecurityPath.Eval(domainObject).Any()

                                       || this.Path.SecurityPath.Eval(domainObject).Any(item => getIdents.Eval(permission).Contains(item.Id));
                        }
                    }

                default:

                    throw new ArgumentOutOfRangeException("this.Path.Mode");
            }
        }

        protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject) =>
            this.Path.SecurityPath.Eval(domainObject, LambdaCompileCache).EmptyIfNull();

        [SuppressMessage("SonarQube", "S2743")]
        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
    }


    public class NestedManySecurityExpressionBuilder<TNestedObject> : SecurityPathExpressionBuilderBase<
        SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject>>
        where TNestedObject : class, IIdentityObject<Guid>
    {

        private readonly SecurityExpressionBuilderBase<TNestedObject> nestedBuilder;

        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);


        public NestedManySecurityExpressionBuilder(
            SecurityExpressionBuilderFactory factory,
            SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> path)
            : base(factory, path) =>
            this.nestedBuilder =
                (SecurityExpressionBuilderBase<TNestedObject>)this.Factory.CreateBuilder(this.Path.NestedSecurityPath);

        public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
        {
            var baseFilter = this.nestedBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

            switch (this.Path.Mode)
            {
                case ManySecurityPathMode.Any:

                    return (domainObject, permission) => !this.Path.NestedObjectsPath.Eval(domainObject).Any()

                                                         || this.Path.NestedObjectsPath.Eval(domainObject).Any(
                                                             nestedObject => baseFilter.Eval(nestedObject, permission));

                case ManySecurityPathMode.AnyStrictly:

                    return (domainObject, permission) => this.Path.NestedObjectsPath.Eval(domainObject)
                                                             .Any(nestedObject => baseFilter.Eval(nestedObject, permission));

                default:

                    throw new ArgumentOutOfRangeException("this.Path.Mode");
            }
        }

        public override Expression<Func<IPermission, bool>> GetAccessorsFilter(
            TDomainObject domainObject,
            HierarchicalExpandType expandType) =>
            this.Path.NestedObjectsPath.Eval(domainObject, LambdaCompileCache)
                .BuildOr(item => this.nestedBuilder.GetAccessorsFilter(item, expandType));
    }

    public abstract class SecurityBinaryExpressionBuilder<TSecurityPath> : SecurityExpressionBuilderBase<TDomainObject, TSecurityPath>
        where TSecurityPath : SecurityPath<TDomainObject>.BinarySecurityPath
    {
        protected readonly SecurityExpressionBuilderBase<TDomainObject> LeftBuilder;

        protected readonly SecurityExpressionBuilderBase<TDomainObject> RightBuilder;

        protected SecurityBinaryExpressionBuilder(SecurityExpressionBuilderFactory factory, TSecurityPath path)
            : base(factory, path)
        {
            this.LeftBuilder =
                (SecurityExpressionBuilderBase<TDomainObject>)this.Path.Left.Pipe(v => this.Factory.CreateBuilder(v));
            this.RightBuilder =
                (SecurityExpressionBuilderBase<TDomainObject>)this.Path.Right.Pipe(v => this.Factory.CreateBuilder(v));
        }


        protected abstract Expression<Func<TArg, bool>> BuildOperation<TArg>(
            Expression<Func<TArg, bool>> arg1,
            Expression<Func<TArg, bool>> arg2);



        public override Expression<Func<IPermission, bool>> GetAccessorsFilter(
            TDomainObject domainObject,
            HierarchicalExpandType expandType)
        {
            var leftFilter = this.LeftBuilder.GetAccessorsFilter(domainObject, expandType);
            var rightFilter = this.RightBuilder.GetAccessorsFilter(domainObject, expandType);

            return this.BuildOperation(leftFilter, rightFilter);
        }
    }

    public class AndBinarySecurityPathExpressionBuilder(
        SecurityExpressionBuilderFactory factory,
        SecurityPath<TDomainObject>.AndSecurityPath path)
        : SecurityBinaryExpressionBuilder<SecurityPath<TDomainObject>.AndSecurityPath>(factory, path)
    {
        public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
        {
            var leftFilter = this.LeftBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();
            var rightFilter = this.RightBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

            return (domainObject, permission) =>

                       leftFilter.Eval(domainObject, permission) && rightFilter.Eval(domainObject, permission);
        }

        protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(
            Expression<Func<TArg, bool>> arg1,
            Expression<Func<TArg, bool>> arg2) =>
            arg1.BuildAnd(arg2);
    }

    public class OrBinarySecurityPathExpressionBuilder(
        SecurityExpressionBuilderFactory factory,
        SecurityPath<TDomainObject>.OrSecurityPath path)
        : SecurityBinaryExpressionBuilder<SecurityPath<TDomainObject>.OrSecurityPath>(factory, path)
    {
        public override Expression<Func<TDomainObject, IPermission, bool>> GetSecurityFilterExpression(
            HierarchicalExpandType expandType)
        {
            var leftFilter = this.LeftBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();
            var rightFilter = this.RightBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

            return (domainObject, permission) =>

                       leftFilter.Eval(domainObject, permission) || rightFilter.Eval(domainObject, permission);
        }

        protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(
            Expression<Func<TArg, bool>> arg1,
            Expression<Func<TArg, bool>> arg2) =>
            arg1.BuildOr(arg2);
    }
}
