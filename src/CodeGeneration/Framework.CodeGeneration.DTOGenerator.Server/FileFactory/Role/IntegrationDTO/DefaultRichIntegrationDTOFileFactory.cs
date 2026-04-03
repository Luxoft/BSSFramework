using System.CodeDom;

using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.IntegrationDTO.Base;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.IntegrationDTO;

public class DefaultRichIntegrationDTOFileFactory<TConfiguration>(TConfiguration configuration, Type domainType)
    : IntegrationDTOFileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
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
