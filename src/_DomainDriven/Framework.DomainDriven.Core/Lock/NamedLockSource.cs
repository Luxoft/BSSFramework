using System.Reflection;

namespace Framework.DomainDriven.Lock;

public class NamedLockSource : INamedLockSource
{
    public NamedLockSource(IEnumerable<NamedLockTypeInfo> typeInfoList)
    {
        var namedLocksRequest = from typeInfo in typeInfoList

                                from prop in typeInfo.NamedLockType.GetProperties(BindingFlags.Static | BindingFlags.Public)

                                where typeof(NamedLock).IsAssignableFrom(prop.PropertyType)

                                select (NamedLock)prop.GetValue(null);

        this.NamedLocks = namedLocksRequest.ToList();
    }

    public IReadOnlyList<NamedLock> NamedLocks { get; }
}
