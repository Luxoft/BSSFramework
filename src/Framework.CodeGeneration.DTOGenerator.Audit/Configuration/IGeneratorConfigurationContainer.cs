namespace Framework.CodeGeneration.DTOGenerator.Audit.Configuration;

public interface IGeneratorConfigurationContainer
{
    IAuditDTOGeneratorConfigurationBase<IAuditDTOGenerationEnvironmentBase> AuditDTO { get; }
}
