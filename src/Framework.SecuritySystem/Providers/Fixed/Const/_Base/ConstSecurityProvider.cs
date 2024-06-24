using Framework.Core;

namespace Framework.SecuritySystem
{
    public class ConstSecurityProvider<TDomainObject> : FixedSecurityProvider<TDomainObject>
    {
        private readonly bool hasAccess;

        public ConstSecurityProvider(bool hasAccess)
        {
            this.hasAccess = hasAccess;
        }


        protected sealed override bool HasAccess()
        {
            return this.hasAccess;
        }

        public override UnboundedList<string> GetAccessors(TDomainObject domainObject)
        {
            return this.hasAccess ? UnboundedList<string>.Infinity : UnboundedList<string>.Empty;
        }
    }
}
