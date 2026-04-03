using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.Configuration;

public interface ICodeGeneratorConfiguration<out TEnvironment, in TFileType> : ICodeGeneratorConfiguration<TEnvironment>, ICodeTypeReferenceService<TFileType>
    where TEnvironment : ICodeGenerationEnvironment;

public interface ICodeGeneratorConfiguration : IFileGeneratorConfiguration
{
    string? Namespace { get; }
}

public interface ICodeGeneratorConfiguration<out TEnvironment> : IFileGeneratorConfiguration<TEnvironment>, ICodeGeneratorConfiguration
    where TEnvironment : IFileGenerationEnvironment;
