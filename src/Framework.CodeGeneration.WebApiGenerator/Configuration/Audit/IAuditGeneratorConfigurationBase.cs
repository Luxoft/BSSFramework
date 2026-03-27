using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration.Audit;

public interface IAuditGeneratorConfigurationBase : IGeneratorConfigurationBase
{

}

public interface IAuditGeneratorConfigurationBase<out TEnvironment> : IAuditGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IAuditGenerationEnvironmentBase
{

}
