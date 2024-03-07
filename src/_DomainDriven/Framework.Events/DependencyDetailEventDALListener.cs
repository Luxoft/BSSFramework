using Framework.Core;
using Framework.DomainDriven;

namespace Framework.Events;

public abstract class DependencyDetailEventDALListener<TPersistentDomainObjectBase> : EventDALListener<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class
{
    private readonly TypeEventDependency[] dependencies;

    protected DependencyDetailEventDALListener(IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>> messageSender, IEnumerable<TypeEvent> typeEvents, IEnumerable<TypeEventDependency> dependencies)
            : base(context, messageSender, typeEvents)
    {
        this.dependencies = (dependencies ?? throw new ArgumentNullException(nameof(dependencies))).ToArray();
    }

    protected override IEnumerable<ValueTuple<IDALObject, EventOperation>> ProcessFinalAllFilteredOrderedValues(
            DALChangesEventArgs eventArgs,
            IEnumerable<ValueTuple<IDALObject, EventOperation>> allFilteredOrderedValues)
    {
        var joinItems = eventArgs.Changes.GroupDALObjectByType().Join(this.dependencies, z => z.Key, z => z.SourceTypeEvent.Type, ValueTuple.Create).ToList();

        if (!joinItems.Any())
        {
            return base.ProcessFinalAllFilteredOrderedValues(eventArgs, allFilteredOrderedValues);
        }

        // потенциальные target-объекты для добавления
        var targetObjectCanditates = joinItems.SelectMany(
                                                          z =>
                                                                  z.Item1.Value.CreatedItems.Concat(z.Item1.Value.UpdatedItems).Where(q => z.Item2.SourceTypeEvent.IsSaveProcessingFunc(q.Object))
                                                                   .Concat(z.Item1.Value.RemovedItems.Where(q => z.Item2.SourceTypeEvent.IsRemoveProcessingFunc(q.Object)))
                                                                   .Where(q => z.Item2.TargetTypeEvent.IsSaveProcessingFunc(z.Item2.GetTargetValue(q.Object)))
                                                                   .Select(q => new
                                                                                {
                                                                                        TargetObject = z.Item2.GetTargetValue(q.Object),
                                                                                        TargetObjectType = z.Item2.TargetTypeEvent.Type,
                                                                                }))
                                              .Distinct(z => z.TargetObject)
                                              .ToList();

        // находим те target, которых еще нет в наборе
        var allTargetObjects =
                this.dependencies.Select(z => z.TargetTypeEvent.Type)
                    .Select(z =>
                            {
                                DALChanges<IDALObject> dalChanges;
                                eventArgs.Changes.GroupDALObjectByType().TryGetValue(z, out dalChanges);
                                return ValueTuple.Create(z, dalChanges);
                            })
                    .Where(z => z.Item2 != null)
                    .Select(z => z.Item2)
                    .SelectMany(z => z.CreatedItems.Concat(z.UpdatedItems).Concat(z.RemovedItems).Select(q => q.Object))
                    .ToHashSet();

        var allAbsentsTargetObjects = targetObjectCanditates.Except(
                                                                    allTargetObjects,
                                                                    (anon, alwaysObject) => anon.TargetObject.Equals(alwaysObject)).ToList();

        // фильруем которые нужно обработать
        return allFilteredOrderedValues.Concat(allAbsentsTargetObjects.Select(z => ValueTuple.Create((IDALObject)(new DALObject(z.TargetObject, z.TargetObjectType, 1)), EventOperation.Save)));
    }
}
