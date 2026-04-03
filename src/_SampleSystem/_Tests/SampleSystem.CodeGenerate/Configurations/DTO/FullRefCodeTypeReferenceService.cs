using Framework.CodeGeneration.DTOGenerator.CodeTypeReferenceService.Base;
using Framework.CodeGeneration.DTOGenerator.Configuration;
using Framework.CodeGeneration.DTOGenerator.FileTypes;

namespace SampleSystem.CodeGenerate;

public class FullRefCodeTypeReferenceService<TConfiguration>(TConfiguration configuration)
    : FixedCodeTypeReferenceService<TConfiguration>(configuration, BaseFileType.FullDTO, SampleSystemFileType.FullRefDTO)
    where TConfiguration : class, IDTOGeneratorConfiguration<IDTOGenerationEnvironment>;
