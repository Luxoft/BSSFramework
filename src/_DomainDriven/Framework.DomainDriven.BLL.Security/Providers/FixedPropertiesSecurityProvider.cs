﻿using System.Linq.Expressions;

using Framework.DomainDriven.Tracking;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security
{
    internal class FixedPropertiesSecurityProvider<TBLLContext, TDomainObject> : ISecurityProvider<TDomainObject>
        where TBLLContext : class, IAccessDeniedExceptionServiceContainer, ITrackingServiceContainer<TDomainObject>
        where TDomainObject : class
    {
        private readonly ISecurityProvider<TDomainObject> baseSecurityProvider;

        private readonly Expression<Func<TDomainObject, object>>[] allowedPropertiesForChangingExpressions;


        public FixedPropertiesSecurityProvider(TBLLContext context, ISecurityProvider<TDomainObject> baseSecurityProvider, Expression<Func<TDomainObject, object>>[] allowedPropertiesForChangingExpressions)
        {
            this.Context = context;
            this.baseSecurityProvider = baseSecurityProvider;
            this.allowedPropertiesForChangingExpressions = allowedPropertiesForChangingExpressions;
        }

        public TBLLContext Context { get; }


        public IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
        {
            return this.baseSecurityProvider.InjectFilter(queryable);
        }

        public bool HasAccess(TDomainObject domainObject)
        {
            return this.baseSecurityProvider.HasAccess(domainObject)
                   && !this.Context.TrackingService.GetChanges(domainObject).GetUnexpectedChangedProprties(this.allowedPropertiesForChangingExpressions).Any();
        }

        public SecurityAccessorData GetAccessorData(TDomainObject domainObject)
        {
            return this.baseSecurityProvider.GetAccessorData(domainObject);
        }
    }
}
