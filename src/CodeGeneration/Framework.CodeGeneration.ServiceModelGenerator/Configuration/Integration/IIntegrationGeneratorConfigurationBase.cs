using System.CodeDom;

using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Integration;

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
