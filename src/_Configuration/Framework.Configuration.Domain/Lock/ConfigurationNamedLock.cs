using Framework.DomainDriven.Lock;

namespace Framework.Configuration.Domain;

public static class ConfigurationNamedLock
{
    public static NamedLock UpdateDomainTypeLock { get; } = new (typeof(DomainType));

    public static NamedLock UpdateSequence { get; } = new (typeof(Sequence));

    public static NamedLock ProcessModifications { get; } = new (typeof(DomainObjectModification));
}
