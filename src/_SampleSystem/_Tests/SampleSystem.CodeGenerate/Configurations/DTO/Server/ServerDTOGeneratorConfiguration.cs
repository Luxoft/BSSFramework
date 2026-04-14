using System.Reflection;

using Framework.Application.Events;
using Framework.BLL.Domain.Serialization;
using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;
using Framework.CodeGeneration.DTOGenerator.GeneratePolicy;
using Framework.CodeGeneration.DTOGenerator.Map;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator.DTOGeneratePolicy;

using SampleSystem.CodeGenerate.Configurations.Services.Integration;
using SampleSystem.CodeGenerate.Configurations.Services.Main;
using SampleSystem.CodeGenerate.Configurations.Services.QueryService;
using SampleSystem.EventMetadata;

namespace SampleSystem.CodeGenerate.Configurations.DTO.Server;

public class ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment) : ServerDTOGeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override string DataContractNamespace => nameof(SampleSystem);

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> FullRefDTOFileFactoryHeader { get; } = SampleSystemFileType.FullRefDTO.ToHeader();

    protected virtual ICodeFileFactoryHeader<MainDTOFileType> SimpleRefFullDetailDTOFileFactoryHeader { get; } = SampleSystemFileType.SimpleRefFullDetailDTO.ToHeader();

    public override IDomainObjectEventMetadata DomainObjectEventMetadata { get; } = new SampleSystemDomainObjectEventMetadata();

    public override IEnumerable<GenerateTypeMap> GetTypeMaps()
    {
        foreach (var baseTypeMap in base.GetTypeMaps())
        {
            yield return baseTypeMap;
        }

        foreach (var type in this.DomainTypes)
        {
            yield return this.GetTypeMap(type, SampleSystemFileType.FullRefDTO);

            yield return this.GetTypeMap(type, SampleSystemFileType.SimpleRefFullDetailDTO);
        }
    }

    public override ILayerCodeTypeReferenceService GetLayerCodeTypeReferenceService(DTOFileType fileType)
    {
        if (fileType == SampleSystemFileType.SimpleRefFullDetailDTO || fileType == SampleSystemFileType.FullRefDTO)
        {
            return new FullRefCodeTypeReferenceService<ServerDTOGeneratorConfiguration>(this);
        }
        else
        {
            return base.GetLayerCodeTypeReferenceService(fileType);
        }
    }

    protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
    {
        var primitivePolicy = new SampleSystemExtGeneratePolicy()
                              .Or(new DTOServiceGeneratePolicy<MainServiceGeneratorConfiguration>(this.Environment.MainService))
                              .Or(new DTOServiceGeneratePolicy<QueryServiceGeneratorConfiguration>(this.Environment.QueryService))
                              .Or(new DTOServiceGeneratePolicy<IntegrationGeneratorConfiguration>(this.Environment.IntegrationService))
                              //.Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<AuditServiceGeneratorConfiguration>(this.Environment.AuditService))
                              .Or(new DTORoleGeneratePolicy(DTORole.Event))
                              .Or(new DTORoleGeneratePolicy(DTORole.Client, ClientDTORole.Projection// | ClientDTORole.Update
                                                           ))
                              .Or(new SampleSystemEventDTORoleGeneratePolicy());

        return new SampleSystemServerDependencyGeneratePolicy(primitivePolicy, this.GetTypeMaps());
    }

    protected override IEnumerable<ICodeFileFactoryHeader<BaseFileType>> GetFileFactoryHeaders() =>
        base.GetFileFactoryHeaders().Concat(
        [
            this.FullRefDTOFileFactoryHeader,
                                                this.SimpleRefFullDetailDTOFileFactoryHeader
        ]);

    protected override IEnumerable<PropertyInfo> GetInternalDomainTypeProperties(Type domainType, DTOFileType fileType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        if (fileType == SampleSystemFileType.FullRefDTO)
        {
            return this.GetFullRefDTOProperties(domainType);
        }
        else if (fileType == SampleSystemFileType.SimpleRefFullDetailDTO)
        {
            return this.GetSimpleRefFullDetailDTOProperties(domainType);
        }
        else
        {
            return base.GetInternalDomainTypeProperties(domainType, fileType);
        }
    }
}
