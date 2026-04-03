using System.CodeDom;

using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Integration;

public interface IIntegrationGeneratorConfiguration : ICodeGeneratorConfiguration
{
    string InsertMethodName { get; }

    string SaveMethodName { get; }

    CodeExpression IntegrationSecurityRule { get; }
}

public interface IIntegrationGeneratorConfiguration<out TEnvironment> : IIntegrationGeneratorConfiguration, IServiceModelGeneratorConfiguration<TEnvironment>
        where TEnvironment : IServiceModelGenerationEnvironment;
