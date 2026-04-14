using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Server.Configuration;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory.Main.Base;

namespace SampleSystem.CodeGenerate.Configurations.DTO.Server.FileFactory.Base;

public abstract class RefDTOFileFactory<TConfiguration> : MainDTOFileFactory<TConfiguration>

        where TConfiguration : class, IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment>
{
    protected RefDTOFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType) =>
        this.CodeTypeReferenceService = new FullRefCodeTypeReferenceService<TConfiguration>(this.Configuration);

    public override IPropertyCodeTypeReferenceService CodeTypeReferenceService { get; }

    protected override bool HasMapToDomainObjectMethod { get; } = false;
}
