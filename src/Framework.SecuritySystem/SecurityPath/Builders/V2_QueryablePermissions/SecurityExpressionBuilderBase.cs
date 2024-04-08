using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders.QueryablePermissions;

public abstract class SecurityExpressionBuilderBase<TDomainObject, TIdent>
        : ISecurityExpressionBuilder<TDomainObject>

        where TDomainObject : class, IIdentityObject<TIdent>

{
    internal readonly SecurityExpressionBuilderFactory<TIdent> Factory;

    protected SecurityExpressionBuilderBase(SecurityExpressionBuilderFactory<TIdent> factory)
    {
        this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public ISecurityExpressionFilter<TDomainObject> GetFilter(SecurityRule securityRule)
    {
        return new SecurityExpressionFilter<TDomainObject, TIdent>(this, securityRule);
    }


    public Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(SecurityRule securityRule)
    {
        var filterExpression = this.GetSecurityFilterExpression(securityRule.ExpandType).ExpandConst().InlineEval();

        var baseQuery = this.Factory.AuthorizationSystem.GetPermissionQuery(securityRule);

        return domainObject => baseQuery.Any(permission => filterExpression.Eval(domainObject, permission));
    }


    public abstract Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType);

    public abstract Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType);

    public Expression<Func<IEnumerable<IPermission<TIdent>>, bool>> GetAccessorsFilterMany(TDomainObject domainObject, HierarchicalExpandType expandType)
    {
        if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

        return this.GetAccessorsFilter(domainObject, expandType).ToEnumerableAny();
    }
}

