using Framework.Projection;
using Framework.Security;

namespace Framework.DomainDriven.Generation.Domain;

public static class GenerationEnvironmentExtensions
{
    public static IEnumerable<Enum> GetSecurityOperationCodes(this IGenerationEnvironment environment)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));

        return environment.SecurityOperationCodeType.IsEnum
                       ? environment.SecurityOperationCodeType.GetSecurityOperationCodes()
                       : new Enum[0];
    }

    public static IProjectionEnvironment GetProjectionEnvironment(this IGenerationEnvironment generationEnvironment, Type projectionType)
    {
        if (generationEnvironment == null) throw new ArgumentNullException(nameof(generationEnvironment));
        if (projectionType == null) throw new ArgumentNullException(nameof(projectionType));

        return generationEnvironment.ProjectionEnvironments.SingleOrDefault(pe => pe.Assembly.GetTypes().Contains(projectionType));
    }
}
