using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;

public interface IAuditGenerationEnvironmentBase : IGenerationEnvironmentBase,

                                                   DTOGenerator.Audit.Configuration.IGeneratorConfigurationContainer
{

}
