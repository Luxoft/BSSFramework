using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using SampleSystem.Domain;

namespace SampleSystem.BLL
{
    public partial class SampleSystemSecurityService
    {
        public override ISecurityProvider<TDomainObject> GetAttachmentSecurityProvider<TDomainObject>(BLLSecurityMode securityMode)
        {
            if (typeof(TDomainObject) == typeof(Country))
            {
                if (securityMode == BLLSecurityMode.View)
                {
                    return this.GetSecurityProvider<TDomainObject>(SampleSystemSecurityOperation.CountryViewAttachment);
                }
                else if (securityMode == BLLSecurityMode.Edit)
                {
                    return this.GetSecurityProvider<TDomainObject>(SampleSystemSecurityOperation.CountryEditAttachment);
                }
            }

            return base.GetAttachmentSecurityProvider<TDomainObject>(securityMode);
        }
    }
}
