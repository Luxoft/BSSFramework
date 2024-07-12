namespace Framework.SecuritySystem
{
    public class ConstSecurityProvider<TDomainObject>(bool hasAccess) : FixedSecurityProvider<TDomainObject>
    {
        protected sealed override bool HasAccess()
        {
            return hasAccess;
        }

        public override SecurityAccessorResult GetAccessors(TDomainObject domainObject)
        {
            return hasAccess ? SecurityAccessorResult.Infinity : SecurityAccessorResult.Empty;
        }
    }
}
