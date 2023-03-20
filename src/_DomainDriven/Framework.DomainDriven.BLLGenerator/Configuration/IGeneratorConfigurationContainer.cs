namespace Framework.DomainDriven.BLLGenerator;

public interface IGeneratorConfigurationContainer
{
    IGeneratorConfigurationBase<IGenerationEnvironmentBase> BLL { get; }
}
