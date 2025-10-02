using System.Linq.Expressions;

using CommonFramework;

namespace Framework.Events.Legacy;

public class RuntimeDomainEventDTOConverter<TPersistentDomainObjectBase, TMappingService, TEventDTOBase>
{
    private readonly object syncRoot = new();

    private readonly Dictionary<(EventOperation, Type), object> FuncDictionary = new();

    public TEventDTOBase Convert<TDomainObject>(TMappingService mappingService, TDomainObject domainObject, EventOperation domainObjectEvent)
        where TDomainObject : TPersistentDomainObjectBase
    {
        var func = this.GetFunc<TDomainObject>(domainObjectEvent);

        return func(mappingService, domainObject);
    }

    private Func<TMappingService, TDomainObject, TEventDTOBase> GetFunc<TDomainObject>(EventOperation domainObjectEvent)
    {
        lock (this.syncRoot)
        {
            var func = this.FuncDictionary.GetValueOrCreate((domainObjectEvent, typeof(TDomainObject)), () => this.CreateLambda<TDomainObject>(domainObjectEvent));

            return (Func<TMappingService, TDomainObject, TEventDTOBase>)func;
        }
    }

    protected virtual string GetEventDtoTypeName<TDomainObject>(EventOperation domainObjectEvent)
    {
        return $"{typeof(TEventDTOBase).Namespace}.{typeof(TDomainObject).Name}{domainObjectEvent.Name}EventDTO";
    }

    protected virtual Type GetEventDtoType<TDomainObject>(EventOperation domainObjectEvent)
    {
        return typeof(TEventDTOBase).Assembly.GetType(this.GetEventDtoTypeName<TDomainObject>(domainObjectEvent));
    }

    private Func<TMappingService, TDomainObject, TEventDTOBase> CreateLambda<TDomainObject>(EventOperation domainObjectEvent)
    {
        var eventDtoType = this.GetEventDtoType<TDomainObject>(domainObjectEvent);
        var eventDtoTypeConstructor = eventDtoType.GetConstructor([typeof(TMappingService), typeof(TDomainObject)]);

        var mappingServiceParam = Expression.Parameter(typeof(TMappingService), "mappingService");
        var domainObjectParam = Expression.Parameter(typeof(TDomainObject), "domainObject");
        var accessor = Expression.Lambda<Func<TMappingService, TDomainObject, TEventDTOBase>>(
            Expression.New(eventDtoTypeConstructor, mappingServiceParam, domainObjectParam),
            mappingServiceParam,
            domainObjectParam);

        return accessor.Compile();
    }
}
