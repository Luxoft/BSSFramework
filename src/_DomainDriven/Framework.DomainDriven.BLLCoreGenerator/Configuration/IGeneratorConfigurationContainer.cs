namespace Framework.DomainDriven.BLLCoreGenerator
{
    public interface IGeneratorConfigurationContainer
    {
        IGeneratorConfigurationBase<IGenerationEnvironmentBase> BLLCore { get; }
    }
}