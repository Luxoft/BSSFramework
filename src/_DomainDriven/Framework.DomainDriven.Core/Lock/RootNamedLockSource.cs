using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.Lock;

public class RootNamedLockSource([FromKeyedServices(RootNamedLockSource.ElementsKey)]IEnumerable<INamedLockSource> elements) : INamedLockSource
{
    public const string ElementsKey = "Elements";

    public IReadOnlyList<NamedLock> NamedLocks { get; } = elements.SelectMany(el => el.NamedLocks).ToList();
}
