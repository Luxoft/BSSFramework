using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;

public interface IAuditGeneratorConfigurationBase : IGeneratorConfigurationBase
{

}

public interface IAuditGeneratorConfigurationBase<out TEnvironment> : IAuditGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IAuditGenerationEnvironmentBase
{

}
