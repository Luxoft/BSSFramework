using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration.Main;

public interface IMainGeneratorConfigurationBase : IGeneratorConfigurationBase
{
    bool GenerateQueryMethods { get; }
}

public interface IMainGeneratorConfigurationBase<out TEnvironment> : IMainGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
{

}
