using Framework.SecuritySystem;

namespace Framework.Core;

public static class BssSecurityOperation
{
    public static NonContextSecurityOperation<Guid> SystemIntegration { get; } = new(nameof(SystemIntegration), new Guid("{0BA8A6B0-43B9-4F59-90CE-2FCBE37B97C9}")) { AdminHasAccess = false };
}
