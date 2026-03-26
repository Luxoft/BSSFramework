using Framework.Projection;

namespace Framework.CodeGeneration.DomainMetadata;

public static class GenerationEnvironmentExtensions
{
    public static IProjectionEnvironment GetProjectionEnvironment(this IGenerationEnvironment generationEnvironment, Type projectionType)
    {
        if (generationEnvironment == null) throw new ArgumentNullException(nameof(generationEnvironment));
        if (projectionType == null) throw new ArgumentNullException(nameof(projectionType));

        return generationEnvironment.ProjectionEnvironments.SingleOrDefault(pe => pe.Assembly.GetTypes().Contains(projectionType));
    }
}
