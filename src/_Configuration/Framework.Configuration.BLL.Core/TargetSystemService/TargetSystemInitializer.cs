using CommonFramework;

using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Lock;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.Configuration.BLL;

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
        var bll = context.Logics.TargetSystem;

        var isBase = targetSystemInfo.Id == PersistentHelper.BaseTargetSystemId;

        var targetSystem = bll.GetById(targetSystemInfo.Id, false, context.FetchService.GetContainer<TargetSystem>(MainDTOType.RichDTO))
                           ?? new TargetSystem(isBase, targetSystemInfo.IsMain, targetSystemInfo.IsRevision)
                              {
                                  Name = targetSystemInfo.Name, SubscriptionEnabled = !isBase, Id = targetSystemInfo.Id
                              }.Self(bll.Insert);

        var mergeResult = targetSystem.DomainTypes.GetMergeResult(targetSystemInfo.DomainTypes, t => t.Id, t => t.Id);

        foreach (var newItem in mergeResult.AddingItems)
        {
            var newDomainType = new DomainType(targetSystem)
            {
                Id = newItem.Id,
                Name = newItem.Type.Name,
                NameSpace = newItem.Type.Namespace
            };

            if (!isBase)
            {
                context.EventOperationSource.GetEventOperations(newItem.Type).Foreach(value => new DomainTypeEventOperation(newDomainType, value));
            }

            context.Logics.DomainType.Insert(newDomainType);
        }

        foreach (var (domainType, (type, _)) in mergeResult.CombineItems)
        {
            var changedName = domainType.Name != type.Name || domainType.NameSpace != type.Namespace;

            var mergeEventResult = domainType.EventOperations.GetMergeResult(
                context.EventOperationSource.GetEventOperations(type), operation => operation.Name, value => value.ToString());

            if (changedName || (!isBase && !mergeEventResult.IsEmpty))
            {
                domainType.Name = type.Name;
                domainType.NameSpace = type.Namespace;

                if (!isBase)
                {
                    domainType.RemoveDetails(mergeEventResult.RemovingItems);

                    mergeEventResult.AddingItems.Foreach(value => new DomainTypeEventOperation(domainType, value));
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
