using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Main;

public interface IMainGeneratorConfigurationBase : IGeneratorConfigurationBase
{
    bool GenerateQueryMethods { get; }
}

public interface IMainGeneratorConfigurationBase<out TEnvironment> : IMainGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
{

}
