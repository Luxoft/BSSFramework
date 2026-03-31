using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;

namespace SampleSystem.CodeGenerate;

public class FullRefCodeTypeReferenceService<TConfiguration>(TConfiguration configuration)
    : FixedCodeTypeReferenceService<TConfiguration>(configuration, FileType.FullDTO, SampleSystemFileType.FullRefDTO)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>;
