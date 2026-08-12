using System.Collections.Immutable;

namespace Framework.FileGeneration.Configuration;

public interface IFileGeneratorConfiguration
{
    ImmutableArray<Type> DomainTypes { get; }
}

public interface IFileGeneratorConfiguration<out TEnvironment> : IFileGeneratorConfiguration
    where TEnvironment : IFileGenerationEnvironment
{
    TEnvironment Environment { get; }
}
