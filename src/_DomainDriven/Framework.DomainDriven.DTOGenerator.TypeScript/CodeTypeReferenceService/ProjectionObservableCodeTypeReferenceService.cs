namespace Framework.DomainDriven.DTOGenerator.TypeScript.CodeTypeReferenceService;

/// <summary>
/// Observable projection code type reference service
/// </summary>
/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
public class ProjectionObservableCodeTypeReferenceService<TConfiguration> : DynamicCodeTypeReferenceService<TConfiguration>
        where TConfiguration : class, ITypeScriptDTOGeneratorConfiguration<ITypeScriptGenerationEnvironmentBase>
{
    public ProjectionObservableCodeTypeReferenceService(TConfiguration configuration)
            : base(configuration, ObservableFileType.ObservableProjectionDTO, ObservableFileType.ObservableProjectionDTO)
    {
    }
}
