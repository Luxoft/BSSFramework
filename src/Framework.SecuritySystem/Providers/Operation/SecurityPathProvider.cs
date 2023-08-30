using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem.Providers.Operation
{
    /// <summary>
    /// Контекстный провайдер доступа
    /// </summary>

    /// <typeparam name="TPersistentDomainObjectBase"></typeparam>
    /// <typeparam name="TDomainObject"></typeparam>
    /// <typeparam name="TIdent"></typeparam>
    /// <typeparam name="TSecurityOperationCode"></typeparam>
    public class SecurityPathProvider<TPersistentDomainObjectBase, TDomainObject, TIdent, TSecurityOperationCode> : ISecurityProvider<TDomainObject>

        where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>
        where TDomainObject : class, TPersistentDomainObjectBase

        where TSecurityOperationCode : struct, Enum
    {
        private readonly ContextSecurityOperation<TSecurityOperationCode> securityOperation;

        private readonly IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService;

        private readonly Lazy<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> injectFilterFunc;
        private readonly Lazy<ISecurityExpressionFilter<TDomainObject>> lazyFilter;

        private readonly ISecurityExpressionBuilder<TPersistentDomainObjectBase, TDomainObject, TIdent> securityExpressionBuilder;


        public SecurityPathProvider(
            SecurityPath<TDomainObject> securityPathBase,
            ContextSecurityOperation<TSecurityOperationCode> securityOperation,
            ISecurityExpressionBuilderFactory<TPersistentDomainObjectBase, TIdent> securityExpressionBuilderFactory,
            IAccessDeniedExceptionService<TPersistentDomainObjectBase> accessDeniedExceptionService)
        {
            if (securityPathBase == null) throw new ArgumentNullException(nameof(securityPathBase));
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            this.securityOperation = securityOperation;
            this.accessDeniedExceptionService = accessDeniedExceptionService;

            this.securityExpressionBuilder = securityExpressionBuilderFactory.CreateBuilder(securityPathBase);

            this.lazyFilter = LazyHelper.Create(() => this.securityExpressionBuilder.GetFilter(securityOperation));
            this.injectFilterFunc = LazyHelper.Create(() => this.lazyFilter.Value.InjectFunc);
        }

        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            return this.injectFilterFunc.Value(queryable);
        }

        public AccessResult GetAccessResult(TDomainObject domainObject)
        {
            return AccessResult.Create(
                this.lazyFilter.Value.HasAccessFunc(domainObject),
                () => this.accessDeniedExceptionService.BuildAccessDeniedException(
                        domainObject,
                        new Dictionary<string, object> { { "SecurityOperation", this.securityOperation } }));
        }

        public UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.lazyFilter.Value.GetAccessors(domainObject).ToUnboundedList();
        }
    }
}
