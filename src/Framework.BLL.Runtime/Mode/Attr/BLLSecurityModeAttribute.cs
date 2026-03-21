using SecuritySystem;

namespace Framework.BLL.Mode.Attr;

public class BLLSecurityModeAttribute : Attribute
{
    public BLLSecurityModeAttribute(SecurityRule securityRule)
    {
        this.SecurityMode = securityRule;
    }


    public SecurityRule SecurityMode { get; private set; }
}
