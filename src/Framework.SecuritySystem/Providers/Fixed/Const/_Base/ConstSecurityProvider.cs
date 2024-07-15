namespace Framework.SecuritySystem
{
    public class ConstSecurityProvider<TDomainObject>(bool hasAccess) : FixedSecurityProvider<TDomainObject>
    {
        protected sealed override bool HasAccess()
        {
            return hasAccess;
        }

        public override SecurityAccessorData GetAccessorData(TDomainObject domainObject)
        {
            return hasAccess ? SecurityAccessorData.Infinity : SecurityAccessorData.Empty;
        }
    }
}
