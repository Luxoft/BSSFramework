namespace Framework.DomainDriven.ServiceModelGenerator;

public interface IMainGeneratorConfigurationBase : IGeneratorConfigurationBase
{
    bool GenerateQueryMethods { get; }
}

public interface IMainGeneratorConfigurationBase<out TEnvironment> : IMainGeneratorConfigurationBase, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : IGenerationEnvironmentBase
{

}
