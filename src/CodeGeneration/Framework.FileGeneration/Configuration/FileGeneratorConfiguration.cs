using System.Collections.Immutable;

using Anch.Core;

namespace Framework.FileGeneration.Configuration;

public abstract class FileGeneratorConfiguration<TEnvironment> : IFileGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IFileGenerationEnvironment
{
    private readonly Lazy<ImmutableArray<Type>> domainTypesLazy;

    protected FileGeneratorConfiguration(TEnvironment environment)
    {
        this.Environment = environment;

        this.domainTypesLazy = LazyHelper.Create(() => this.GetDomainTypes().OrderBy(v => v.FullName).ToImmutableArray());
    }

    public TEnvironment Environment { get; }

    public ImmutableArray<Type> DomainTypes => this.domainTypesLazy.Value;

    protected virtual IEnumerable<Type> GetDomainTypes() => this.Environment.GetDefaultDomainTypes();
}
