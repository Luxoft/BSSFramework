using System.Linq.Expressions;

using Framework.Tracking;

using SecuritySystem.Providers;
using SecuritySystem.SecurityAccessor;

namespace Framework.BLL.Providers
{
    internal class FixedPropertiesSecurityProvider<TBLLContext, TDomainObject>(
        TBLLContext context,
        ISecurityProvider<TDomainObject> baseSecurityProvider,
        Expression<Func<TDomainObject, object>>[] allowedPropertiesForChangingExpressions)
        : ISecurityProvider<TDomainObject>
        where TBLLContext : class, IAccessDeniedExceptionServiceContainer, ITrackingServiceContainer<TDomainObject>
        where TDomainObject : class
    {
        public TBLLContext Context { get; } = context;

        public IQueryable<TDomainObject> Inject(IQueryable<TDomainObject> queryable) => baseSecurityProvider.Inject(queryable);

        public async ValueTask<bool> HasAccessAsync(TDomainObject domainObject, CancellationToken cancellationToken) =>
            await baseSecurityProvider.HasAccessAsync(domainObject, cancellationToken)
            && !this.Context.TrackingService.GetChanges(domainObject).GetUnexpectedChangedProprties(allowedPropertiesForChangingExpressions).Any();

        public ValueTask<SecurityAccessorData> GetAccessorDataAsync(TDomainObject domainObject, CancellationToken cancellationToken) =>
            baseSecurityProvider.GetAccessorDataAsync(domainObject, cancellationToken);
    }
}
