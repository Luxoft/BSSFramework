using Framework.Core;
using Framework.SecuritySystem.Rules.Builders;

namespace Framework.SecuritySystem.Providers.Operation
{
    /// <summary>
    /// Контекстный провайдер доступа
    /// </summary>
    /// <typeparam name="TDomainObject"></typeparam>
    public class ContextSecurityPathProvider<TDomainObject> : ISecurityProvider<TDomainObject>
    {
        private readonly SecurityRule.RolesSecurityRule securityRule;

        private readonly Lazy<Func<IQueryable<TDomainObject>, IQueryable<TDomainObject>>> injectFilterFunc;

        private readonly Lazy<ISecurityExpressionFilter<TDomainObject>> lazyFilter;

        private readonly ISecurityExpressionBuilder<TDomainObject> securityExpressionBuilder;


        public ContextSecurityPathProvider(
            SecurityPath<TDomainObject> securityPathBase,
            SecurityRule.RolesSecurityRule securityRule,
            ISecurityExpressionBuilderFactory securityExpressionBuilderFactory)
        {
            if (securityPathBase == null) throw new ArgumentNullException(nameof(securityPathBase));

            this.securityRule = securityRule ?? throw new ArgumentNullException(nameof(securityRule));

            this.securityExpressionBuilder = securityExpressionBuilderFactory.CreateBuilder(securityPathBase);

            this.lazyFilter = LazyHelper.Create(() => this.securityExpressionBuilder.GetFilter(securityRule));
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
                           SecurityRule = this.securityRule, DomainObjectInfo = (domainObject, typeof(TDomainObject))
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
