namespace Framework.CodeGeneration.DomainMetadata;

public class CreateProjectionLambdaSetupParams
{
    public string AssemblyName { get; set; }

    public string FullAssemblyName { get; set; }

    /// <summary>
    /// Использование безопасности через атрибут `DependencySecurityAttribute`
    /// </summary>
    public bool UseDependencySecurity { get; set; }
}
