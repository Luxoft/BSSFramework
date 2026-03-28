using Framework.Database.Mapping;
using Framework.Database.Metadata;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.SqlMapper;

public class ReferenceTypeFieldMapper : Mapper<ReferenceTypeFieldMetadata>
{
    protected override IEnumerable<SqlFieldMappingInfo> GetMapping(ReferenceTypeFieldMetadata field) => this.GetMapping(field, string.Empty);

    public IEnumerable<SqlFieldMappingInfo> GetMapping(ReferenceTypeFieldMetadata field, string namePrefix)
    {
        yield return new SqlFieldMappingInfo(
                                             DataType.UniqueIdentifier,
                                             namePrefix + field.GetSqlReferenceColumnName(),
                                             this.GetIsNullable(field),
                                             false,
                                             field,
                                             null,
                                             field.Attributes.OfType<MappingAttribute>().Any(z=>z.IsUnique));
    }
}
