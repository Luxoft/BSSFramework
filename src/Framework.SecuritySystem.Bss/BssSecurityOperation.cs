using Framework.SecuritySystem;

namespace Framework.Core;

public static class BssSecurityOperation
{
    public static SecurityOperation<Guid> SystemIntegration { get; } = new(nameof(SystemIntegration), new Guid("0ba8a6b0-43b9-4f59-90ce-2fcbe37b97c9")) { AdminHasAccess = false, Description = "Can integrate" };
}
