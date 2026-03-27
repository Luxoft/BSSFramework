using System.CodeDom;

using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration.Integration;

public interface IIntegrationGeneratorConfigurationBase : IGeneratorConfigurationBase
{
    string InsertMethodName { get; }

    string SaveMethodName { get; }

    CodeExpression IntegrationSecurityRule { get; }
}

public interface IIntegrationGeneratorConfigurationBase<out TEnvironment> : IIntegrationGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
{

}
