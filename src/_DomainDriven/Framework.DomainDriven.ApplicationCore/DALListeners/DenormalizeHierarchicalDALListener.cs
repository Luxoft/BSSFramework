using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.Persistent;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.AncestorDenormalization;
using SecuritySystem.HierarchicalExpand;

namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class DenormalizeHierarchicalDALListener(
    IServiceProvider serviceProvider,
    IEnumerable<FullAncestorLinkInfo> hierarchicalInfoList,
    IDenormalizedAncestorsServiceFactory denormalizedAncestorsServiceFactory,
    INamedLockSource namedLockSource,
    INamedLockService namedLockService)
    : IBeforeTransactionCompletedDALListener
{
    private readonly IReadOnlyDictionary<Type, FullAncestorLinkInfo> hierarchicalInfoTypes = hierarchicalInfoList.ToDictionary(h => h.DomainObjectType);

    public async Task Process(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        foreach (var typeGroup in eventArgs.Changes.GroupByType())
        {
            var domainType = typeGroup.Key;

            if (this.hierarchicalInfoTypes.TryGetValue(domainType, out var hierarchicalInfo))
            {
                var values = typeGroup.Value.ToChangeTypeDict().Partial(
                    pair => pair.Value == DALObjectChangeType.Created || pair.Value == DALObjectChangeType.Updated,
                    (modified, removing) => new { Modified = modified, Removing = removing });

                var method = new Func<ISecurityContext[], ISecurityContext[], FullAncestorLinkInfo<ISecurityContext>, CancellationToken, Task>(this.Process).CreateGenericMethod(domainType);

                await method.Invoke<Task>(
                    this,
                    [
                        values.Modified.Select(z => z.Key).ToArray(domainType),
                        values.Removing.Select(z => z.Key).ToArray(domainType),
                        hierarchicalInfo,
                        cancellationToken
                    ]);
            }
        }
    }

    private async Task Process<TDomainObject>(TDomainObject[] modified, TDomainObject[] removing, FullAncestorLinkInfo<TDomainObject> fullAncestorLinkInfo, CancellationToken cancellationToken)
        where TDomainObject : class
    {
        await this.LockChanges(fullAncestorLinkInfo, cancellationToken);

        if (serviceProvider.GetService(typeof(DeepLevelInfo<>).MakeGenericType(typeof(TDomainObject))) != null)
        {
            await serviceProvider.GetRequiredService<IUpdateDeepLevelService<TDomainObject>>().UpdateDeepLevels(modified, cancellationToken);
        }

        await denormalizedAncestorsServiceFactory.Create<TDomainObject>().SyncAsync(modified, removing, cancellationToken);
    }

    private async Task LockChanges(FullAncestorLinkInfo fullAncestorLinkInfo, CancellationToken cancellationToken)
    {
        var domainObjectAncestorLinkType = fullAncestorLinkInfo.DirectedLinkType;

        var namedLock = namedLockSource.NamedLocks.Where(nl => nl.DomainType == domainObjectAncestorLinkType).Single(
            () => new ArgumentException($"System must have namedLock for {domainObjectAncestorLinkType.Name} global lock "));

        await namedLockService.LockAsync(namedLock, LockRole.Update, cancellationToken);
    }
}
