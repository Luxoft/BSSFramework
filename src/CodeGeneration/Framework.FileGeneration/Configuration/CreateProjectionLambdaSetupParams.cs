namespace Framework.FileGeneration.Configuration;

public record CreateProjectionLambdaSetupParams
{
    public required string AssemblyName { get; init; }

    public required string FullAssemblyName { get; init; }

    /// <summary>
    /// Use of security through the `DependencySecurityAttribute` attribute
    /// </summary>
    public required bool UseDependencySecurity { get; init; }
}
