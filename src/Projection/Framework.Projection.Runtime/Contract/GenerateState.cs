using System.Collections.Immutable;

using Framework.Projection.Contract.ImplType;

namespace Framework.Projection.Contract;

internal class GenerateState
{
    private readonly Dictionary<Type, GeneratedType> dict = [];

    private ImmutableHashSet<Type> types = [];

    public void Add(Type contractType, GeneratedType generatedType)
    {
        this.types = this.types.Add(generatedType);

        this.dict.Add(contractType, generatedType);
    }

    public IReadOnlyDictionary<Type, GeneratedType> Dict => this.dict;

    public ImmutableHashSet<Type> Types => this.types;
}
