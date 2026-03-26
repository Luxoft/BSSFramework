namespace Framework.DomainDriven.DTOGenerator.Audit;

public interface IGeneratorConfigurationContainer
{
    IAuditDTOGeneratorConfigurationBase<IAuditDTOGenerationEnvironmentBase> AuditDTO { get; }
}
