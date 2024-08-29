using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using NHibernate.Linq;

namespace Framework.Configuration.NamedLocks;

public class NamedLockInitializer(
    [DisabledSecurity] IRepository<GenericNamedLock> namedLockRepository,
    INamedLockSource namedLockSource)
    : INamedLockInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        var dbValues = await namedLockRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeResult = dbValues.GetMergeResult(namedLockSource.NamedLocks, v => v.Name, v => v.Name);

        foreach (var addingItem in mergeResult.AddingItems)
        {
            await namedLockRepository.SaveAsync(new GenericNamedLock { Name = addingItem.Name }, cancellationToken);
        }

        foreach (var removingItem in mergeResult.RemovingItems)
        {
            await namedLockRepository.RemoveAsync(removingItem, cancellationToken);
        }
    }
}
