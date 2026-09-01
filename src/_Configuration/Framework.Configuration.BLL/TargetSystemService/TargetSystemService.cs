using Anch.Core;

using Framework.Application.Domain;
using Framework.Application.Events;
using Framework.BLL;
using Framework.BLL.Domain.TargetSystem;
using Framework.Configuration.Domain;
using Framework.Core.TypeResolving;
using Framework.Database;
using Framework.Database.Audit;
using Framework.Database.Domain;
using Framework.Subscriptions;

namespace Framework.Configuration.BLL.TargetSystemService;

public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase>(
    TBLLContext context,
    PersistentTargetSystemInfo targetSystemInfo,
    IEventOperationSender eventOperationSender,
    ISubscriptionResolver subscriptionResolver,
    TargetSystem targetSystem,
    IRevisionService revisionService) : ITargetSystemService

    where TBLLContext : class, ISecurityBLLContext<TPersistentDomainObjectBase, Guid>
    where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    public TargetSystem TargetSystem { get; } = targetSystem;

    public ITypeResolver<DomainType> TypeResolver => field ??= targetSystemInfo.Domain.TypeResolver.OverrideInput((DomainType v) => v);

    public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

    public Task ForceEventAsync(DomainTypeEventModel eventModel, CancellationToken ct)
    {
        var domainType = this.TypeResolver.Resolve(eventModel.Operation.DomainType);

        return new Func<DomainTypeEventModel, CancellationToken, Task>(this.ForceEvent<TPersistentDomainObjectBase>)
               .CreateGenericMethod(domainType)
               .Invoke<Task>(this, [eventModel, ct]);
    }

    private async Task ForceEvent<TDomainObject>(DomainTypeEventModel eventModel, CancellationToken ct)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        var bll = context.Logics.Default.Create<TDomainObject>();

        foreach (var domainObjectId in eventModel.DomainObjectIdents)
        {
            var actualRevision = eventModel.Revision is null && eventModel.Operation.Name == EventOperation.Remove.Name
                                     ? bll.GetObjectRevisions(domainObjectId).RevisionInfos.Select(v => v.RevisionNumber).OrderByDescending(v => v).Skip(1)
                                          .First()
                                     : eventModel.Revision;

            var domainObject = actualRevision is null ? bll.GetById(domainObjectId, true)! : bll.GetObjectByRevision(domainObjectId, actualRevision.Value);

            var domainObjectEvent = new EventOperation(eventModel.Operation.Name);

            await eventOperationSender.Send(domainObject, domainObjectEvent, ct);
        }
    }

    public bool IsAssignable(Type domainType) => typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainType);

    /// <summary>
    ///     Возвращает данные об изменениях доменного объекта.
    /// </summary>
    /// <param name="changes">Описатель операций, проведенных над объектом в слое доступа к данным.</param>
    /// <returns>Экземпляр <see cref="IEnumerable{ObjectModificationInfo}" />.</returns>
    /// <exception cref="ArgumentNullException">Аргумент changes равен null.</exception>
    public IEnumerable<ObjectModificationInfo<Guid>> GetObjectModifications(DALChanges changes)
    {
        var revisionNumber = revisionService.GetCurrentRevision();

        if (revisionNumber != 0)
        {
            foreach (var item in changes.GetSubset(typeof(TPersistentDomainObjectBase)).ToChangeTypeDict())
            {
                if (subscriptionResolver.DomainTypes.Contains(item.Key.Type))
                {
                    yield return new ObjectModificationInfo<Guid>(
                        Identity: ((TPersistentDomainObjectBase)item.Key.Object).Id,
                        Revision: revisionNumber,
                        ModificationType: item.Value.ToModificationType(),
                        TypeInfo: item.Key.Type);
                }
            }
        }
    }
}
