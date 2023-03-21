namespace Framework.DomainDriven.DTOGenerator.TypeScript.CodeTypeReferenceService;

/// <summary>
/// Observable code type reference service
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class ObservableCodeTypeReferenceService<TConfiguration> : DynamicCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public ObservableCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration, ObservableFileType.ObservableSimpleDTO, ObservableFileType.ObservableRichDTO)
    {
    }
}
