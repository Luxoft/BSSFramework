using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Main;

public interface IMainGeneratorConfiguration : ICodeGeneratorConfiguration
{
    bool GenerateQueryMethods { get; }
}

public interface IMainGeneratorConfiguration<out TEnvironment> : IMainGeneratorConfiguration, IServiceModelGeneratorConfiguration<TEnvironment>
        where TEnvironment : IServiceModelGenerationEnvironment;
