using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public class BLLSecurityModeAttribute : Attribute
{
    public BLLSecurityModeAttribute(SecurityRule securityRule)
    {
        this.SecurityMode = securityRule;
    }


    public SecurityRule SecurityMode { get; private set; }
}