public abstract class SecurityExpressionBuilderBase<TDomainObject, TIdent, TPath>
        : SecurityExpressionBuilderBase<TDomainObject, TIdent>

        where TDomainObject : class, IIdentityObject<TIdent>
        where TPath : SecurityPath<TDomainObject>
{
    protected readonly TPath Path;

    protected SecurityExpressionBuilderBase(SecurityExpressionBuilderFactory<TIdent> factory,
                                            TPath path) : base(factory)
    {
        this.Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public abstract class SecurityPathExpressionBuilderBase<TInnerPath> : SecurityExpressionBuilderBase<TDomainObject, TIdent, TInnerPath>
            where TInnerPath : SecurityPath<TDomainObject>
    {
        protected SecurityPathExpressionBuilderBase(SecurityExpressionBuilderFactory<TIdent> factory, TInnerPath path) : base(factory, path)
        {

        }
    }

    public abstract class SecurityByIdentsExpressionBuilderBase<TSecurityContext, TInnerPath> : SecurityPathExpressionBuilderBase<TInnerPath>
            where TSecurityContext : class, ISecurityContext, IIdentityObject<TIdent>
            where TInnerPath : SecurityPath<TDomainObject>
    {
        protected SecurityByIdentsExpressionBuilderBase(SecurityExpressionBuilderFactory<TIdent> factory, TInnerPath path)
                : base(factory, path)
        {

        }

        protected abstract IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject);

        public override Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var securityObjects = this.GetSecurityObjects(domainObject).ToArray();

            var securityContextTypeName = this.Factory.SecurityContextInfoService.GetSecurityContextInfo(typeof(TSecurityContext)).Name;

            var fullAccessFilter = ExpressionHelper.Create((IPermission<TIdent> permission) => permission.FilterItems.All(filterItem => filterItem.Entity.EntityType.Name != securityContextTypeName));

            if (securityObjects.Any())
            {
                var securityIdents = this.Factory.HierarchicalObjectExpanderFactory
                                         .Create(typeof(TSecurityContext))
                                         .Expand(securityObjects.Select(securityObject => securityObject.Id), expandType.Reverse());

                return fullAccessFilter.BuildOr(permission =>

                                                        permission.FilterItems.Any(filterItem => filterItem.Entity.EntityType.Name == securityContextTypeName
                                                                                       && securityIdents.Contains(filterItem.ContextEntityId)));
            }
            else
            {
                return fullAccessFilter;
            }
        }
    }

    public class ConditionBinarySecurityPathExpressionBuilder : SecurityPathExpressionBuilderBase<SecurityPath<TDomainObject>.ConditionPath>
    {
        public ConditionBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TIdent> factory, SecurityPath<TDomainObject>.ConditionPath path)
                : base(factory, path)
        {
        }

        public override Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
        {
            var securityFilter = this.Path.SecurityFilter;

            return (domainObject, _) => securityFilter.Eval(domainObject);
        }

        public override Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
        {
            var hasAccess = this.Path.SecurityFilter.Eval(domainObject, LambdaCompileCache);

            return _ => hasAccess;
        }

        [SuppressMessage("SonarQube", "S2743")]
        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
    }

    public class SingleSecurityExpressionBuilder<TSecurityContext> : SecurityByIdentsExpressionBuilderBase<TSecurityContext, SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext>>
            where TSecurityContext : class, ISecurityContext, IIdentityObject<TIdent>
    {
        public SingleSecurityExpressionBuilder(SecurityExpressionBuilderFactory<TIdent> factory, SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> path)
                : base(factory, path)
        {
        }


        public override Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
        {
            var securityContextTypeId =
                ((SecurityContextInfo<TSecurityContext, TIdent>)this.Factory.SecurityContextInfoService.GetSecurityContextInfo(
                        typeof(TSecurityContext))).Id;

            var eqIdentsExpr = ExpressionHelper.GetEquality<TIdent>();

            var getIdents = ExpressionHelper.Create((IPermission<TIdent> permission) =>
                                                            permission.FilterItems
                                                                      .Where(item => eqIdentsExpr.Eval(item.EntityType.Id, securityContextTypeId))
                                                                      .Select(fi => fi.ContextEntityId))
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
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var securityObject = this.Path.SecurityPath.Eval(domainObject, LambdaCompileCache);

            if (securityObject != null)
            {
                yield return securityObject;
            }
        }

        [SuppressMessage("SonarQube", "S2743")]
        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
    }

    public class ManySecurityExpressionBuilder<TSecurityContext> : SecurityByIdentsExpressionBuilderBase<TSecurityContext, SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext>>

            where TSecurityContext : class, ISecurityContext, IIdentityObject<TIdent>
    {
        public ManySecurityExpressionBuilder(SecurityExpressionBuilderFactory<TIdent> factory, SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> path)
                : base(factory, path)
        {
        }

        public override Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
        {
            var securityContextTypeId =
                ((SecurityContextInfo<TSecurityContext, TIdent>)this.Factory.SecurityContextInfoService.GetSecurityContextInfo(
                        typeof(TSecurityContext))).Id;

            var eqIdentsExpr = ExpressionHelper.GetEquality<TIdent>();

            var getIdents = ExpressionHelper.Create((IPermission<TIdent> permission) =>
                                                            permission.FilterItems
                                                                      .Where(item => eqIdentsExpr.Eval(item.EntityType.Id, securityContextTypeId))
                                                                      .Select(fi => fi.ContextEntityId))
                                            .ExpandConst()
                                            .InlineEval();

            var expander = (IHierarchicalObjectQueryableExpander<TIdent>)this.Factory.HierarchicalObjectExpanderFactory.Create(typeof(TSecurityContext));

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

                                       || this.Path.SecurityPathQ.Eval(domainObject).Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
                    }
                    else if (this.Path.SecurityPath != null)
                    {
                        return (domainObject, permission) =>

                                       !getIdents.Eval(permission).Any()

                                       || this.Path.SecurityPath.Eval(domainObject).Any(item => expandExpressionQ.Eval(permission).Contains(item.Id));
                    }
                    else
                    {
                        throw new Exception("Invalid path");
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
                    else if (this.Path.SecurityPath != null)
                    {
                        return (domainObject, permission) =>

                                       !getIdents.Eval(permission).Any()

                                       || !this.Path.SecurityPath.Eval(domainObject).Any()

                                       || this.Path.SecurityPath.Eval(domainObject).Any(item => getIdents.Eval(permission).Contains(item.Id));
                    }
                    else
                    {
                        throw new Exception("Invalid path");
                    }
                }

                default:

                    throw new ArgumentOutOfRangeException("this.Path.Mode");
            }
        }

        protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject)
        {
            return this.Path.SecurityPath.Eval(domainObject, LambdaCompileCache).EmptyIfNull();
        }

        [SuppressMessage("SonarQube", "S2743")]
        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
    }


    public class NestedManySecurityExpressionBuilder<TNestedObject> : SecurityPathExpressionBuilderBase<SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject>>
            where TNestedObject : class, IIdentityObject<TIdent>
    {

        private readonly SecurityExpressionBuilderBase<TNestedObject, TIdent> _nestedBuilder;

        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);


        public NestedManySecurityExpressionBuilder(
                SecurityExpressionBuilderFactory<TIdent> factory,
                SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> path)
                : base(factory, path)
        {
            this._nestedBuilder = (SecurityExpressionBuilderBase<TNestedObject, TIdent>)this.Factory.CreateBuilder(this.Path.NestedSecurityPath);
        }

        public override Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
        {
            var baseFilter = this._nestedBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

            switch (this.Path.Mode)
            {
                case ManySecurityPathMode.Any:

                    return (domainObject, permission) => !this.Path.NestedObjectsPath.Eval(domainObject).Any()

                                                         || this.Path.NestedObjectsPath.Eval(domainObject).Any(nestedObject => baseFilter.Eval(nestedObject, permission));

                case ManySecurityPathMode.AnyStrictly:

                    return (domainObject, permission) => this.Path.NestedObjectsPath.Eval(domainObject).Any(nestedObject => baseFilter.Eval(nestedObject, permission));

                default:

                    throw new ArgumentOutOfRangeException("this.Path.Mode");
            }
        }

        public override Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
        {
            if (domainObject == null)
            {
                throw new ArgumentNullException(nameof(domainObject));
            }

            return this.Path.NestedObjectsPath.Eval(domainObject, LambdaCompileCache)
                       .BuildOr(item => this._nestedBuilder.GetAccessorsFilter(item, expandType));
        }
    }

    public abstract class SecurityBinaryExpressionBuilder<TBinaryPath> : SecurityExpressionBuilderBase<TDomainObject, TIdent, TBinaryPath>
            where TBinaryPath : SecurityPath<TDomainObject>.BinarySecurityPath
    {
        protected readonly SecurityExpressionBuilderBase<TDomainObject, TIdent> LeftBuilder;
        protected readonly SecurityExpressionBuilderBase<TDomainObject, TIdent> RightBuilder;

        protected SecurityBinaryExpressionBuilder(SecurityExpressionBuilderFactory<TIdent> factory, TBinaryPath path)
                : base(factory, path)
        {
            this.LeftBuilder = (SecurityExpressionBuilderBase<TDomainObject, TIdent>)this.Path.Left.Pipe(v => this.Factory.CreateBuilder(v));
            this.RightBuilder = (SecurityExpressionBuilderBase<TDomainObject, TIdent>)this.Path.Right.Pipe(v => this.Factory.CreateBuilder(v));
        }


        protected abstract Expression<Func<TArg, bool>> BuildOperation<TArg>(Expression<Func<TArg, bool>> arg1, Expression<Func<TArg, bool>> arg2);



        public override Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var leftFilter = this.LeftBuilder.GetAccessorsFilter(domainObject, expandType);
            var rightFilter = this.RightBuilder.GetAccessorsFilter(domainObject, expandType);

            return this.BuildOperation(leftFilter, rightFilter);
        }
    }

    public class AndBinarySecurityPathExpressionBuilder : SecurityBinaryExpressionBuilder<SecurityPath<TDomainObject>.AndSecurityPath>
    {
        public AndBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TIdent> factory, SecurityPath<TDomainObject>.AndSecurityPath path)
                : base(factory, path)
        {

        }


        public override Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
        {
            var leftFilter = this.LeftBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();
            var rightFilter = this.RightBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

            return (domainObject, permission) =>

                           leftFilter.Eval(domainObject, permission) && rightFilter.Eval(domainObject, permission);
        }

        protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(Expression<Func<TArg, bool>> arg1, Expression<Func<TArg, bool>> arg2)
        {
            return arg1.BuildAnd(arg2);
        }
    }

    public class OrBinarySecurityPathExpressionBuilder : SecurityBinaryExpressionBuilder<SecurityPath<TDomainObject>.OrSecurityPath>
    {
        public OrBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TIdent> factory, SecurityPath<TDomainObject>.OrSecurityPath path)
                : base(factory, path)
        {

        }
        public override Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
        {
            var leftFilter = this.LeftBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();
            var rightFilter = this.RightBuilder.GetSecurityFilterExpression(expandType).ExpandConst().InlineEval();

            return (domainObject, permission) =>

                           leftFilter.Eval(domainObject, permission) || rightFilter.Eval(domainObject, permission);
        }

        protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(Expression<Func<TArg, bool>> arg1, Expression<Func<TArg, bool>> arg2)
        {
            return arg1.BuildOr(arg2);
        }
    }
}
