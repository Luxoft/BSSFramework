using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;
using Framework.Events;

namespace Framework.Authorization.Events;

public static class AuthorizationDomainEventDTOMapper<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    private static object syncRoot = new object();

    private static readonly Dictionary<EventOperation, Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase>> FuncDictionary = new ();


    public static EventDTOBase MapToEventDTO(IAuthorizationDTOMappingService mappingService, TDomainObject domainObject, EventOperation operation)
    {
        return GetFunc(operation)(mappingService, domainObject);
    }

    private static Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase> GetFunc(EventOperation operation)
    {
        lock (syncRoot)
        {
            return FuncDictionary.GetValueOrCreate(operation, () => CreateLambda(operation));
        }
    }

    private static Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase> CreateLambda(EventOperation operation)
    {
        var eventDtoTypeName = "Framework.Authorization.Generated.DTO." + typeof(TDomainObject).Name +
                               operation.Name + "EventDTO";

        var eventDtoType = typeof(EventDTOBase).Assembly.GetType(eventDtoTypeName);
        var eventDtoTypeConstructor = eventDtoType.GetConstructor(new[] { typeof(IAuthorizationDTOMappingService), typeof(TDomainObject) });

        var mappingServiceParam = Expression.Parameter(typeof(IAuthorizationDTOMappingService), "mappingService");
        var domainObjectParam = Expression.Parameter(typeof(TDomainObject), "domainObject");
        var accessor = Expression.Lambda<Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase>>
                (Expression.New(eventDtoTypeConstructor, mappingServiceParam, domainObjectParam), mappingServiceParam, domainObjectParam);

        return accessor.Compile();
    }
}
