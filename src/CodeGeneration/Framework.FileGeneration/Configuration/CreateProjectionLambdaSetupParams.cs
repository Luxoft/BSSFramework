namespace Framework.FileGeneration.Configuration;

public record CreateProjectionLambdaSetupParams
{
    public required string AssemblyName { get; init; }

    public required string FullAssemblyName { get; init; }

    /// <summary>
    /// Использование безопасности через атрибут `DependencySecurityAttribute`
    /// </summary>
    public required bool UseDependencySecurity { get; init; }
}
