namespace Framework.DomainDriven.DTOGenerator.Server
{
    public interface IGeneratorConfigurationContainer
    {
        IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase> ServerDTO { get; }
    }
}