using System.Collections.Generic;
using System.Linq;
using Framework.Persistent.Mapping;
using Framework.DomainDriven.Metadata;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DAL.Sql;

public class ReferenceTypeFieldMapper : Mapper<ReferenceTypeFieldMetadata>
{
    protected override IEnumerable<SqlFieldMappingInfo> GetMapping(ReferenceTypeFieldMetadata field)
    {
        return this.GetMapping(field, string.Empty);
    }

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
