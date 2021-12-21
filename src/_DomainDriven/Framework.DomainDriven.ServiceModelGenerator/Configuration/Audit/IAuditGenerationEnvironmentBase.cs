namespace Framework.DomainDriven.ServiceModelGenerator
{
    public interface IAuditGenerationEnvironmentBase : IGenerationEnvironmentBase,

        Framework.DomainDriven.DTOGenerator.Audit.IGeneratorConfigurationContainer
    {

    }
}