using CommonFramework;

using Framework.Core;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem;
using SecuritySystem.HierarchicalExpand;
using SecuritySystem.Services;

namespace Framework.DomainDriven.ApplicationCore.DALListeners;

public class DenormalizeHierarchicalDALListener(IEnumerable<HierarchicalInfo> hierarchicalInfoList, IDenormalizedAncestorsServiceFactory denormalizedAncestorsServiceFactory)
    : IBeforeTransactionCompletedDALListener
{
    private readonly IReadOnlySet<Type> hierarchicalInfoTypes = hierarchicalInfoList.Select(h => h.DomainObjectType).ToHashSet();

    public async Task Process(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        foreach (var typeGroup in eventArgs.Changes.GroupByType())
        {
            var domainType = typeGroup.Key;

            if (this.hierarchicalInfoTypes.Contains(domainType))
            {
                var values = typeGroup.Value.ToChangeTypeDict().Partial(
                    pair => pair.Value == DALObjectChangeType.Created || pair.Value == DALObjectChangeType.Updated,
                    (modified, removing) => new { Modified = modified, Removing = removing });

                var method = new Func<ISecurityContext[], ISecurityContext[], CancellationToken, Task>(this.Process).CreateGenericMethod(domainType);

                await method.Invoke<Task>(
                    this,
                    [values.Modified.Select(z => z.Key).ToArray(domainType), values.Removing.Select(z => z.Key).ToArray(domainType), cancellationToken]);
            }
        }
    }

    private async Task Process<TDomainObject>(TDomainObject[] modified, TDomainObject[] removing, CancellationToken cancellationToken)
    {
        await denormalizedAncestorsServiceFactory.Create<TDomainObject>().SyncAsync(modified, removing, cancellationToken);
    }
}