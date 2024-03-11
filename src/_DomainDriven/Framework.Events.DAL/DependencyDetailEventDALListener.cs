using Framework.Core;
using Framework.DomainDriven;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Events;

/// <summary>
/// Базовый класс для DAL-евентов
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public class DependencyDetailEventDALListener<TPersistentDomainObjectBase> : IBeforeTransactionCompletedDALListener
    where TPersistentDomainObjectBase : class
{
    private readonly IEventOperationSender eventOperationSender;

    private readonly EventDALListenerSettings<TPersistentDomainObjectBase> settings;

    public DependencyDetailEventDALListener(
        [FromKeyedServices("DAL")] IEventOperationSender eventOperationSender,
        EventDALListenerSettings<TPersistentDomainObjectBase> settings)
    {
        this.eventOperationSender = eventOperationSender;
        this.settings = settings;
    }

    /// <inheritdoc />
    public void Process(DALChangesEventArgs eventArgs)
    {
        if (!this.settings.TypeEvents.Any())
        {
            return;
        }

        // фильтруем изменения по тем типам, ко которым есть подписка
        var joinItems = eventArgs.Changes.GroupDALObjectByType().Join(this.settings.TypeEvents, z => z.Key, z => z.Type, ValueTuple.Create);

        // сопоставляем измененных объект с его описанием его изменения (save, remove) и функцией, определяющей необходимость отсылки события
        var values = joinItems.SelectMany(
                                  z =>
                                      new[]
                                      {
                                          ValueTuple.Create(
                                              z.Item1.Value.CreatedItems.Concat(z.Item1.Value.UpdatedItems),
                                              DomainObjectEvent.Save,
                                              z.Item2.IsSaveProcessingFunc),
                                          ValueTuple.Create(
                                              (IEnumerable<IDALObject>)z.Item1.Value.RemovedItems,
                                              DomainObjectEvent.Remove,
                                              z.Item2.IsRemoveProcessingFunc)
                                      })
                              .ToList();

        // применяем функцию, определяющая необходимость отсылки события
        var allFilteredValues = values.SelectMany(z => z.Item1.Where(q => z.Item3(q.Object)).Select(q => ValueTuple.Create(q, z.Item2)))
                                      .ToList();

        // группируем по порядку получения(применения в базу) инфы о коммите данной сущности
        // могут быть ньюансы, если сущность была сначало создана в базе, потом обновлена, в этом наборе будут две записи, с разными индексами
        // но по факту с одним набором полей (ссылка одна и таже будет)
        // на текущий момент: все такие кейсы решать в конкретных системах конкретным образом
        // в общем случае пока сложно написать.
        var allFilteredOrderedValues = allFilteredValues.OrderBy(z => z.Item1.ApplyIndex);

        var proccessedValues = this.ProcessFinalAllFilteredOrderedValues(eventArgs, allFilteredOrderedValues);

        // отсылаем во внешние системы.
        foreach (var item in proccessedValues)
        {
            var domainObject = (TPersistentDomainObjectBase)item.Item1.Object;
            var domainObjectType = item.Item1.Type;
            var eventType = item.Item2;

            this.eventOperationSender.Send(domainObject, domainObjectType, eventType);
        }
    }
    protected virtual IEnumerable<ValueTuple<IDALObject, DomainObjectEvent>> ProcessFinalAllFilteredOrderedValues(
            DALChangesEventArgs eventArgs,
            IEnumerable<ValueTuple<IDALObject, DomainObjectEvent>> allFilteredOrderedValues)
    {
        var joinItems = eventArgs.Changes.GroupDALObjectByType().Join(this.settings.Dependencies, z => z.Key, z => z.SourceTypeEvent.Type, ValueTuple.Create).ToList();

        if (!joinItems.Any())
        {
            return allFilteredOrderedValues;
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
                this.settings.Dependencies.Select(z => z.TargetTypeEvent.Type)
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
        return allFilteredOrderedValues.Concat(allAbsentsTargetObjects.Select(z => ValueTuple.Create((IDALObject)(new DALObject(z.TargetObject, z.TargetObjectType, 1)), DomainObjectEvent.Save)));
    }
}
