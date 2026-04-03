namespace Framework.CodeGeneration.DTOGenerator.Server.Configuration;

public interface IServerDTOGeneratorConfigurationContainer
{
    IServerDTOGeneratorConfiguration<IServerDTOGenerationEnvironment> ServerDTO { get; }
}
