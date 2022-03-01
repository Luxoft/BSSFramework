using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.SecuritySystem;

using WorkflowSampleSystem.Domain;

namespace WorkflowSampleSystem.BLL
{
    public partial class WorkflowSampleSystemSecurityService
    {
        public override ISecurityProvider<TDomainObject> GetAttachmentSecurityProvider<TDomainObject>(BLLSecurityMode securityMode)
        {
            if (typeof(TDomainObject) == typeof(Country))
            {
                if (securityMode == BLLSecurityMode.View)
                {
                    return this.GetSecurityProvider<TDomainObject>(WorkflowSampleSystemSecurityOperation.CountryViewAttachment);
                }
                else if (securityMode == BLLSecurityMode.Edit)
                {
                    return this.GetSecurityProvider<TDomainObject>(WorkflowSampleSystemSecurityOperation.CountryEditAttachment);
                }
            }

            return base.GetAttachmentSecurityProvider<TDomainObject>(securityMode);
        }
    }
}
