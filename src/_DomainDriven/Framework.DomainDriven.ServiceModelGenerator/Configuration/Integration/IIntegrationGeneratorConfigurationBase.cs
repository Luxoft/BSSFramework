using System.CodeDom;

namespace Framework.DomainDriven.ServiceModelGenerator;

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
