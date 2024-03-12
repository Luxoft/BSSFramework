using Framework.Core;
using Framework.DomainDriven;

namespace Framework.Events;

/// <summary>
/// Базовый класс для DAL-евентов
/// </summary>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public class DependencyDetailEventDALListener<TPersistentDomainObjectBase> : IBeforeTransactionCompletedDALListener, IEventOperationReceiver
    where TPersistentDomainObjectBase : class
{
    private readonly IEventDTOMessageSender<TPersistentDomainObjectBase> messageSender;

    private readonly EventDALListenerSettings<TPersistentDomainObjectBase> settings;

    public DependencyDetailEventDALListener(
        IEventDTOMessageSender<TPersistentDomainObjectBase> messageSender,
        EventDALListenerSettings<TPersistentDomainObjectBase> settings = null)
    {
        this.messageSender = messageSender;
        this.settings = settings ?? new EventDALListenerSettings<TPersistentDomainObjectBase>();
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
                                              EventOperation.Save,
                                              z.Item2.IsSaveProcessingFunc),
                                          ValueTuple.Create(
                                              (IEnumerable<IDALObject>)z.Item1.Value.RemovedItems,
                                              EventOperation.Remove,
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

            var message = new DomainOperationSerializeData<TPersistentDomainObjectBase>
                          {
                              DomainObject = domainObject,
                              Operation = eventType,
                              CustomDomainObjectType = domainObjectType
                          };

            this.messageSender.Send(message);
        }
    }
    protected virtual IEnumerable<ValueTuple<IDALObject, EventOperation>> ProcessFinalAllFilteredOrderedValues(
            DALChangesEventArgs eventArgs,
            IEnumerable<ValueTuple<IDALObject, EventOperation>> allFilteredOrderedValues)
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
        return allFilteredOrderedValues.Concat(allAbsentsTargetObjects.Select(z => ValueTuple.Create((IDALObject)(new DALObject(z.TargetObject, z.TargetObjectType, 1)), EventOperation.Save)));
    }

    void IEventOperationReceiver.Receive<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
    {
        this.Process(new DALChangesEventArgs(GetDALChanges(domainObject, domainObjectEvent)));
    }

    private static DALChanges GetDALChanges<TDomainObject>(TDomainObject domainObject, EventOperation domainObjectEvent)
    {
        switch (domainObjectEvent.Name)
        {
            case nameof(EventOperation.Save):

                return new DALChanges(
                    new IDALObject[0],
                    new IDALObject[] { new DALObject(domainObject, typeof(TDomainObject), 0) },
                    new IDALObject[0]);

            case nameof(EventOperation.Remove):

                return new DALChanges(
                    new IDALObject[0],
                    new IDALObject[0],
                    new IDALObject[] { new DALObject(domainObject, typeof(TDomainObject), 0) });

            default:
                throw new ArgumentOutOfRangeException(nameof(domainObjectEvent));
        }
    }
}
