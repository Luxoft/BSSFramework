using System.CodeDom;

namespace Framework.DomainDriven.ServiceModelGenerator
{
    public interface IIntegrationGeneratorConfigurationBase : IGeneratorConfigurationBase
    {
        string InsertMethodName { get; }

        string SaveMethodName { get; }

        CodeExpression IntegrationSecurityOperation { get; }
    }

    public interface IIntegrationGeneratorConfigurationBase<out TEnvironment> : IIntegrationGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
    {

    }
}