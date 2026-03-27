using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration.Audit;

public interface IAuditGenerationEnvironmentBase : IGenerationEnvironmentBase,

                                                   DTOGenerator.Audit.Configuration.IGeneratorConfigurationContainer
{

}
