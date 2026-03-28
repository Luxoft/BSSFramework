using Framework.Database.Mapping;
using Framework.Database.Metadata;

namespace Framework.Database.SqlMapper;

public abstract class Mapper<TFieldMetadata> : IMapper
        where TFieldMetadata : FieldMetadata
{
    IEnumerable<SqlFieldMappingInfo> IMapper.GetMapping(FieldMetadata field) => this.GetMapping((TFieldMetadata)field);

    protected abstract IEnumerable<SqlFieldMappingInfo> GetMapping(TFieldMetadata field);

    protected virtual bool GetIsNullable(FieldMetadata field)
    {
        var nullableAttributes = field.Attributes.OfType<NullableAttribute>();

        return nullableAttributes.Any() || field.Type.IsClass;
    }
}
