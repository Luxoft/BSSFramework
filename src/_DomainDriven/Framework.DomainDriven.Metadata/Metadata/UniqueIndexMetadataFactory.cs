using System.Reflection;

using CommonFramework;

using Framework.Core;
using Framework.Persistent;
using Framework.Restriction;

namespace Framework.DomainDriven.Metadata;

public class UniqueIndexMetadataReader
{
    private readonly AssemblyMetadata _metadata;
    private readonly Dictionary<Type, DomainTypeMetadata> _typeToMetadataDictionary;

    public UniqueIndexMetadataReader(AssemblyMetadata metadata)
    {
        this._metadata = metadata;
        this._typeToMetadataDictionary = this._metadata.DomainTypes.SelectMany(z => new[] { z }.Concat(z.NotAbstractChildrenDomainTypes)).ToDictionary(z => z.DomainType);
    }

    public IEnumerable<UniqueIndexMetadata> Read(DomainTypeMetadata domainTypeMetadata)
    {
        return this.GetFromTypeAttribute(domainTypeMetadata).Concat(this.ProcessPropertyAttribute(domainTypeMetadata));
    }

    public IEnumerable<UniqueIndexMetadata> ReadFromReferenceTo(DomainTypeMetadata domainTypeMetadata)
    {
        return this.ProcessPropertyAttribute(domainTypeMetadata);
    }

    private IEnumerable<UniqueIndexMetadata> GetFromTypeAttribute(DomainTypeMetadata domainTypeMetadata)
    {
        var info = new[] { domainTypeMetadata }.Concat(domainTypeMetadata.NotAbstractChildrenDomainTypes)
                                               .Select(
                                                       z =>
                                                               new
                                                               {
                                                                       Source = z,
                                                                       UniqueGroupLowKeys = z.DomainType.GetCustomAttributes<UniqueGroupAttribute>().Select(w => w.Key.MaybeString(e => e.ToLower())).ToList(),
                                                                       UniqueElementFields = z.GetExpandedUpFields().Where(w => w.GetExpandedAttributes(z.DomainType).OfType<UniqueElementAttribute>().Any()).ToList(),
                                                                       UniqueElementsLowKeys = z.GetExpandedUpFields().SelectMany(w => w.GetExpandedAttributes(z.DomainType).OfType<UniqueElementAttribute>().Select(e => e.Key.MaybeString(r => r.ToLower()))).Distinct().ToList()
                                                               })
                                               .Where(z => z.UniqueGroupLowKeys.Any() && z.UniqueElementFields.Any() && z.UniqueGroupLowKeys.Intersect(z.UniqueElementsLowKeys).Any())

                                               .ToList();

        var keyToUniqueProperties = info.SelectMany(z => z.UniqueGroupLowKeys
                                                          .Select(w => new
                                                                       {
                                                                               Source = z.Source,
                                                                               LowGroupedKey = w,
                                                                               UniqueElementFields = z
                                                                                       .UniqueElementFields
                                                                                       .Where(u => u.GetExpandedAttributes(z.Source.DomainType).OfType<UniqueElementAttribute>()
                                                                                               .Any(q => string.Equals(q.Key, w, StringComparison.InvariantCultureIgnoreCase))),
                                                                       }))
                                        .GroupBy(z => z.LowGroupedKey)
                                        .ToList();


        return keyToUniqueProperties.Select(z => new UniqueIndexMetadata(domainTypeMetadata, z.Key, z.SelectMany(q => q.UniqueElementFields).Distinct()));
    }

    private IEnumerable<UniqueIndexMetadata> ProcessPropertyAttribute(DomainTypeMetadata domainTypeMetadata)
    {
        var allDomainTypes = this._metadata.DomainTypes.SelectMany(z => new[] { z }.Concat(z.NotAbstractChildrenDomainTypes));

        var masterTypeToListField = allDomainTypes
                                    .SelectMany(z => z.ListFields
                                                      .Where(listField => listField.ElementType == domainTypeMetadata.DomainType)
                                                      .SelectMany(listField => listField.Attributes.OfType<UniqueGroupAttribute>().Select(attr => new { Master = z, ListField = listField, UniqueAttribute = attr })))
                                    .ToList();


        foreach (var info in masterTypeToListField)
        {
            var detailType = info.ListField.ElementType;

            var groupName = info.UniqueAttribute.Key;

            var detailTypeMetadata = this._typeToMetadataDictionary[detailType];

            var refFields = detailTypeMetadata
                            .GetExpandedUpFields()
                            .Where(z => !(z is ListTypeFieldMetadata))
                            .Where(z => z.Attributes.OfType<UniqueElementAttribute>().Any())
                            .ToList();

            var masterRef = this.GetMasterRefMetadata(detailTypeMetadata, info.Master);

            var fields = refFields.Where(z => z.Attributes.OfType<UniqueElementAttribute>().Any(q => q.Key == groupName)).ToList();

            var uniqueFields = fields.Union(new[] { masterRef }).ToList();

            yield return new UniqueIndexMetadata(domainTypeMetadata, groupName, uniqueFields);
        }
    }

    private ReferenceTypeFieldMetadata GetMasterRefMetadata(
            DomainTypeMetadata detailMetadata,
            DomainTypeMetadata masterMetadata)
    {
        var masterRefCandidates = detailMetadata.GetExpandedUpFields().OfType<ReferenceTypeFieldMetadata>().Where(z => z.ToType == masterMetadata.DomainType).ToList();

        var masterRef = masterRefCandidates.FirstOrDefault();

        if (null == masterRef)
        {
            throw new ArgumentException($"Type:'{detailMetadata.DomainType.Name}' has no reference to master:'{masterMetadata.DomainType.Name}'");
        }

        if (masterRefCandidates.Count > 1)
        {
            masterRef = masterRefCandidates.Single(z => z.Attributes.Any(q => q is IsMasterAttribute),
                                                   () =>
                                                           new Exception(
                                                                         $"Type:{detailMetadata.DomainType.Name} has more then one property with master type:{masterMetadata.DomainType.Name}. Mark one property:{typeof(IsMasterAttribute).Name} attribute"),
                                                   () =>
                                                           new Exception(
                                                                         $"Type:{detailMetadata.DomainType.Name} has more then one property with IsMaster attribute master type:{masterMetadata.DomainType.Name}"));
        }

        return masterRef;
    }

}
