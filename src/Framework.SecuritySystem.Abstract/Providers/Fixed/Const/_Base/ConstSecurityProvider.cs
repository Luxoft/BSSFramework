namespace Framework.SecuritySystem;

public class ConstSecurityProvider<TDomainObject>(bool hasAccess) : FixedSecurityProvider<TDomainObject>
{
    protected sealed override bool HasAccess() => hasAccess;

    public override SecurityAccessorData GetAccessorData(TDomainObject domainObject) =>
        hasAccess ? SecurityAccessorData.Infinity : SecurityAccessorData.Empty;
}
