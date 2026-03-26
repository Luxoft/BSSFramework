namespace Framework.DomainDriven.ServiceModelGenerator;

public interface IAuditGeneratorConfigurationBase : IGeneratorConfigurationBase
{

}

public interface IAuditGeneratorConfigurationBase<out TEnvironment> : IAuditGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IAuditGenerationEnvironmentBase
{

}
