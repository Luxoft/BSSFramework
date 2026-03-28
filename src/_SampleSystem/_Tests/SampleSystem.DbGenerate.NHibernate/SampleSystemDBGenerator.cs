using Framework.Database.Mapping;
using Framework.Database.Metadata;
using Framework.Database.NHibernate._MappingSettings;
using Framework.DomainDriven.DBGenerator;

namespace SampleSystem.DbGenerate;

public class SampleSystemDBGenerator : DBGenerator
{
    public SampleSystemDBGenerator(MappingSettings settings) : base(settings)
    {
    }

    protected override void FilterMetadata(AssemblyMetadata metadata)
    {
        base.FilterMetadata(metadata);

        var nextDomainTypes = metadata.DomainTypes.Where(this.Used).ToList();

        metadata.DomainTypes = nextDomainTypes;
    }

    private bool Used(DomainTypeMetadata domainTypeMetadata)
    {
        var tableAttribute = domainTypeMetadata.DomainType.GetTableAttribute();

        return tableAttribute == null || tableAttribute.Schema == "app";
    }
}
