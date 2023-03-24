using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;

namespace Framework.Events;

/// <summary>
/// Базовый класс для DAL-евентов
/// </summary>
/// <typeparam name="TBLLContext"></typeparam>
/// <typeparam name="TPersistentDomainObjectBase"></typeparam>
public abstract class EventDALListener<TBLLContext, TPersistentDomainObjectBase> : BLLContextContainer<TBLLContext>, IManualEventDALListener<TPersistentDomainObjectBase>, IBeforeTransactionCompletedDALListener
        where TBLLContext : class
        where TPersistentDomainObjectBase : class
{
    private readonly TypeEvent[] typeEvents;

    private readonly IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>> messageSender;

    protected EventDALListener(TBLLContext context, IMessageSender<IDomainOperationSerializeData<TPersistentDomainObjectBase>> messageSender, IEnumerable<TypeEvent> typeEvents)
            : base(context)
    {
        this.messageSender = messageSender;

        this.typeEvents = (typeEvents ?? throw new ArgumentNullException(nameof(typeEvents))).ToArray();
    }

    Type IPersistentDomainObjectBaseTypeContainer.PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

    /// <inheritdoc />
    public void Process(DALChangesEventArgs eventArgs)
    {
        if (!this.typeEvents.Any())
        {
            return;
        }

        // фильтруем изменения по тем типам, ко которым есть подписка
        var joinItems = eventArgs.Changes.GroupDALObjectByType().Join(this.typeEvents, z => z.Key, z => z.Type, TupleStruct.Create);

        // сопоставляем измененных объект с его описанием его изменения (save, remove) и функцией, определяющей необходимость отсылки события
        var values = joinItems.SelectMany(z =>
                                                  new[]
                                                  {
                                                          TupleStruct.Create(z.Item1.Value.CreatedItems.Concat(z.Item1.Value.UpdatedItems), EventOperation.Save, z.Item2.IsSaveProcessingFunc),
                                                          TupleStruct.Create((IEnumerable<IDALObject>)z.Item1.Value.RemovedItems, EventOperation.Remove, z.Item2.IsRemoveProcessingFunc)
                                                  })
                              .ToList();

        // применяем функцию, определяющая необходимость отсылки события
        var allFilteredValues = values.SelectMany(z => z.Item1.Where(q => z.Item3(q.Object)).Select(q => TupleStruct.Create(q, z.Item2))).ToList();

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

            var message = new DomainOperationSerializeData<TPersistentDomainObjectBase, EventOperation>
                          {
                                  DomainObject = domainObject,
                                  Operation = eventType,
                                  CustomDomainObjectType = domainObjectType
                          };

            this.messageSender.Send(message);
        }
    }

    protected virtual IEnumerable<TupleStruct<IDALObject, EventOperation>> ProcessFinalAllFilteredOrderedValues(
            DALChangesEventArgs eventArgs,
            IEnumerable<TupleStruct<IDALObject, EventOperation>> allFilteredOrderedValues)
    {
        return allFilteredOrderedValues;
    }

    IOperationEventSender<TDomainObject, EventOperation> IManualEventDALListener<TPersistentDomainObjectBase>.GetForceEventContainer<TDomainObject>()
    {
        return new ForceEventContainer<TDomainObject>(this);
    }

    private class ForceEventContainer<TDomainObject> : IOperationEventSender<TDomainObject, EventOperation>
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        private readonly EventDALListener<TBLLContext, TPersistentDomainObjectBase> dalListener;

        public ForceEventContainer(EventDALListener<TBLLContext, TPersistentDomainObjectBase> dalListener)
        {
            this.dalListener = dalListener ?? throw new ArgumentNullException(nameof(dalListener));
        }

        public void SendEvent(IDomainOperationEventArgs<TDomainObject, EventOperation> eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            this.dalListener.Process(new DALChangesEventArgs(GetDALChanges(eventArgs.DomainObject, eventArgs.Operation)));
        }

        private static DALChanges GetDALChanges(TDomainObject domainObject, EventOperation operation)
        {
            switch (operation)
            {
                case EventOperation.Save:

                    return new DALChanges(
                                          new IDALObject[0],
                                          new IDALObject[] { new DALObject(domainObject, typeof(TDomainObject), 0) },
                                          new IDALObject[0]);

                case EventOperation.Remove:

                    return new DALChanges(
                                          new IDALObject[0],
                                          new IDALObject[0],
                                          new IDALObject[] { new DALObject(domainObject, typeof(TDomainObject), 0) });

                default:
                    throw new ArgumentOutOfRangeException(nameof(operation));
            }
        }
    }
}
