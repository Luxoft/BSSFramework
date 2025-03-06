using Framework.SecuritySystem;

namespace SampleSystem.Security;

public static class SampleSystemSecurityRole
{
    public static SecurityRole SeManager { get; } = new("SE Manager");
}
