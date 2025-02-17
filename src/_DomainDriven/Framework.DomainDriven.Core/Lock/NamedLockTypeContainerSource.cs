using Framework.Core;

namespace Framework.DomainDriven.Lock;

public class NamedLockTypeContainerSource(Type typeInfo) : INamedLockSource
{
    public IReadOnlyList<NamedLock> NamedLocks { get; } = typeInfo.GetStaticPropertyValueList<NamedLock>().ToList();
}
