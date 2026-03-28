using Framework.Database.Metadata;

namespace Framework.Database.SqlMapper;

/// <summary>
/// TODO: inject interface where use concrete mapping factory
/// </summary>
public static class MapperFactory
{
    private static readonly Dictionary<Type, IMapper> dictionary = new Dictionary<Type, IMapper>
                                                                   {
                                                                       { typeof(ReferenceTypeFieldMetadata), new ReferenceTypeFieldMapper() },
                                                                       { typeof(PrimitiveTypeFieldMetadata), new PrimitiveTypeFieldMapper() },
                                                                       { typeof(ListTypeFieldMetadata), new ListTypeFieldMapper() },
                                                                       { typeof(InlineTypeFieldMetadata), new InlineTypeFieldMapper() }
                                                                   };

    public static IMapper GetMapper(FieldMetadata field) => dictionary[field.GetType()];

    public static IEnumerable<SqlFieldMappingInfo> GetMapping(FieldMetadata fieldMetadata) => GetMapper(fieldMetadata).GetMapping(fieldMetadata);
}
