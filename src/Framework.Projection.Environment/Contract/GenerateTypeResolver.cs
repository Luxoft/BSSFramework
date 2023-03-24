using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.Projection.Contract;

internal class GenerateTypeResolver : ITypeResolver<Type>
{
    private readonly ProjectionContractEnvironment environment;

    internal readonly HashSet<Type> projectionContracts;

    private readonly Dictionary<Type, GeneratedType> generateTypes = new Dictionary<Type, GeneratedType>();


    public GenerateTypeResolver([NotNull] ProjectionContractEnvironment environment, [NotNull] ITypeSource typeSource)
    {
        if (environment == null) throw new ArgumentNullException(nameof(environment));
        if (typeSource == null) throw new ArgumentNullException(nameof(typeSource));

        this.environment = environment;

        this.projectionContracts = typeSource.GetTypes().Where(type => type.HasAttribute<ProjectionContractAttribute>()).ToHashSet();
    }

    public Type Resolve([NotNull] Type contractType)
    {
        if (contractType == null) throw new ArgumentNullException(nameof(contractType));

        return this.generateTypes.GetValueOrDefault(contractType)

               ?? (this.projectionContracts.Contains(contractType) ? new GeneratedType(this.environment, contractType, this.generateTypes) : null);
    }


    public IEnumerable<Type> GetTypes()
    {
        return this.generateTypes.Values;
    }
}
