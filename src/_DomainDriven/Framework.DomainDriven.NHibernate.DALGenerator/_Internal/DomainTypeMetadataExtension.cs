using Framework.DomainDriven.DAL.Sql;
using Framework.DomainDriven.Metadata;
using Framework.Core;

namespace Framework.DomainDriven.NHibernate.DALGenerator;

internal static class DomainTypeMetadataExtension
{
    public static IEnumerable<ReferenceTypeFieldMetadata> GetManyToOneReference(this DomainTypeMetadata domainTypeMetadata)
    {
        return domainTypeMetadata.ReferenceFields.Except(domainTypeMetadata.GetOneToOneReference());
    }
    public static IEnumerable<ReferenceTypeFieldMetadata> GetOneToOneReference( this DomainTypeMetadata domainTypeMetadata)
    {
        var referencies = domainTypeMetadata.ReferenceFields;
        var toTypes = referencies.Select(z => z.ToType).Distinct();
        var toDomainTypeMetadataReferencies = domainTypeMetadata.AssemblyMetadata.DomainTypes
                                                                .Join(toTypes, z => z.DomainType, z => z, (d, t) => d.ReferenceFields).SelectMany(z => z).Distinct();

        return referencies.Join(toDomainTypeMetadataReferencies, z => z.FromType, z => z.ToType, (f, s) => f);
    }

    public static FieldMetadata GetIdentityField(this DomainTypeMetadata source)
    {
        var request = from typeMetadata in source.GetAllElements (m => m.Parent)

                      from fieldMetaData in typeMetadata.Fields

                      where string.Equals (fieldMetaData.Name, "Id", StringComparison.InvariantCultureIgnoreCase)

                      select fieldMetaData;

        return request.Single(() => new ArgumentException (
                                                                  $"Domain type {source.DomainType.Name} has no identity field"));



        //var typeToDomainTypeMetadata = source.AssemblyMetadata.DomainTypes.ToDictionary (z => z.DomainType, z=>z);
        //var allDomainTypes = typeToDomainTypeMetadata.Join (allTypes, z => z.Key, z => z,
        //                               (type, domainTypeMetadata) => domainTypeMetadata).ToList ();

        //allDomainTypes.SelectMany (z=>z)
        //var identityField =
        //        source.Fields.Single(
        //            z => string.Equals (z.Name, "Id", StringComparison.InvariantCultureIgnoreCase),
        //        () => new ArgumentException (string.Format ("Domain type {0} has no identity field",
        //                                                   source.DomainType.Name)));

        //return identityField;
    }
    public static string GetIdentityFieldName(this DomainTypeMetadata source, Func<DomainTypeMetadata, string> getIdentityFieldInBDFunc)
    {
        return source.GetIdentityField ().ToColumnName (z => getIdentityFieldInBDFunc (source));
    }
}
