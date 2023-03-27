using System.Linq.Expressions;

using Framework.Authorization.Domain;
using Framework.Authorization.Generated.DTO;
using Framework.Core;

namespace Framework.Authorization.Events;

public static class AuthorizationDomainEventDTOMapper<TDomainObject, TOperation>
        where TDomainObject : PersistentDomainObjectBase
        where TOperation : struct, Enum
{
    private static object syncRoot = new object();

    private static readonly Dictionary<TOperation, Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase>> FuncDictionary =
            new Dictionary<TOperation, Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase>>();


    public static EventDTOBase MapToEventDTO(IAuthorizationDTOMappingService mappingService, TDomainObject domainObject, TOperation operation)
    {
        return GetFunc(operation)(mappingService, domainObject);
    }

    private static Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase> GetFunc(TOperation operation)
    {
        lock (syncRoot)
        {
            return FuncDictionary.GetValueOrCreate(operation, () => CreateLambda(operation));
        }
    }

    private static Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase> CreateLambda(TOperation operation)
    {
        var eventDtoTypeName = "Framework.Authorization.Generated.DTO." + typeof(TDomainObject).Name +
                               operation + "EventDTO";

        var eventDtoType = typeof(EventDTOBase).Assembly.GetType(eventDtoTypeName);
        var eventDtoTypeConstructor = eventDtoType.GetConstructor(new[] { typeof(IAuthorizationDTOMappingService), typeof(TDomainObject) });

        var mappingServiceParam = Expression.Parameter(typeof(IAuthorizationDTOMappingService), "mappingService");
        var domainObjectParam = Expression.Parameter(typeof(TDomainObject), "domainObject");
        var accessor = Expression.Lambda<Func<IAuthorizationDTOMappingService, TDomainObject, EventDTOBase>>
                (Expression.New(eventDtoTypeConstructor, mappingServiceParam, domainObjectParam), mappingServiceParam, domainObjectParam);

        return accessor.Compile();
    }
}
