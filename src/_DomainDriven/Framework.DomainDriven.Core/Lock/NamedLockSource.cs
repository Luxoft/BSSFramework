using Framework.Core;

namespace Framework.DomainDriven.Lock;

public class NamedLockSource(IEnumerable<NamedLockTypeInfo> typeInfoList) : INamedLockSource
{
    public IReadOnlyList<NamedLock> NamedLocks { get; } =
        typeInfoList.SelectMany(typeInfo => typeInfo.NamedLockType.GetStaticPropertyValueList<NamedLock>()).ToList();
}
