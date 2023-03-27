using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public class BLLSecurityModeAttribute : Attribute
{
    public BLLSecurityModeAttribute(BLLSecurityMode securityMode)
    {
        this.SecurityMode = securityMode;
    }


    public BLLSecurityMode SecurityMode { get; private set; }
}
