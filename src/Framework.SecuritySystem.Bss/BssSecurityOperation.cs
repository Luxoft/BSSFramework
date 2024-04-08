namespace Framework.SecuritySystem.Bss;

public static class BssSecurityOperation
{
    public static SecurityOperation SystemIntegration { get; } = new(nameof(SystemIntegration)) { Description = "Can integrate" };
}
