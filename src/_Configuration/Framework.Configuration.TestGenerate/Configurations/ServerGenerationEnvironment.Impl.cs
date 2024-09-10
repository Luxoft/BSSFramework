namespace Framework.Configuration.TestGenerate;

public partial class ServerGenerationEnvironment :

        DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase,

        DomainDriven.BLLGenerator.IGenerationEnvironmentBase,

        DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase,

        Framework.DomainDriven.NHibernate.DALGenerator.IGenerationEnvironmentBase,

        DomainDriven.DTOGenerator.Audit.IAuditDTOGenerationEnvironmentBase,

        DomainDriven.ServiceModelGenerator.IAuditGenerationEnvironmentBase
{
    DomainDriven.BLLCoreGenerator.IGeneratorConfigurationBase<DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase> DomainDriven.BLLCoreGenerator.IGeneratorConfigurationContainer.BLLCore => this.BLLCore;

    DomainDriven.BLLGenerator.IGeneratorConfigurationBase<DomainDriven.BLLGenerator.IGenerationEnvironmentBase> DomainDriven.BLLGenerator.IGeneratorConfigurationContainer.BLL => this.BLL;

    DomainDriven.DTOGenerator.Server.IServerGeneratorConfigurationBase<DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase> DomainDriven.DTOGenerator.Server.IGeneratorConfigurationContainer.ServerDTO => this.ServerDTO;

    DomainDriven.DTOGenerator.Audit.IAuditDTOGeneratorConfigurationBase<DomainDriven.DTOGenerator.Audit.IAuditDTOGenerationEnvironmentBase> DomainDriven.DTOGenerator.Audit.IGeneratorConfigurationContainer.AuditDTO => this.AuditDTO;
}
