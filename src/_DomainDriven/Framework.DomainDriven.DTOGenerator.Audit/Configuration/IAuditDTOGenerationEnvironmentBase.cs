using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.DTOGenerator.Audit
{
    public interface IAuditDTOGenerationEnvironmentBase : IGenerationEnvironment,

        Server.IGeneratorConfigurationContainer
    {
    }
}