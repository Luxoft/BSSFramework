using Framework.Application.Events;
using Framework.BLL.Events.SubscriptionManager;
using Framework.Core;
using Framework.Database;
using Framework.Database.DALListener;

namespace Framework.BLL.Events;

/// <summary>
/// Base class for DAL events
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public class DependencyDetailEventDALListener<TPersistentDomainObjectBase>(
    IEventDTOMessageSender<TPersistentDomainObjectBase> messageSender,
    EventDALListenerSettings<TPersistentDomainObjectBase>? settings = null)
    : IBeforeTransactionCompletedDALListener, IEventOperationReceiver
    where TPersistentDomainObjectBase : class
{
    private readonly EventDALListenerSettings<TPersistentDomainObjectBase> settings = settings ?? new EventDALListenerSettings<TPersistentDomainObjectBase>();

    /// <inheritdoc />
    public async Task Process(DALChangesEventArgs eventArgs, CancellationToken ct)
    {
        if (!this.settings.TypeEvents.Any())
        {
            return;
        }

        // filter changes by the types that have a subscription
        var joinItems = eventArgs.Changes.GroupDALObjectByType().Join(this.settings.TypeEvents, z => z.Key, z => z.Type, ValueTuple.Create);

        // match the changed object with its change description (save, remove) and the function that determines whether the event needs to be sent
        var values = joinItems.SelectMany(
                                  z =>
                                      new[]
                                      {
                                          ValueTuple.Create(
                                              z.Item1.Value.CreatedItems.Concat(z.Item1.Value.UpdatedItems),
                                              EventOperation.Save,
                                              z.Item2.IsSaveProcessingFunc),
                                          ValueTuple.Create(
                                              (IEnumerable<IDALObject>)z.Item1.Value.RemovedItems,
                                              EventOperation.Remove,
                                              z.Item2.IsRemoveProcessingFunc)
                                      })
                              .ToList();

        // apply the function that determines whether the event needs to be sent
        var allFilteredValues = values.SelectMany(z => z.Item1.Where(q => z.Item3(q.Object)).Select(q => ValueTuple.Create(q, z.Item2)))
                                      .ToList();

        // group by the order in which commit info of the given entity was received (applied to the database)
        // there may be nuances if the entity was first created in the database and then updated - this set will contain two records with different indexes
        // but in fact with the same set of fields (the reference will be the same)
        // for now: all such cases should be resolved in specific systems in a specific way
        // is hard to write in the general case for now.
        var allFilteredOrderedValues = allFilteredValues.OrderBy(z => z.Item1.ApplyIndex);

        var processedValues = this.ProcessFinalAllFilteredOrderedValues(eventArgs, allFilteredOrderedValues);

        // send to external systems.
        foreach (var item in processedValues)
        {
            var domainObject = (TPersistentDomainObjectBase)item.Item1.Object;
            var domainObjectType = item.Item1.Type;
            var eventType = item.Item2;

            var message = new DomainOperationSerializeData<TPersistentDomainObjectBase>
            {
                DomainObject = domainObject,
                Operation = eventType,
                CustomDomainObjectType = domainObjectType
            };

            await messageSender.SendAsync(message, ct);
        }
    }

    protected virtual IEnumerable<ValueTuple<IDALObject, EventOperation>> ProcessFinalAllFilteredOrderedValues(
        DALChangesEventArgs eventArgs,
        IEnumerable<ValueTuple<IDALObject, EventOperation>> allFilteredOrderedValues)
    {
        var joinItems = eventArgs.Changes.GroupDALObjectByType().Join(
            this.settings.Dependencies,
            z => z.Key,
            z => z.SourceTypeEvent.Type,
            ValueTuple.Create).ToList();

        if (!joinItems.Any())
        {
            return allFilteredOrderedValues;
        }

        // potential target objects to add
        var targetObjectCandidates = joinItems.SelectMany(
                                                  z =>
                                                      z.Item1.Value.CreatedItems.Concat(z.Item1.Value.UpdatedItems).Where(
                                                           q => z.Item2.SourceTypeEvent.IsSaveProcessingFunc(q.Object))
                                                       .Concat(
                                                           z.Item1.Value.RemovedItems.Where(
                                                               q => z.Item2.SourceTypeEvent.IsRemoveProcessingFunc(q.Object)))
                                                       .Select(
                                                           q => new
                                                           {
                                                               TargetObject = z.Item2.GetTargetValue(q.Object),
                                                               TargetObjectType = z.Item2.TargetTypeEvent.Type,
                                                           })
                                                       // TargetObject can be null if the target entity was cascade-deleted in the same transaction
                                                       // and its navigation property is not loaded (relevant for EF Core cascade delete)
                                                       .Where(
                                                           q => q.TargetObject is not null
                                                                && z.Item2.TargetTypeEvent.IsSaveProcessingFunc(q.TargetObject)))
                                              .Distinct(z => z.TargetObject)
                                              .ToList();

        // find those targets that are not in the set yet
        var allTargetObjects =
            this.settings.Dependencies.Select(z => z.TargetTypeEvent.Type)
                .Select(
                    z =>
                    {
                        eventArgs.Changes.GroupDALObjectByType().TryGetValue(z, out var dalChanges);
                        return ValueTuple.Create(z, dalChanges);
                    })
                .Where(z => z.Item2 is not null)
                .Select(z => z.Item2!)
                .SelectMany(z => z.CreatedItems.Concat(z.UpdatedItems).Concat(z.RemovedItems).Select(q => q.Object))
                .ToHashSet();

        var allAbsentsTargetObjects = targetObjectCandidates.Except(
            allTargetObjects,
            (anon, alwaysObject) => anon.TargetObject.Equals(alwaysObject)).ToList();

        // filter those that need processing
        return allFilteredOrderedValues.Concat(
            allAbsentsTargetObjects.Select(
                z => ValueTuple.Create((IDALObject)(new DALObject(z.TargetObject, z.TargetObjectType, 1)), EventOperation.Save)));
    }

    async Task IEventOperationReceiver.Receive<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent, CancellationToken ct)
    {
        var dalChanges = GetDALChanges(domainObject, domainObjectEvent);

        if (dalChanges is not null)
        {
            await this.Process(new DALChangesEventArgs(dalChanges), ct);
        }
    }

    private static DALChanges? GetDALChanges<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
        where TDomainObject : class
    {
        if (domainObjectEvent == EventOperation.Save)
        {
            return new DALChanges([], [new DALObject(domainObject, typeof(TDomainObject), 0)], []);
        }
        else if (domainObjectEvent == EventOperation.Remove)
        {
            return new DALChanges([], [], [new DALObject(domainObject, typeof(TDomainObject), 0)]);
        }
        else
        {
            return null;
        }
    }
}
