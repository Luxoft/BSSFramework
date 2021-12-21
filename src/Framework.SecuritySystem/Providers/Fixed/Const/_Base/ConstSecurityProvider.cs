using Framework.Core;

namespace Framework.SecuritySystem
{
    public class ConstSecurityProvider<TDomainObject> : FixedSecurityProvider<TDomainObject>
        where TDomainObject : class
    {
        private readonly bool hasAccess;

        public ConstSecurityProvider(IAccessDeniedExceptionService<TDomainObject> accessDeniedExceptionService, bool hasAccess)
            : base(accessDeniedExceptionService)
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
