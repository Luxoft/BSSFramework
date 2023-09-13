using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;

namespace Framework.SecuritySystem.Rules.Builders.QueryablePermissions;

public abstract class SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent>
        : ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase

{
    internal readonly SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> Factory;

    protected SecurityExpressionBuilderBase(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory)
    {
        this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));
    }

    public ISecurityExpressionFilter<TDomainObject> GetFilter(ContextSecurityOperation securityOperation)
    {
        return new SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TIdent>(this, securityOperation);
    }


    public Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(ContextSecurityOperation securityOperation)
    {
        var filterExpression = this.GetSecurityFilterExpression(securityOperation.ExpandType).ExpandConst().InlineEval();

        var baseQuery = this.Factory.AuthorizationSystem.GetPermissionQuery(securityOperation);

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

public abstract class SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, TPath>
        : SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase
        where TPath : SecurityPath<TDomainObject>
{
    protected readonly TPath Path;

    protected SecurityExpressionBuilderBase(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory,
                                            TPath path) : base(factory)
    {
        this.Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public abstract class SecurityPathExpressionBuilderBase<TInnerPath> : SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, TInnerPath>
            where TInnerPath : SecurityPath<TDomainObject>
    {
        protected SecurityPathExpressionBuilderBase(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, TInnerPath path) : base(factory, path)
        {

        }
    }

    public abstract class SecurityByIdentsExpressionBuilderBase<TSecurityContext, TInnerPath> : SecurityPathExpressionBuilderBase<TInnerPath>
            where TSecurityContext : TPersistentDomainObjectBase, ISecurityContext
            where TInnerPath : SecurityPath<TDomainObject>
    {
        protected SecurityByIdentsExpressionBuilderBase(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, TInnerPath path)
                : base(factory, path)
        {

        }

        protected abstract IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject);

        public override Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            var securityObjects = this.GetSecurityObjects(domainObject).ToArray();

            var securityObjectName = this.Factory.AuthorizationSystem.ResolveSecurityTypeName(typeof(TSecurityContext));

            var fullAccessFilter = ExpressionHelper.Create((IPermission<TIdent> permission) => permission.FilterItems.All(filterItem => filterItem.Entity.EntityType.Name != securityObjectName));

            if (securityObjects.Any())
            {
                var securityIdents = this.Factory.HierarchicalObjectExpanderFactory
                                         .Create(typeof(TSecurityContext))
                                         .Expand(securityObjects.Select(securityObject => securityObject.Id), expandType.Reverse());

                return fullAccessFilter.BuildOr(permission =>

                                                        permission.FilterItems.Any(filterItem => filterItem.Entity.EntityType.Name == securityObjectName
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
        public ConditionBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TDomainObject>.ConditionPath path)
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
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        public SingleSecurityExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TDomainObject>.SingleSecurityPath<TSecurityContext> path)
                : base(factory, path)
        {
        }


        public override Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
        {
            var entityTypeId = this.Factory.AuthorizationSystem.ResolveSecurityTypeId(typeof(TSecurityContext));

            var eqIdentsExpr = ExpressionHelper.GetEquality<TIdent>();

            var getIdents = ExpressionHelper.Create((IPermission<TIdent> permission) =>
                                                            permission.FilterItems
                                                                      .Where(item => eqIdentsExpr.Eval(item.EntityType.Id, entityTypeId))
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

            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
    {
        public ManySecurityExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TDomainObject>.ManySecurityPath<TSecurityContext> path)
                : base(factory, path)
        {
        }

        public override Expression<Func<TDomainObject, IPermission<TIdent>, bool>> GetSecurityFilterExpression(HierarchicalExpandType expandType)
        {
            var entityTypeId = this.Factory.AuthorizationSystem.ResolveSecurityTypeId(typeof(TSecurityContext));

            var eqIdentsExpr = ExpressionHelper.GetEquality<TIdent>();

            var getIdents = ExpressionHelper.Create((IPermission<TIdent> permission) =>
                                                            permission.FilterItems
                                                                      .Where(item => eqIdentsExpr.Eval(item.EntityType.Id, entityTypeId))
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

                case ManySecurityPathMode.All:
                {
                    if (this.Path.SecurityPathQ != null)
                    {
                        return (domainObject, permission) =>

                                       !getIdents.Eval(permission).Any()

                                       || this.Path.SecurityPathQ.Eval(domainObject).All(item => getIdents.Eval(permission).Contains(item.Id));
                    }
                    else if (this.Path.SecurityPath != null)
                    {
                        return (domainObject, permission) =>

                                       !getIdents.Eval(permission).Any()

                                       || this.Path.SecurityPath.Eval(domainObject).All(item => getIdents.Eval(permission).Contains(item.Id));
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
            where TNestedObject : class, TPersistentDomainObjectBase
    {
        private readonly SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TNestedObject, TIdent> _nestedBuilder;

        private static readonly string getAccessortFilterMethodInfoName;
        private static readonly MethodInfo buildOrMethod;
        private static readonly MethodInfo buildAndMethod;

        [SuppressMessage("SonarQube", "S2743")]
        private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);

        private readonly Lazy<Expression<Func<TDomainObject, HierarchicalExpandType, Expression<Func<IPermission<TIdent>, bool>>>>> _getAccessableFilterLazy;



        static NestedManySecurityExpressionBuilder()
        {
            buildOrMethod = ((Func<IEnumerable<Expression<Func<IPermission<TIdent>, bool>>>, Expression<Func<IPermission<TIdent>, bool>>>)(Framework.Core.ExpressionExtensions.BuildOr)).Method;
            buildAndMethod = ((Func<IEnumerable<Expression<Func<IPermission<TIdent>, bool>>>, Expression<Func<IPermission<TIdent>, bool>>>)(Framework.Core.ExpressionExtensions.BuildAnd)).Method;

            getAccessortFilterMethodInfoName = "GetAccessorsFilter";
        }

        public NestedManySecurityExpressionBuilder(
                SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory,
                SecurityPath<TDomainObject>.NestedManySecurityPath<TNestedObject> path)
                : base(factory, path)
        {
            this._nestedBuilder = (SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TNestedObject, TIdent>)this.Factory.CreateBuilder(this.Path.NestedSecurityPath);
            this._getAccessableFilterLazy = new Lazy<Expression<Func<TDomainObject, HierarchicalExpandType, Expression<Func<IPermission<TIdent>, bool>>>>>(() => this.CreateAccessorsFilterExpression(), true);
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

                case ManySecurityPathMode.All:

                    return (domainObject, permission) => this.Path.NestedObjectsPath.Eval(domainObject).All(nestedObject => baseFilter.Eval(nestedObject, permission));

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

            var result = this._getAccessableFilterLazy.Value;

            return result.Compile(LambdaCompileCache)(domainObject, expandType);
        }

        private Expression<Func<TDomainObject, HierarchicalExpandType, Expression<Func<IPermission<TIdent>, bool>>>> CreateAccessorsFilterExpression()
        {
            var nestedObjectParameter = Expression.Parameter(typeof(TNestedObject));
            var expandTypeParameter = Expression.Parameter(typeof(HierarchicalExpandType));
            var builderParameter = Expression.Constant(
                                                       this._nestedBuilder,
                                                       typeof(SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TNestedObject, TIdent>));

            var getAccessorFilters = Expression.Call(
                                                     builderParameter,
                                                     getAccessortFilterMethodInfoName,
                                                     Type.EmptyTypes,
                                                     nestedObjectParameter,
                                                     expandTypeParameter);

            var getAccessorFiltersExpression =
                    Expression.Lambda<Func<TNestedObject, Expression<Func<IPermission<TIdent>, bool>>>>(
                     getAccessorFilters,
                     nestedObjectParameter);

            var selectAccessorExpression = Expression.Call(
                                                           typeof(Enumerable),
                                                           "Select",
                                                           new[] { typeof(TNestedObject), typeof(Expression<Func<IPermission<TIdent>, bool>>) },
                                                           this.Path.NestedObjectsPath.Body,
                                                           getAccessorFiltersExpression);

            MethodInfo buildMethodInfo;

            switch (this.Path.Mode)
            {
                case ManySecurityPathMode.Any:
                case ManySecurityPathMode.AnyStrictly:
                    buildMethodInfo = buildOrMethod;
                    break;
                case ManySecurityPathMode.All:
                    buildMethodInfo = buildAndMethod;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(this.Path.Mode.ToString());
            }

            var buildOrExpression = Expression.Call(buildMethodInfo, selectAccessorExpression);

            var result =
                    Expression.Lambda<Func<TDomainObject, HierarchicalExpandType, Expression<Func<IPermission<TIdent>, bool>>>>(
                     buildOrExpression,
                     this.Path.NestedObjectsPath.Parameters.First(),
                     expandTypeParameter);
            return result;
        }
    }

    public abstract class SecurityBinaryExpressionBuilder<TBinaryPath> : SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, TBinaryPath>
            where TBinaryPath : SecurityPath<TDomainObject>.BinarySecurityPath
    {
        protected readonly SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent> LeftBuilder;
        protected readonly SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent> RightBuilder;

        protected SecurityBinaryExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, TBinaryPath path)
                : base(factory, path)
        {
            this.LeftBuilder = (SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent>)this.Path.Left.Pipe(v => this.Factory.CreateBuilder(v));
            this.RightBuilder = (SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent>)this.Path.Right.Pipe(v => this.Factory.CreateBuilder(v));
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
        public AndBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TDomainObject>.AndSecurityPath path)
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
        public OrBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TDomainObject>.OrSecurityPath path)
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
