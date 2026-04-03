using Framework.Database.Metadata;

namespace Framework.Database.SqlMapper;

public class ListTypeFieldMapper : Mapper<ListTypeFieldMetadata>
{
    protected override IEnumerable<SqlFieldMappingInfo> GetMapping(ListTypeFieldMetadata field)
    {
        yield break;
    }
}
