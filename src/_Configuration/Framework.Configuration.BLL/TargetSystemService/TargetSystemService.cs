using CommonFramework;

using Framework.Application.Domain;
using Framework.Application.Events;
using Framework.BLL;
using Framework.BLL.Domain.TargetSystem;
using Framework.BLL.Services;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.Database;
using Framework.Database.Domain;
using Framework.Subscriptions;

namespace Framework.Configuration.BLL.TargetSystemService;

public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase>(
    IConfigurationBLLContext context,
    TBLLContext targetSystemContext,
    PersistentTargetSystemInfo targetSystemInfo,
    IEventOperationSender eventOperationSender,
    ISubscriptionResolver subscriptionResolver,
    ICurrentRevisionService currentRevisionService) : ITargetSystemService

    where TBLLContext : class, ISecurityServiceContainer<IRootSecurityService>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
    where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
{
    public ITypeResolver<DomainType> TypeResolver { get; } = targetSystemInfo.TypeResolver.OverrideInput((DomainType domainType) => domainType.FullTypeName);

    public TargetSystem TargetSystem => field ??= context.Logics.TargetSystem.GetByName(targetSystemInfo.Name, true)!;

    public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

    public void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId)
    {
        if (operation == null) throw new ArgumentNullException(nameof(operation));
        if (domainObjectId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainObjectId));

        var domainType = this.TypeResolver.Resolve(operation.DomainType);

        new Action<string, long?, Guid>(this.ForceEvent<TPersistentDomainObjectBase>)
            .CreateGenericMethod(domainType)
            .Invoke(this, [operation.Name, revision, domainObjectId]);
    }

    private void ForceEvent<TDomainObject>(string operationName, long? revision, Guid domainObjectId)
        where TDomainObject : class, TPersistentDomainObjectBase
    {
        var bll = targetSystemContext.Logics.Default.Create<TDomainObject>();

        var domainObject = revision == null
                               ? bll.GetById(domainObjectId, true)
                               : bll.GetObjectByRevision(domainObjectId, revision.Value);

        var domainObjectEvent = new EventOperation(operationName);

        eventOperationSender.Send(domainObject, domainObjectEvent, CancellationToken.None).GetAwaiter().GetResult();
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
