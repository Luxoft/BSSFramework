using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.Metadata;
using Framework.Persistent.Mapping;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.DomainDriven.DBGenerator
{
    internal static class DbGeneratorExtensions
    {
        public static void ClearIndexies(this Table source)
        {
            var indexNames = source.Indexes.Cast<Index>().Select(z => z).OrderBy(z => IndexKeyType.DriPrimaryKey == z.IndexKeyType).ToList();
            indexNames.Foreach(z => z.Drop());
        }

        public static IEnumerable<FieldMetadata> GetPersistentFields(this DomainTypeMetadata source)
        {
            return source.Fields.Where(z => !(z.Attributes.OfType<MappingAttribute>().Any(q => q.IsOneToOne)));
        }

        public static IEnumerable<ReferenceTypeFieldMetadata> GetPersistentReferenceFields(this DomainTypeMetadata source)
        {
            return source.ReferenceFields.Where(z => !(z.Attributes.OfType<MappingAttribute>().Any(q => q.IsOneToOne)));
        }
    }
}