using Framework.Persistent.Mapping;
using Framework.DomainDriven.Metadata;

namespace Framework.DomainDriven.DAL.Sql;

public abstract class Mapper<TFieldMetadata> : IMapper
        where TFieldMetadata : FieldMetadata
{
    IEnumerable<SqlFieldMappingInfo> IMapper.GetMapping(FieldMetadata field)
    {
        return this.GetMapping((TFieldMetadata)field);
    }
    protected abstract IEnumerable<SqlFieldMappingInfo> GetMapping(TFieldMetadata field);
    protected virtual bool GetIsNullable(FieldMetadata field)
    {
        var nullableAttributes = field.Attributes.OfType<NullableAttribute>();
        return nullableAttributes.Any() || field.Type.IsClass;
    }

}
