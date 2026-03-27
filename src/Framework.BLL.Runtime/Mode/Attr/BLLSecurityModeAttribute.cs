using SecuritySystem;

namespace Framework.BLL.Mode.Attr;

public class BLLSecurityModeAttribute(SecurityRule securityRule) : Attribute
{
    public SecurityRule SecurityMode { get; private set; } = securityRule;
}
