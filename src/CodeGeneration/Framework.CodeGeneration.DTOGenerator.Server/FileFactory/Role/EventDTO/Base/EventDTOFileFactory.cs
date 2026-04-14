using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Server.CodeTypeReferenceService;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Role.EventDTO.Base;

public abstract class EventDTOFileFactory<TConfiguration> : RoleDTOFileFactory<TConfiguration>
        where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    protected EventDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType) =>
        this.CodeTypeReferenceService = new CryptCodeTypeReferenceService<TConfiguration>(this.Configuration, ServerFileType.SimpleEventDTO, ServerFileType.RichEventDTO);

    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }


    protected override string DataContractNamespace => this.Configuration.EventDataContractNamespace;
}
