using Framework.Core;

namespace Framework.DomainDriven.Lock;

public class NamedLockSource : INamedLockSource
{
    public NamedLockSource(IEnumerable<NamedLockTypeInfo> typeInfoList)
    {
        this.NamedLocks = typeInfoList.SelectMany(typeInfo => typeInfo.NamedLockType.GetStaticPropertyValueList<NamedLock>()).ToList();
    }

    public IReadOnlyList<NamedLock> NamedLocks { get; }
}
