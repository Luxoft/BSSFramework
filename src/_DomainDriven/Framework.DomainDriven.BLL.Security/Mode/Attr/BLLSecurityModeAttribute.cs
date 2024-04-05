using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public class BLLSecurityModeAttribute : Attribute
{
    public BLLSecurityModeAttribute(SecurityRule securityMode)
    {
        this.SecurityMode = securityMode;
    }


    public SecurityRule SecurityMode { get; private set; }
}
