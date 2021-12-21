using Framework.DomainDriven.DTOGenerator;

namespace SampleSystem.CodeGenerate
{
    public class FullRefCodeTypeReferenceService<TConfiguration> : FixedCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public FullRefCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration, FileType.FullDTO, SampleSystemFileType.FullRefDTO)
        {
        }
    }
}