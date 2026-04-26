using Anch.Core;

using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.TargetSystem;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Database;
using Framework.Relations;

namespace Framework.Configuration.BLL.TargetSystemService;

public class TargetSystemInitializer(
    IConfigurationBLLContext context,
    IEnumerable<TargetSystemInfo> targetSystemInfoList) : ITargetSystemInitializer
{
    public async Task Initialize(CancellationToken cancellationToken)
    {
        await context.NamedLockService.LockAsync(ConfigurationNamedLock.UpdateDomainTypeLock, LockRole.Update, cancellationToken);

        foreach (var targetSystemInfo in targetSystemInfoList)
        {
            this.Register(targetSystemInfo);
        }
    }

    private void Register(TargetSystemInfo targetSystemInfo)
    {
        var persistentTargetSystemInfo = targetSystemInfo as PersistentTargetSystemInfo;

        var bll = context.Logics.TargetSystem;

        var isBase = targetSystemInfo.Id == TargetSystemInfo.Base.Id;

        var targetSystem = bll.GetById(targetSystemInfo.Id, false, new DTOFetchRule<TargetSystem>(MainDTOType.RichDTO))
                           ?? new TargetSystem(isBase, persistentTargetSystemInfo?.IsMain ?? false, persistentTargetSystemInfo?.IsRevision ?? false)
                              {
                                  Name = targetSystemInfo.Name, SubscriptionEnabled = !isBase, Id = targetSystemInfo.Id
                              }.Self(bll.Insert);

        var mergeResult = targetSystem.DomainTypes.GetMergeResult(targetSystemInfo.Domain.Types, t => t.Id, t => t.Id);

        foreach (var newItem in mergeResult.AddingItems)
        {
            var newDomainType = new DomainType(targetSystem) { Id = newItem.Id, Name = newItem.Type.Name, Namespace = newItem.Type.Namespace! };

            if (!isBase)
            {
                context.EventOperationSource.GetEventOperations(newItem.Type).Foreach(value => _ = new DomainTypeEventOperation(newDomainType) { Name = value.Name });
            }

            context.Logics.DomainType.Insert(newDomainType);
        }

        foreach (var (domainType, (type, _)) in mergeResult.CombineItems)
        {
            var changedName = domainType.Name != type.Name || domainType.Namespace != type.Namespace;

            var mergeEventResult = domainType.EventOperations.GetMergeResult(
                context.EventOperationSource.GetEventOperations(type),
                operation => operation.Name,
                value => value.ToString());

            if (changedName || (!isBase && !mergeEventResult.IsEmpty))
            {
                domainType.Name = type.Name;
                domainType.Namespace = type.Namespace!;

                if (!isBase)
                {
                    domainType.RemoveDetails(mergeEventResult.RemovingItems);

                    mergeEventResult.AddingItems.Foreach(value => _ = new DomainTypeEventOperation(domainType) { Name = value.Name });
                }

                bll.Save(targetSystem);
            }
        }

        if (mergeResult.RemovingItems.Any())
        {
            targetSystem.RemoveDetails(mergeResult.RemovingItems);
            bll.Save(targetSystem);
        }
    }
}
