using Framework.Database.Metadata;

namespace Framework.Database.SqlMapper;

public interface IMapper
{
    IEnumerable<SqlFieldMappingInfo> GetMapping (FieldMetadata field);
}
