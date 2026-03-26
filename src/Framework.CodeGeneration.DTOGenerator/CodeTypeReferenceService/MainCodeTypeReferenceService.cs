using Framework.CodeGeneration.DTOGenerator.Configuration;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class MainCodeTypeReferenceService<TConfiguration> : DynamicCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public MainCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration, FileType.FileType.SimpleDTO, FileType.FileType.RichDTO)
    {
    }

    public override Type CollectionType => this.Configuration.ClientEditCollectionType;
}
