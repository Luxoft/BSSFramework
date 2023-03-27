using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Events;

public static class DomainToEventMapper<TEventBase, TMappingService>
{
    private static object syncTypeRoot = new object();
    private static object syncFuncRoot = new object();

    private static readonly Dictionary<Type, Dictionary<EventOperation, Func<TMappingService, object, TEventBase>>> typeToFuncDictionary =
            new Dictionary<Type, Dictionary<EventOperation, Func<TMappingService, object, TEventBase>>>();


    public static TEventBase MapToEventDTO(TMappingService mappingService, object domain, EventOperation eventOperation)
    {
        return GetFunc(eventOperation, domain.GetType())(mappingService, domain);
    }

    private static Func<TMappingService, object, TEventBase> GetFunc(EventOperation operation, Type domainType)
    {
        var funcDictionary = typeToFuncDictionary.GetValueOrCreate(
                                                                   domainType,
                                                                   syncTypeRoot,
                                                                   () => new Dictionary<EventOperation, Func<TMappingService, object, TEventBase>>());


        return funcDictionary.GetValueOrCreate(operation, syncFuncRoot, () => CreateLambda(operation, domainType));
    }

    private static Func<TMappingService, object, TEventBase> CreateLambda(EventOperation operation, Type domainType)
    {
        var eventDtoTypeName = $"{typeof(TEventBase).Namespace}.{domainType.Name}{operation}EventDTO";

        var eventDtoType = typeof(TEventBase).Assembly.GetType(eventDtoTypeName);
        var eventDtoTypeConstructor = eventDtoType.GetConstructor(new[] { typeof(TMappingService), domainType });

        var mappingServiceParam = Expression.Parameter(typeof(TMappingService), "mappingService");
        var domainObjectParam = Expression.Parameter(typeof(object), "domainObject");

        var castExpression = Expression.Convert(domainObjectParam, domainType);

        var accessor = Expression.Lambda<Func<TMappingService, object, TEventBase>>
                (Expression.New(eventDtoTypeConstructor, mappingServiceParam, castExpression), mappingServiceParam, domainObjectParam);

        return accessor.Compile();
    }
}
