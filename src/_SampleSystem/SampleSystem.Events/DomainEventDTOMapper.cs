using System.Linq.Expressions;

using Framework.Core;
using Framework.Events;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events;

public static class DomainEventDTOMapper<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase

{
    private static readonly object SyncRoot = new object();

    private static readonly Dictionary<EventOperation, Func<ISampleSystemDTOMappingService, TDomainObject, EventDTOBase>> FuncDictionary = new ();


    public static EventDTOBase MapToEventDTO(ISampleSystemDTOMappingService mappingService, TDomainObject domainObject, EventOperation operation)
    {
        return GetFunc(operation)(mappingService, domainObject);
    }

    private static Func<ISampleSystemDTOMappingService, TDomainObject, EventDTOBase> GetFunc(EventOperation operation)
    {
        lock (SyncRoot)
        {
            return FuncDictionary.GetValueOrCreate(operation, () => CreateLambda(operation));
        }
    }

    private static Func<ISampleSystemDTOMappingService, TDomainObject, EventDTOBase> CreateLambda(EventOperation operation)
    {
        var eventDtoTypeName = "SampleSystem.Generated.DTO." + typeof(TDomainObject).Name + operation.Name + "EventDTO";
        var eventDtoType = typeof(EventDTOBase).Assembly.GetType(eventDtoTypeName);
        if (null == eventDtoType)
        {
            throw new Exception($"Type '{eventDtoTypeName}' for Raise Integration Event Not Found");
        }

        var eventDtoTypeConstructor = eventDtoType.GetConstructor(new[] { typeof(ISampleSystemDTOMappingService), typeof(TDomainObject) });


        var mappingServiceParam = Expression.Parameter(typeof(ISampleSystemDTOMappingService), "mappingService");

        var domainObjectParam = Expression.Parameter(typeof(TDomainObject), "domainObject");

        var accessor = Expression.Lambda<Func<ISampleSystemDTOMappingService, TDomainObject, EventDTOBase>>
                (Expression.New(eventDtoTypeConstructor, mappingServiceParam, domainObjectParam), mappingServiceParam, domainObjectParam);

        return accessor.Compile();
    }
}
