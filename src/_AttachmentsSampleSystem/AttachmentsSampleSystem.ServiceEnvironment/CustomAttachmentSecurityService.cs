using AttachmentsSampleSystem.BLL;
using AttachmentsSampleSystem.Domain;

using Framework.SecuritySystem;

namespace AttachmentsSampleSystem.ServiceEnvironment;

public class CustomAttachmentSecurityService : AttachmentsSampleSystemSecurityService
{
    public CustomAttachmentSecurityService(IAttachmentsSampleSystemBLLContext context) : base(context)
    {
    }

    public override ISecurityProvider<TDomainObject> GetSecurityProvider<TDomainObject>(BLLSecurityMode securityMode)
    {
        if (typeof(TDomainObject) == typeof(Location))
        {
            if (securityMode == BLLSecurityMode.View)
            {
                return this.GetSecurityProvider<TDomainObject>(AttachmentsSampleSystemSecurityOperation.LocationViewAttachment);
            }
            else if (securityMode == BLLSecurityMode.Edit)
            {
                return this.GetSecurityProvider<TDomainObject>(AttachmentsSampleSystemSecurityOperation.LocationEditAttachment);
            }
        }

        return base.GetSecurityProvider<TDomainObject>(securityMode);
    }
}
