using Framework.Core;

namespace Framework.Application.Lock;

public class NamedLockTypeContainerSource(Type typeInfo) : INamedLockSource
{
    public IReadOnlyList<NamedLock> NamedLocks { get; } = typeInfo.GetStaticPropertyValueList<NamedLock>().ToList();
}
