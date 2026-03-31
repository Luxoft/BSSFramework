namespace Framework.CodeGeneration.DTOGenerator.Audit.Configuration;

public interface IAuditDTOGeneratorConfigurationContainer
{
    IAuditDTOGeneratorConfiguration<IAuditDTOGenerationEnvironment> AuditDTO { get; }
}
