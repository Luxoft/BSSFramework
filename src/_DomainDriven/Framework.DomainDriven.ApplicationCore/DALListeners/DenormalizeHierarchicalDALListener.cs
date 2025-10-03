using CommonFramework;

using Framework.Core;
using Framework.DomainDriven.Lock;
using Framework.Persistent;

using SecuritySystem;
using SecuritySystem.AncestorDenormalization;
using SecuritySystem.HierarchicalExpand;

namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class DenormalizeHierarchicalDALListener(
    IEnumerable<HierarchicalInfo> hierarchicalInfoList,
    IDenormalizedAncestorsServiceFactory denormalizedAncestorsServiceFactory,
    INamedLockSource namedLockSource,
    INamedLockService namedLockService)
    : IBeforeTransactionCompletedDALListener
{
    private readonly IReadOnlyDictionary<Type, HierarchicalInfo> hierarchicalInfoTypes = hierarchicalInfoList.ToDictionary(h => h.DomainObjectType);

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

                var method = new Func<ISecurityContext[], ISecurityContext[], HierarchicalInfo<ISecurityContext>, CancellationToken, Task>(this.Process).CreateGenericMethod(domainType);

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

    private async Task Process<TDomainObject>(TDomainObject[] modified, TDomainObject[] removing, HierarchicalInfo<TDomainObject> hierarchicalInfo, CancellationToken cancellationToken)
        where TDomainObject : class
    {
        await this.LockChanges(hierarchicalInfo, cancellationToken);

        modified.Foreach(domainObject => this.UpdateDeepLevel(domainObject, hierarchicalInfo));

        await denormalizedAncestorsServiceFactory.Create<TDomainObject>().SyncAsync(modified, removing, cancellationToken);
    }

    private void UpdateDeepLevel<TDomainObject>(TDomainObject domainObject, HierarchicalInfo<TDomainObject> hierarchicalInfo)
        where TDomainObject : class
    {
        if (domainObject is IHierarchicalLevelObjectDenormalized objectDenormalized)
        {
            objectDenormalized.SetDeepLevel(domainObject.GetAllElements(v => hierarchicalInfo.ParentFunc(v)).Count());
        }
    }

    private async Task LockChanges(HierarchicalInfo hierarchicalInfo, CancellationToken cancellationToken)
    {
        var domainObjectAncestorLinkType = hierarchicalInfo.DirectedLinkType;

        var namedLock = namedLockSource.NamedLocks.Where(nl => nl.DomainType == domainObjectAncestorLinkType).Single(
            () => new ArgumentException($"System must have namedLock for {domainObjectAncestorLinkType.Name} global lock "));

        await namedLockService.LockAsync(namedLock, LockRole.Update, cancellationToken);
    }
}
