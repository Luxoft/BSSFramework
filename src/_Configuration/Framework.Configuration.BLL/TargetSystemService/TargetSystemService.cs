using CommonFramework;

using Framework.Application.Domain;
using Framework.Application.Events;
using Framework.BLL;
using Framework.BLL.Domain.TargetSystem;
using Framework.BLL.Services;
using Framework.Configuration.Domain;
using Framework.Core.TypeResolving;
using Framework.Database;
using Framework.Database.Domain;
using Framework.Subscriptions;

namespace Framework.Configuration.BLL.TargetSystemService;

public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase>(
    TBLLContext context,
    PersistentTargetSystemInfo targetSystemInfo,
    IEventOperationSender eventOperationSender,
    ISubscriptionResolver subscriptionResolver,
    TargetSystem targetSystem,
    ICurrentRevisionService currentRevisionService) : ITargetSystemService

    where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
    where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    public TargetSystem TargetSystem { get; } = targetSystem;

    public ITypeResolver<DomainType> TypeResolver => field ??= targetSystemInfo.Domain.TypeResolver.OverrideInput((DomainType v) => v);

    public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

    public Task ForceEventAsync(DomainTypeEventModel eventModel, CancellationToken cancellationToken)
    {
        var domainType = this.TypeResolver.Resolve(eventModel.Operation.DomainType);

        return new Func<DomainTypeEventModel, CancellationToken, Task>(this.ForceEvent<TPersistentDomainObjectBase>)
               .CreateGenericMethod(domainType)
               .Invoke<Task>(this, [eventModel, cancellationToken]);
    }

    private async Task ForceEvent<TDomainObject>(DomainTypeEventModel eventModel, CancellationToken cancellationToken)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        var bll = context.Logics.Default.Create<TDomainObject>();

        foreach (var domainObjectId in eventModel.DomainObjectIdents)
        {
            var actualRevision = eventModel.Revision == null && eventModel.Operation.Name == EventOperation.Remove.Name
                                     ? bll.GetObjectRevisions(domainObjectId).RevisionInfos.Select(v => v.RevisionNumber).OrderByDescending(v => v).Skip(1).First()
                                     : eventModel.Revision;

            var domainObject = actualRevision == null ? bll.GetById(domainObjectId, true) : bll.GetObjectByRevision(domainObjectId, actualRevision.Value);

            var domainObjectEvent = new EventOperation(eventModel.Operation.Name);

            await eventOperationSender.Send(domainObject, domainObjectEvent, cancellationToken);
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
        var revisionNumber = currentRevisionService.GetCurrentRevision();

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
