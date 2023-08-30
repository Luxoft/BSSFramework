using Framework.DomainDriven.Metadata;

namespace Framework.DomainDriven.DAL.Sql;

public class ListTypeFieldMapper : Mapper<ListTypeFieldMetadata>
{
    protected override IEnumerable<SqlFieldMappingInfo> GetMapping(ListTypeFieldMetadata field)
    {
        yield break;
    }
}
