using Framework.DomainDriven.DTOGenerator;

namespace WorkflowSampleSystem.CodeGenerate
{
    public class FullRefCodeTypeReferenceService<TConfiguration> : FixedCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public FullRefCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration, FileType.FullDTO, WorkflowSampleSystemFileType.FullRefDTO)
        {
        }
    }
}