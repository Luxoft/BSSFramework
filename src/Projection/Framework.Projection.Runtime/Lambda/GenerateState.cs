using System.Collections.Immutable;

using Framework.Projection.Lambda.ImplType;

namespace Framework.Projection.Lambda;

internal class GenerateState
{
    private readonly Dictionary<IProjection, GeneratedType> dict = [];

    private ImmutableHashSet<Type> types = [];

    public void Add(IProjection projection, GeneratedType generatedType)
    {
        this.types = this.types.Add(generatedType);

        this.dict.Add(projection, generatedType);
    }

    public IReadOnlyDictionary<IProjection, GeneratedType> Dict => this.dict;

    public ImmutableHashSet<Type> Types => this.types;
}
