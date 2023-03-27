using System.CodeDom;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Server;

public class DefaultRichIntegrationDTOFileFactory<TConfiguration> : IntegrationDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase>
{
    public DefaultRichIntegrationDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override DTOFileType FileType { get; } = ServerFileType.RichIntegrationDTO;


    protected override bool HasMapToDomainObjectMethod { get; } = true;


    public override IEnumerable<CodeMemberMethod> GetServerMappingServiceMethods()
    {
        foreach (var method in base.GetServerMappingServiceMethods())
        {
            yield return method;
        }

        if (this.HasMapToDomainObjectMethod)
        {
            foreach (var masterType in this.Configuration.GetDomainTypeMasters(this.DomainType, this.FileType, true))
            {
                if (this.Configuration.IsPersistentObject(masterType))
                {
                    yield return this.GetMappingServiceToDomainObjectMethod(masterType);
                }
            }
        }
    }
}
