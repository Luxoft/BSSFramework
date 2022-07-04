using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Framework.Core;
using Framework.HierarchicalExpand;
using Framework.Persistent;

using JetBrains.Annotations;

namespace Framework.SecuritySystem.Rules.Builders.MaterializedPermissions
{
    public abstract class SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent>
        : ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase

    {
        internal readonly SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> Factory;

        protected SecurityExpressionBuilderBase(
            [NotNull] SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory)
        {
            this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        public ISecurityExpressionFilter<TDomainObject> GetFilter<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
            where TSecurityOperationCode : struct, Enum
        {
            return new SecurityExpressionFilter<TPersistentDomainObjectBase, TDomainObject, TSecurityOperationCode, TIdent>(this, securityOperation);
        }




        public abstract Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<TIdent>> permission);

        public abstract Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType);

        public abstract IEnumerable<Type> GetUsedTypes();

        public virtual Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(List<Dictionary<Type, IEnumerable<TIdent>>> permissions)
        {
            if (permissions == null) throw new ArgumentNullException(nameof(permissions));

            return permissions.BuildOr(this.GetSecurityFilterExpression);
        }

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
        where TPath : SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>
    {
        protected readonly TPath Path;

        protected SecurityExpressionBuilderBase([NotNull] SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory,
                                                TPath path) : base(factory)
        {
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
        }


        public override IEnumerable<Type> GetUsedTypes()
        {
            return this.Path.GetUsedTypes();
        }


        public abstract class SecurityPathExpressionBuilderBase<TInnerPath> : SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TDomainObject, TIdent, TInnerPath>
            where TInnerPath : SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>
        {
            protected SecurityPathExpressionBuilderBase(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, TInnerPath path): base(factory, path)
            {

            }
        }

        public abstract class SecurityByIdentsExpressionBuilderBase<TSecurityContext, TInnerPath> : SecurityPathExpressionBuilderBase<TInnerPath>
            where TSecurityContext : TPersistentDomainObjectBase, ISecurityContext
            where TInnerPath : SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>
        {
            protected SecurityByIdentsExpressionBuilderBase(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, TInnerPath path)
                : base(factory, path)
            {

            }


            protected abstract Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> SecurityFilter { get; }



            public sealed override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<TIdent>> permission)
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
                                                                      && securityIdents.Contains(filterItem.Entity.EntityId)));
                }
                else
                {
                    return fullAccessFilter;
                }
            }
        }

        public class SecurityByIdentsExpressionBuilder<TSecurityContext> : SecurityByIdentsExpressionBuilderBase<TSecurityContext, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext>>
            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
        {
            public SecurityByIdentsExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SecurityPathByIdents<TSecurityContext> path)
                : base(factory, path)
            {

            }


            protected override Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> SecurityFilter
            {
                get { return this.Path.SecurityFilter; }
            }


            protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject)
            {
                throw new NotImplementedException();
            }
        }

        public class ConditionBinarySecurityPathExpressionBuilder : SecurityPathExpressionBuilderBase<SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath>
        {
            public ConditionBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ConditionPath path)
                : base(factory, path)
            {

            }


            public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<TIdent>> _)
            {
                return this.Path.SecurityFilter;
            }

            public override Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
            {
                var hasAccess = this.Path.SecurityFilter.Eval(domainObject, LambdaCompileCache);

                return _ => hasAccess;
            }


            private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
        }

        public class SingleSecurityExpressionBuilder<TSecurityContext> : SecurityByIdentsExpressionBuilderBase<TSecurityContext, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SingleSecurityPath<TSecurityContext>>
           where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
        {
            public SingleSecurityExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.SingleSecurityPath<TSecurityContext> path)
                : base(factory, path)
            {
            }


            protected override Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> SecurityFilter
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
                if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

                var securityObject = this.Path.SecurityPath.Eval(domainObject, LambdaCompileCache);

                if (securityObject != null)
                {
                    yield return securityObject;
                }
            }

            private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
        }

        public class ManySecurityExpressionBuilder<TSecurityContext> : SecurityByIdentsExpressionBuilderBase<TSecurityContext, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ManySecurityPath<TSecurityContext>>

            where TSecurityContext : class, TPersistentDomainObjectBase, ISecurityContext
        {
            public ManySecurityExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.ManySecurityPath<TSecurityContext> path)
                : base(factory, path)
            {

            }


            protected override Func<IEnumerable<TIdent>, Expression<Func<TDomainObject, bool>>> SecurityFilter
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
                            else if (this.Path.SecurityPath != null)
                            {
                                return securityIdents => from securityObjects in this.Path.SecurityPath

                                                         select securityObjects.Any(item => securityIdents.Contains(item.Id));
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
                                return securityIdents => from securityObjects in this.Path.SecurityPathQ

                                                         select !securityObjects.Any() || securityObjects.Any(item => securityIdents.Contains(item.Id));
                            }
                            else if (this.Path.SecurityPath != null)
                            {
                                return securityIdents => from securityObjects in this.Path.SecurityPath

                                                         select !securityObjects.Any() || securityObjects.Any(item => securityIdents.Contains(item.Id));
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
                                return securityIdents => from securityObjects in this.Path.SecurityPathQ

                                                         select securityObjects.All(item => securityIdents.Contains(item.Id));
                            }
                            else if (this.Path.SecurityPath != null)
                            {
                                return securityIdents => from securityObjects in this.Path.SecurityPath

                                                         select securityObjects.All(item => securityIdents.Contains(item.Id));
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
            }

            protected override IEnumerable<TSecurityContext> GetSecurityObjects(TDomainObject domainObject)
            {
                return this.Path.SecurityPath.Eval(domainObject, LambdaCompileCache).EmptyIfNull();
            }

            private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);
        }


        public class NestedManySecurityExpressionBuilder<TNestedObject> : SecurityPathExpressionBuilderBase<SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.NestedManySecurityPath<TNestedObject>>
            where TNestedObject : class, TPersistentDomainObjectBase
        {
            private readonly SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TNestedObject, TIdent> _nestedBuilder;

            private static readonly MethodInfo anyEmptyEnumerableMethodInfo;
            private static readonly MethodInfo anyEnumerableMethodInfo;
            private static readonly MethodInfo allEnumerableMethodInfo;
            private static readonly string getAccessortFilterMethodInfoName;
            private static readonly MethodInfo selectPermissionFuncEnumerableMethodInfo;
            private static readonly MethodInfo buildOrMethod;
            private static readonly MethodInfo buildAndMethod;

            private static readonly LambdaCompileCache LambdaCompileCache = new LambdaCompileCache(LambdaCompileMode.All);

            private readonly Lazy<Expression<Func<TDomainObject, HierarchicalExpandType, Expression<Func<IPermission<TIdent>, bool>>>>> _getAccessableFilterLazy;



            static NestedManySecurityExpressionBuilder()
            {
                anyEmptyEnumerableMethodInfo = new Func<IEnumerable<TNestedObject>, bool>(Enumerable.Any).Method;

                anyEnumerableMethodInfo = new Func<IEnumerable<TNestedObject>, Func<TNestedObject, bool>, bool>(Enumerable.Any).Method;

                allEnumerableMethodInfo = new Func<IEnumerable<TNestedObject>, Func<TNestedObject, bool>, bool>(Enumerable.All).Method;

                buildOrMethod = ((Func<IEnumerable<Expression<Func<IPermission<TIdent>, bool>>>, Expression<Func<IPermission<TIdent>, bool>>>)(Framework.Core.ExpressionExtensions.BuildOr)).Method;
                buildAndMethod = ((Func<IEnumerable<Expression<Func<IPermission<TIdent>, bool>>>, Expression<Func<IPermission<TIdent>, bool>>>)(Framework.Core.ExpressionExtensions.BuildAnd)).Method;

                selectPermissionFuncEnumerableMethodInfo = ((Func<
                    IEnumerable<TNestedObject>,
                    Func<TNestedObject, Expression<Func<IPermission<TIdent>, bool>>>,
                    IEnumerable<Expression<Func<IPermission<TIdent>, bool>>>>)(Enumerable.Select)).Method;

                getAccessortFilterMethodInfoName = "GetAccessorsFilter";
            }

            public NestedManySecurityExpressionBuilder(
                SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory,
                SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.NestedManySecurityPath<TNestedObject> path)
                : base(factory, path)
            {
                this._nestedBuilder = (SecurityExpressionBuilderBase<TPersistentDomainObjectBase, TNestedObject, TIdent>)this.Factory.CreateBuilder(this.Path.NestedSecurityPath);
                this._getAccessableFilterLazy = new Lazy<Expression<Func<TDomainObject, HierarchicalExpandType, Expression<Func<IPermission<TIdent>, bool>>>>>(() => this.CreateAccessorsFilterExpression(), true);
            }

            public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(List<Dictionary<Type, IEnumerable<TIdent>>> permissions)
            {
                var filterExpression = permissions.BuildOr(this._nestedBuilder.GetSecurityFilterExpression);

                switch (this.Path.Mode)
                {
                    case ManySecurityPathMode.Any:

                        var any = Expression.Call(anyEmptyEnumerableMethodInfo, this.Path.NestedObjectsPath.Body);
                        var anyFilter = Expression.Call(anyEnumerableMethodInfo, this.Path.NestedObjectsPath.Body, filterExpression);

                        var notAny = Expression.Not(any);
                        var finalOr = Expression.OrElse(notAny, anyFilter);

                        var anyResult = Expression.Lambda<Func<TDomainObject, bool>>(finalOr, this.Path.NestedObjectsPath.Parameters.First());

                        return anyResult;

                    case ManySecurityPathMode.AnyStrictly:

                        var onlyAnyFilter = Expression.Call(anyEnumerableMethodInfo, this.Path.NestedObjectsPath.Body, filterExpression);

                        var onlyAnyResult = Expression.Lambda<Func<TDomainObject, bool>>(onlyAnyFilter, this.Path.NestedObjectsPath.Parameters.First());

                        return onlyAnyResult;

                    case ManySecurityPathMode.All:

                        var allFilter = Expression.Call(allEnumerableMethodInfo, this.Path.NestedObjectsPath.Body, filterExpression);

                        var allResult = Expression.Lambda<Func<TDomainObject, bool>>(allFilter, this.Path.NestedObjectsPath.Parameters.First());

                        return allResult;

                    default:

                        throw new ArgumentOutOfRangeException("this.Path.Mode");
                }
            }

            public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<TIdent>> permission)
            {
                throw new NotImplementedException();
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
                    new Type[0],
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

                MethodInfo buildMethodInfo = null;

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
            where TBinaryPath : SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.BinarySecurityPath
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


            public override Expression<Func<TDomainObject, bool>> GetSecurityFilterExpression(Dictionary<Type, IEnumerable<TIdent>> permission)
            {
                if (permission == null) throw new ArgumentNullException(nameof(permission));

                var leftFilter = this.LeftBuilder.GetSecurityFilterExpression(permission);
                var rightFilter = this.RightBuilder.GetSecurityFilterExpression(permission);

                return this.BuildOperation(leftFilter, rightFilter);
            }

            public override Expression<Func<IPermission<TIdent>, bool>> GetAccessorsFilter(TDomainObject domainObject, HierarchicalExpandType expandType)
            {
                if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

                var leftFilter = this.LeftBuilder.GetAccessorsFilter(domainObject, expandType);
                var rightFilter = this.RightBuilder.GetAccessorsFilter(domainObject, expandType);

                return this.BuildOperation(leftFilter, rightFilter);
            }
        }

        public class AndBinarySecurityPathExpressionBuilder : SecurityBinaryExpressionBuilder<SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath>
        {
            public AndBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.AndSecurityPath path)
                : base(factory, path)
            {

            }


            protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(Expression<Func<TArg, bool>> arg1, Expression<Func<TArg, bool>> arg2)
            {
                return arg1.BuildAnd(arg2);
            }
        }

        public class OrBinarySecurityPathExpressionBuilder : SecurityBinaryExpressionBuilder<SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath>
        {
            public OrBinarySecurityPathExpressionBuilder(SecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> factory, SecurityPath<TPersistentDomainObjectBase, TDomainObject, TIdent>.OrSecurityPath path)
                : base(factory, path)
            {

            }


            protected override Expression<Func<TArg, bool>> BuildOperation<TArg>(Expression<Func<TArg, bool>> arg1, Expression<Func<TArg, bool>> arg2)
            {
                return arg1.BuildOr(arg2);
            }
        }
    }
}
