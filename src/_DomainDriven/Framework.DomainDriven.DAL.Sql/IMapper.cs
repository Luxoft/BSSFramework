using System.Collections.Generic;
using Framework.DomainDriven.Metadata;

namespace Framework.DomainDriven.DAL.Sql
{
    public interface IMapper
    {
        IEnumerable<SqlFieldMappingInfo> GetMapping (FieldMetadata field);
    }
}
