using CommonFramework;

using Framework.Database.Mapping;
using Framework.Database.Metadata;

using Microsoft.SqlServer.Management.Smo;

namespace Framework.Database.NHibernate.DBGenerator;

internal static class DbGeneratorExtensions
{
    public static void ClearIndexies(this Table source)
    {
        var indexNames = source.Indexes.Select(z => z).OrderBy(z => IndexKeyType.DriPrimaryKey == z.IndexKeyType).ToList();
        indexNames.Foreach(z => z.Drop());
    }

    public static IEnumerable<FieldMetadata> GetPersistentFields(this DomainTypeMetadata source) => source.Fields.Where(z => !(z.Attributes.OfType<MappingAttribute>().Any(q => q.IsOneToOne)));

    public static IEnumerable<ReferenceTypeFieldMetadata> GetPersistentReferenceFields(this DomainTypeMetadata source) => source.ReferenceFields.Where(z => !(z.Attributes.OfType<MappingAttribute>().Any(q => q.IsOneToOne)));
}
