using Framework.Application.Lock;

namespace Framework.Configuration.Environment;

public static class ConfigurationNamedLock
{
    public static NamedLock UpdateDomainTypeLock { get; } = new (nameof(UpdateDomainTypeLock), typeof(DomainType));

    public static NamedLock UpdateSequence { get; } = new (nameof(UpdateSequence), typeof(Sequence));

    public static NamedLock ProcessModifications { get; } = new (nameof(ProcessModifications), typeof(DomainObjectModification));
}
