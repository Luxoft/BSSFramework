using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileType;

namespace Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService;

public class MainCodeTypeReferenceService<TConfiguration>(TConfiguration configuration)
    : DynamicCodeTypeReferenceService<TConfiguration>(configuration, BaseFileType.SimpleDTO, BaseFileType.RichDTO)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override Type CollectionType => this.Configuration.ClientEditCollectionType;
}
