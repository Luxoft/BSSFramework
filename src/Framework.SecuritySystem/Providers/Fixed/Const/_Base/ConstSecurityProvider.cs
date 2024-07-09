using Framework.Core;

namespace Framework.SecuritySystem
{
    public class ConstSecurityProvider<TDomainObject>(bool hasAccess) : FixedSecurityProvider<TDomainObject>
    {
        protected sealed override bool HasAccess()
        {
            return hasAccess;
        }

        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            return hasAccess ? UnboundedList<string>.Infinity : UnboundedList<string>.Empty;
        }
    }
}
