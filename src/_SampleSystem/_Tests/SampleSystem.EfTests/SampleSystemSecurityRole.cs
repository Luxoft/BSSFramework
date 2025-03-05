using Framework.SecuritySystem;

namespace SampleSystem.EfTests;

public static class SampleSystemSecurityRole
{
    public static SecurityRole SeManager { get; } = new("SE Manager");
}
