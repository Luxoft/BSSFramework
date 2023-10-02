using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem.Providers.Operation
{
    /// <summary>
    /// Контекстный провайдер доступа
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public class ContextSecurityPathProvider<TDomainObject> : ISecurityProvider<TDomainObject>
    {
        private readonly SecurityOperation securityOperation;

        private readonly Lazy<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> injectFilterFunc;

        private readonly Lazy<ISecurityExpressionFilter<TDomainObject>> lazyFilter;

        private readonly ISecurityExpressionBuilder<TDomainObject> securityExpressionBuilder;


        public ContextSecurityPathProvider(
            SecurityPath<TDomainObject> securityPathBase,
            SecurityOperation securityOperation,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
        {
            if (securityPathBase == null) throw new ArgumentNullException(nameof(securityPathBase));
            if (securityOperation == null) throw new ArgumentNullException(nameof(securityOperation));

            this.securityOperation = securityOperation;

            this.securityExpressionBuilder = securityExpressionBuilderFactory.CreateBuilder(securityPathBase);

            this.lazyFilter = LazyHelper.Create(() => this.securityExpressionBuilder.GetFilter(securityOperation));
            this.injectFilterFunc = LazyHelper.Create(() => this.lazyFilter.Value.InjectFunc);
        }

        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));

            return this.injectFilterFunc.Value(queryable);
        }

        public bool HasAccess(TDomainObject domainObject)
        {
            return this.lazyFilter.Value.HasAccessFunc(domainObject);
        }

        public AccessResult GetAccessResult(TDomainObject domainObject)
        {
            if (this.HasAccess(domainObject))
            {
                return AccessResult.AccessGrantedResult.Default;
            }
            else
            {
                return new AccessResult.AccessDeniedResult
                       {
                           SecurityOperation = this.securityOperation, DomainObjectInfo = (domainObject, typeof(TDomainObject))
                       };
            }
        }

        public UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            return this.lazyFilter.Value.GetAccessors(domainObject).ToUnboundedList();
        }
    }
}
