using Framework.CodeGeneration.DomainMetadata;
using Framework.CodeGeneration.Rendering;

namespace Framework.CodeGeneration.Configuration;

public interface IGeneratorConfiguration : IRenderingConfiguration
{
    IReadOnlyCollection<Type> DomainTypes { get; }
}

public interface IGeneratorConfiguration<out TEnvironment> : IGeneratorConfiguration
        where TEnvironment : IGenerationEnvironment
{
    TEnvironment Environment { get; }
}

public interface IGeneratorConfiguration<out TEnvironment, in TFileType> : IGeneratorConfiguration<TEnvironment>, ICodeTypeReferenceService<TFileType>
        where TEnvironment : IGenerationEnvironment
{
}
