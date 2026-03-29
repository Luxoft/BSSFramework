namespace Framework.CodeGeneration.DTOGenerator.Server.Configuration;

public interface IGeneratorConfigurationContainer
{
    IServerGeneratorConfigurationBase<IServerGenerationEnvironmentBase> ServerDTO { get; }
}
