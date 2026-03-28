using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.Projection.Lambda.ImplType;

namespace Framework.Projection.Lambda;

internal class GenerateTypeResolver(ProjectionLambdaEnvironment environment) : ITypeResolver<IProjection>
{
    private readonly ProjectionLambdaEnvironment environment = environment ?? throw new ArgumentNullException(nameof(environment));

    private readonly Dictionary<IProjection, GeneratedType> generateTypes = new Dictionary<IProjection, GeneratedType>();

    public Type TryResolve(IProjection projection)
    {
        if (projection == null) throw new ArgumentNullException(nameof(projection));

        return this.generateTypes.GetValueOrDefault(projection) ?? new GeneratedType(this.environment, projection, this.generateTypes);
    }

    public IEnumerable<Type> GetTypes()
    {
        return this.generateTypes.Values;
    }
}
