using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Events.DTOMapper;

public class RuntimeDomainEventDTOConverter<TPersistentDomainObjectBase, TMappingService, TEventDTOBase>
{
    private readonly object syncRoot = new();

    private readonly Dictionary<EventOperation, object> FuncDictionary = new();

    public TEventDTOBase Convert<TDomainObject>(TMappingService mappingService, TDomainObject domainObject, EventOperation eventOperation)
        where TDomainObject : TPersistentDomainObjectBase
    {
        var func = this.GetFunc<TDomainObject>(eventOperation);

        return func(mappingService, domainObject);
    }

    private Func<TMappingService, TDomainObject, TEventDTOBase> GetFunc<TDomainObject>(EventOperation eventOperation)
    {
        lock (this.syncRoot)
        {
            var func = this.FuncDictionary.GetValueOrCreate(eventOperation, () => this.CreateLambda<TDomainObject>(eventOperation));

            return (Func<TMappingService, TDomainObject, TEventDTOBase>)func;
        }
    }

    protected virtual string GetEventDtoTypeName<TDomainObject>(EventOperation eventOperation)
    {
        return $"{typeof(TEventDTOBase).Namespace}.{typeof(TDomainObject).Name}{eventOperation.Name}EventDTO";
    }

    protected virtual Type GetEventDtoType<TDomainObject>(EventOperation eventOperation)
    {
        return typeof(TEventDTOBase).Assembly.GetType(this.GetEventDtoTypeName<TDomainObject>(eventOperation));
    }

    private Func<TMappingService, TDomainObject, TEventDTOBase> CreateLambda<TDomainObject>(EventOperation eventOperation)
    {
        var eventDtoType = this.GetEventDtoType<TDomainObject>(eventOperation);
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
