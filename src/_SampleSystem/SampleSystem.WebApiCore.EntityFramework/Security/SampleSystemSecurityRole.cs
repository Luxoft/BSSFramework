using Anch.SecuritySystem;

namespace SampleSystem.WebApiCore.Security;

public static class SampleSystemSecurityRole
{
    public static SecurityRole SeManager { get; } = new("SE Manager");
}
