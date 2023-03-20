using System;
using System.Linq;
using System.Linq.Expressions;
using Framework.Core;
using Framework.DomainDriven.BLL.Tracking;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

internal class FixedPropertiesSecurityProvider<TBLLContext, TDomainObject> : SecurityProviderBase<TDomainObject>
        where TBLLContext : class, IAccessDeniedExceptionServiceContainer<TDomainObject>, ITrackingServiceContainer<TDomainObject>
        where TDomainObject : class
{
    private readonly ISecurityProvider<TDomainObject> baseSecurityProvider;

    private readonly Expression<Func<TDomainObject, object>>[] allowedPropertiesForChangingExpressions;


    public FixedPropertiesSecurityProvider(TBLLContext context, ISecurityProvider<TDomainObject> baseSecurityProvider, Expression<Func<TDomainObject, object>>[] allowedPropertiesForChangingExpressions)
            : base(context.AccessDeniedExceptionService)
    {
        this.Context = context;
        this.baseSecurityProvider = baseSecurityProvider;
        this.allowedPropertiesForChangingExpressions = allowedPropertiesForChangingExpressions;
    }

    public TBLLContext Context { get; }


    public override IQueryable<TDomainObject> InjectFilter(IQueryable<TDomainObject> queryable)
    {
        return this.baseSecurityProvider.InjectFilter(queryable);
    }

    public override bool HasAccess(TDomainObject domainObject)
    {
        return this.baseSecurityProvider.HasAccess(domainObject)
               && !this.Context.TrackingService.GetChanges(domainObject).GetUnexpectedChangedProprties(this.allowedPropertiesForChangingExpressions).Any();
    }

    public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
    {
        return this.baseSecurityProvider.GetAccessors(domainObject);
    }

    public override Exception GetAccessDeniedException(TDomainObject domainObject, Func<string, string> formatMessageFunc = null)
    {
        if (!this.baseSecurityProvider.HasAccess(domainObject))
        {
            return this.baseSecurityProvider.GetAccessDeniedException(domainObject, formatMessageFunc);
        }

        var unexpectedChanges = this.Context.TrackingService.GetChanges(domainObject).GetUnexpectedChangedProprties(this.allowedPropertiesForChangingExpressions)
                                    .Where(p => !typeof(TDomainObject).GetProperty(p.PropertyName, true).HasAttribute<SystemPropertyAttribute>())
                                    .ToArray();

        return this.baseSecurityProvider.GetAccessDeniedException(domainObject, message =>
                                                                                        $"{(formatMessageFunc == null ? message : formatMessageFunc(message))} (Can't changed properties: {unexpectedChanges.Join(", ", p => p.PropertyName)})");
    }
}
