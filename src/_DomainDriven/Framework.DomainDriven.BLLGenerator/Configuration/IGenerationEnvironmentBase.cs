namespace Framework.DomainDriven.BLLGenerator;

public interface IGenerationEnvironmentBase : Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase
{
    IGeneratorConfigurationBase<IGenerationEnvironmentBase> BLL { get; }
}
