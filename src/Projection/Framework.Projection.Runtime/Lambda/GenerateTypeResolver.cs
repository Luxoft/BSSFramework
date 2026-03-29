using System.Collections.Immutable;

using Framework.Core.TypeResolving;
using Framework.Projection.Lambda.ImplType;

namespace Framework.Projection.Lambda;

internal class GenerateTypeResolver(ProjectionLambdaEnvironment environment) : ITypeResolver<IProjection>
{
    private readonly ProjectionLambdaEnvironment environment = environment ?? throw new ArgumentNullException(nameof(environment));

    private readonly GenerateState generateState = new();


    public Type TryResolve(IProjection projection)
    {
        if (projection == null) throw new ArgumentNullException(nameof(projection));

        return this.generateState.Dict.GetValueOrDefault(projection) ?? new GeneratedType(this.environment, projection, this.generateState);
    }

    public ImmutableHashSet<Type> Types => this.generateState.Types;
}
