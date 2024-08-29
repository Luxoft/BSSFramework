using System.Linq.Expressions;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.SecuritySystem.Builders._Base;
using Framework.SecuritySystem.Builders._Filter;
using Framework.SecuritySystem.ExternalSystem;

namespace Framework.SecuritySystem.Builders.V1_MaterializedPermissions;

public abstract class SecurityExpressionBuilderBase<TDomainObject>(SecurityExpressionBuilderFactory factory)
    : ISecurityExpressionBuilder<TDomainObject>
    where TDomainObject : class, IIdentityObject<Guid>
{
    internal readonly SecurityExpressionBuilderFactory Factory = factory;

    public ISecurityExpressionFilter<TDomainObject> GetFilter(DomainSecurityRule.RoleBaseSecurityRule securityRule, IEnumerable<Type> securityTypes) => new SecurityExpressionFilter<TDomainObject>(this, securityRule, securityTypes);

    public abstract Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission);

    public abstract Expression<Func<IPermission, bool>> GetAccessorsFilter(
        TDomainObject domainObject,
        HierarchicalExpandType expandType);
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

    public abstract class
        SecurityByIdentsExpressionBuilderBase<TSecurityContext, TInnerPath>(
        SecurityExpressionBuilderFactory factory,
        TInnerPath path) : SecurityPathExpressionBuilderBase<TInnerPath>(factory, path)
        where TSecurityContext : class, ISecurityContext, IIdentityObject<Guid>
        where TInnerPath : SecurityPath<TDomainObject>
    {
        protected abstract Func<IEnumerable<Guid>, Expression<Func<TDomainObject, bool>>> SecurityFilter { get; }

        public sealed override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(
            Dictionary<Type, IEnumerable<Guid>> permission)
        {
            if (permission.TryGetValue(typeof(TSecurityContext), out var securityIdents))
            {
                return this.SecurityFilter(securityIdents);
            }
            else
            {
                return _ => true;
            }
        }

        protected abstract IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject);

        public override Expression<Func<IPermission, bool>> GetAccessorsFilter(
            TDomainObject domainObject,
            HierarchicalExpandType expandType)
        {
            var securityObjects = this.GetSecurityObjects(domainObject).ToArray();

            var securityContextTypeId = this.Factory.SecurityContextInfoService.GetSecurityContextInfo(typeof(TSecurityContext)).Id;

            var getIdentsExpr = ExpressionHelper.Create(
                (IPermission permission) =>
                    permission.Restrictions
                              .Where(item => item.SecurityContextTypeId == securityContextTypeId)
                              .Select(fi => fi.SecurityContextId));

            var fullAccessFilter = getIdentsExpr.Select(idents => !idents.Any());

            if (securityObjects.Any())
            {
                var securityIdents = this.Factory.HierarchicalObjectExpanderFactory
                                         .Create(typeof(TSecurityContext))
                                         .Expand(securityObjects.Select(securityObject => securityObject.Id), expandType.Reverse());

                return fullAccessFilter.BuildOr(getIdentsExpr.Select(idents => idents.Any(securityIdents.Contains)));
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
        public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> _) =>
            this.Path.SecurityFilter;

        public override Expression<Func<IPermission, bool>> GetAccessorsFilter(
            TDomainObject domainObject,
            HierarchicalExpandType expandType)
        {
            var hasAccess = this.Path.SecurityFilter.Eval(domainObject, LambdaCompileCache);

            return _ => hasAccess;
        }


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
        protected override Func<IEnumerable<Guid>, Expression<Func<TDomainObject, bool>>> SecurityFilter
        {
            get
            {
                switch (this.Path.Mode)
                {
                    case SingleSecurityMode.AllowNull:

                        return securityIdents => from securityObject in this.Path.SecurityPath

                                                 select securityObject == null || securityIdents.Contains(securityObject.Id);

                    case SingleSecurityMode.Strictly:

                        return securityIdents => from securityObject in this.Path.SecurityPath

                                                 select securityIdents.Contains(securityObject.Id);

                    default: throw new ArgumentOutOfRangeException(this.Path.Mode.ToString());
                }
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
        protected override Func<IEnumerable<Guid>, Expression<Func<TDomainObject, bool>>> SecurityFilter
        {
            get
            {
                switch (this.Path.Mode)
                {
                    case ManySecurityPathMode.AnyStrictly:
                        {
                            if (this.Path.SecurityPathQ != null)
                            {
                                return securityIdents => from securityObjects in this.Path.SecurityPathQ

                                                         select securityObjects.Any(item => securityIdents.Contains(item.Id));
                            }
                            else
                            {
                                return securityIdents => from securityObjects in this.Path.SecurityPath

                                                         select securityObjects.Any(item => securityIdents.Contains(item.Id));
                            }
                        }

                    case ManySecurityPathMode.Any:
                        {
                            if (this.Path.SecurityPathQ != null)
                            {
                                return securityIdents => from securityObjects in this.Path.SecurityPathQ

                                                         select !securityObjects.Any()
                                                                || securityObjects.Any(item => securityIdents.Contains(item.Id));
                            }
                            else
                            {
                                return securityIdents => from securityObjects in this.Path.SecurityPath

                                                         select !securityObjects.Any()
                                                                || securityObjects.Any(item => securityIdents.Contains(item.Id));
                            }
                        }

                    default:

                        throw new ArgumentOutOfRangeException("this.Path.Mode");
                }
            }
        }

        protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject) =>
            this.Path.SecurityPath.Eval(domainObject, LambdaCompileCache).EmptyIfNull();

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

        public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission)
        {
            var filterExpression = this.nestedBuilder.GetSecurityFilterExpression(permission);

            var collectionFilterExpression = filterExpression.ToCollectionFilter();

            var emptyCondition = this.Path.NestedObjectsPath.Select(v => !v.Any());

            var mainCondition = this.Path.NestedObjectsPath.Select(v => collectionFilterExpression.Eval(v).Any()).InlineEval();

            switch (this.Path.Mode)
            {
                case ManySecurityPathMode.Any:

                    return emptyCondition.BuildOr(mainCondition);

                case ManySecurityPathMode.AnyStrictly:

                    return mainCondition;

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

        public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<Guid>> permission)
        {
            var leftFilter = this.LeftBuilder.GetSecurityFilterExpression(permission);

            var rightFilter = this.RightBuilder.GetSecurityFilterExpression(permission);

            return this.BuildOperation(leftFilter, rightFilter);
        }

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
        protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(
            Expression<Func<TArg, bool>> arg1,
            Expression<Func<TArg, bool>> arg2) => arg1.BuildAnd(arg2);
    }

    public class OrBinarySecurityPathExpressionBuilder(
        SecurityExpressionBuilderFactory factory,
        SecurityPath<TDomainObject>.OrSecurityPath path)
        : SecurityBinaryExpressionBuilder<SecurityPath<TDomainObject>.OrSecurityPath>(factory, path)
    {
        protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(
            Expression<Func<TArg, bool>> arg1,
            Expression<Func<TArg, bool>> arg2) => arg1.BuildOr(arg2);
    }
}
