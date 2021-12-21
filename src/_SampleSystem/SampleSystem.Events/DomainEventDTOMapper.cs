using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;
using Framework.DomainDriven.BLL;

using SampleSystem.Domain;
using SampleSystem.Generated.DTO;

namespace SampleSystem.Events
{
    public static class DomainEventDTOMapper<TDomainObject, TOperation>
        where TDomainObject : PersistentDomainObjectBase
        where TOperation : struct, Enum
    {
        private static readonly object SyncRoot = new object();

        private static readonly Dictionary<TOperation, Func<ISampleSystemDTOMappingService, TDomainObject, EventDTOBase>> FuncDictionary =
            new Dictionary<TOperation, Func<ISampleSystemDTOMappingService, TDomainObject, EventDTOBase>>();


        public static EventDTOBase MapToEventDTO(ISampleSystemDTOMappingService mappingService, TDomainObject domainObject, TOperation operation)
        {
            return GetFunc(operation)(mappingService, domainObject);
        }

        private static Func<ISampleSystemDTOMappingService, TDomainObject, EventDTOBase> GetFunc(TOperation operation)
        {
            lock (SyncRoot)
            {
                return FuncDictionary.GetValueOrCreate(operation, () => CreateLambda(operation));
            }
        }

        private static Func<ISampleSystemDTOMappingService, TDomainObject, EventDTOBase> CreateLambda(TOperation operation)
        {
            var eventDtoTypeName = "SampleSystem.Generated.DTO." + typeof(TDomainObject).Name + operation + "EventDTO";
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
}
