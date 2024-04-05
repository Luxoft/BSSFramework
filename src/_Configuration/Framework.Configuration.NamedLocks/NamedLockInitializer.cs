using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;

namespace Framework.Configuration.NamedLocks;

public class NamedLockInitializer : INamedLockInitializer
{
    private readonly IRepository<GenericNamedLock> namedLockRepository;

    private readonly INamedLockSource namedLockSource;

    public NamedLockInitializer(
        [FromKeyedServices(SecurityRule.Disabled)] IRepository<GenericNamedLock> namedLockRepository,
        INamedLockSource namedLockSource)
    {
        this.namedLockRepository = namedLockRepository;
        this.namedLockSource = namedLockSource;
    }

    public async Task Initialize(CancellationToken cancellationToken)
    {
        var dbValues = await this.namedLockRepository.GetQueryable().ToListAsync(cancellationToken);

        var mergeResult = dbValues.GetMergeResult(this.namedLockSource.NamedLocks, v => v.Name, v => v.Name);

        foreach (var addingItem in mergeResult.AddingItems)
        {
            await this.namedLockRepository.SaveAsync(new GenericNamedLock { Name = addingItem.Name }, cancellationToken);
        }

        foreach (var removingItem in mergeResult.RemovingItems)
        {
            await this.namedLockRepository.RemoveAsync(removingItem, cancellationToken);
        }
    }
}
