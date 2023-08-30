using Framework.Persistent;

namespace Framework.DomainDriven.SerializeMetadata;

internal class DomainTypePropertyExpandLoader
{
    protected readonly SystemMetadata SystemMetadata;


    public DomainTypePropertyExpandLoader(SystemMetadata systemMetadata)
    {
        if (systemMetadata == null) throw new ArgumentNullException(nameof(systemMetadata));

        this.SystemMetadata = systemMetadata;
    }

    protected virtual IPropertyMetadata GetDomainTypeProperty(IDomainTypeMetadata domainType, string propertyName)
    {
        return domainType.Properties.GetByName(propertyName);
    }

    protected virtual IEnumerable<PropertySubsetMetadata> GetDomainTypeProperties(IDomainTypeMetadata domainType, Tuple<string, string>[][] paths)
    {
        return from path in paths

               let head = path[0]

               let tail = path.Skip(1).ToArray()

               group tail by head into propGroup

               let property = this.GetDomainTypeProperty(domainType, propGroup.Key.Item1)

               let baseType = this.SystemMetadata.Types.GetById(property.TypeHeader)

               let subsetType = baseType.Role.HasSubset()
                                        ? this.GetDomainTypeMetadataSubset((IDomainTypeMetadata)baseType, propGroup.Where(tails => tails.Any()).ToArray())
                                        : baseType


               select new PropertySubsetMetadata(property.Name, subsetType, property.IsCollection, property.AllowNull, property.IsVirtual, property.IsSecurity, property.IsVisualIdentity, propGroup.Key.Item2);
    }


    public DomainTypeSubsetMetadata GetDomainTypeMetadataSubset(IDomainTypeMetadata domainType, Tuple<string, string>[][] paths)
    {
        return new DomainTypeSubsetMetadata(
                                            domainType.Type,
                                            domainType.Role,
                                            domainType.IsHierarchical,
                                            this.GetDomainTypeProperties(domainType, paths));
    }
}
