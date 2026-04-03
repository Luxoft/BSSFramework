using System.Collections.Immutable;

using Framework.Core;
using Framework.Core.TypeResolving;
using Framework.Core.TypeResolving.TypeSource;
using Framework.Projection.Contract.ImplType;

namespace Framework.Projection.Contract;

internal class GenerateTypeResolver : ITypeResolver<Type>
{
    private readonly ProjectionContractEnvironment environment;

    private readonly GenerateState generateState = new();

    internal readonly HashSet<Type> ProjectionContracts;

    public GenerateTypeResolver(ProjectionContractEnvironment environment, ITypeSource typeSource)
    {
        this.environment = environment;

        this.ProjectionContracts = [.. typeSource.Types.Where(type => type.HasAttribute<ProjectionContractAttribute>())];
    }

    public Type? TryResolve(Type contractType)
    {
        if (contractType == null) throw new ArgumentNullException(nameof(contractType));

        return this.generateState.Dict.GetValueOrDefault(contractType)

               ?? (this.ProjectionContracts.Contains(contractType) ? new GeneratedType(this.environment, contractType, this.generateState) : null);
    }


    public ImmutableHashSet<Type> Types => this.generateState.Types;
}
