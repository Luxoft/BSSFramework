using System.Collections.Concurrent;
using System.Linq.Expressions;

using Anch.Core;

using Framework.Application.Events;

namespace Framework.BLL.DTOMapping.DTOMapper;

public class RuntimeDomainEventDTOConverter<TPersistentDomainObjectBase, TMappingService, TEventDTOBase>
{
    private readonly ConcurrentDictionary<(EventOperation, Type), object> funcCache = [];

    public TEventDTOBase Convert<TDomainObject>(TMappingService mappingService, TDomainObject domainObject, EventOperation domainObjectEvent)
        where TDomainObject : TPersistentDomainObjectBase
    {
        var func = this.GetFunc<TDomainObject>(domainObjectEvent);

        return func(mappingService, domainObject);
    }

    private Func<TMappingService, TDomainObject, TEventDTOBase> GetFunc<TDomainObject>(EventOperation domainObjectEvent) =>
        this.funcCache.GetOrAddAs(
            (domainObjectEvent, typeof(TDomainObject)),
            _ => this.CreateLambda<TDomainObject>(domainObjectEvent).Compile());

    protected virtual string GetEventDtoTypeName<TDomainObject>(EventOperation domainObjectEvent) => $"{typeof(TEventDTOBase).Namespace}.{typeof(TDomainObject).Name}{domainObjectEvent.Name}EventDTO";

    protected virtual Type GetEventDtoType<TDomainObject>(EventOperation domainObjectEvent) => typeof(TEventDTOBase).Assembly.GetType(this.GetEventDtoTypeName<TDomainObject>(domainObjectEvent))!;

    private Expression<Func<TMappingService, TDomainObject, TEventDTOBase>> CreateLambda<TDomainObject>(EventOperation domainObjectEvent)
    {
        var eventDtoType = this.GetEventDtoType<TDomainObject>(domainObjectEvent);
        var eventDtoTypeConstructor = eventDtoType.GetConstructor([typeof(TMappingService), typeof(TDomainObject)]);

        var mappingServiceParam = Expression.Parameter(typeof(TMappingService), "mappingService");
        var domainObjectParam = Expression.Parameter(typeof(TDomainObject), "domainObject");
        var accessor = Expression.Lambda<Func<TMappingService, TDomainObject, TEventDTOBase>>(
            Expression.New(eventDtoTypeConstructor, mappingServiceParam, domainObjectParam),
            mappingServiceParam,
            domainObjectParam);

        return accessor;
    }
}
