using System;
using System.Collections.Generic;
using Framework.DomainDriven.Metadata;

namespace Framework.DomainDriven.DAL.Sql
{
    /// <summary>
    /// TODO: inject interface where use concrete mapping factory
    /// </summary>
    public static class MapperFactory
    {
        private static readonly Dictionary<Type, IMapper> dictionary = new Dictionary<Type, IMapper>
        {
             { typeof(ReferenceTypeFieldMetadata), new ReferenceTypeFieldMapper()},
             { typeof(PrimitiveTypeFieldMetadata), new PrimitiveTypeFieldMapper()},
             { typeof(ListTypeFieldMetadata), new ListTypeFieldMapper()},
             { typeof(InlineTypeFieldMetadata), new InlineTypeFieldMapper() }
        };

        public static IMapper GetMapper(FieldMetadata field)
        {
            return dictionary[field.GetType()];
        }
        public static IEnumerable<SqlFieldMappingInfo> GetMapping(FieldMetadata fieldMetadata)
        {
            return GetMapper (fieldMetadata).GetMapping (fieldMetadata);
        }
    }
}
