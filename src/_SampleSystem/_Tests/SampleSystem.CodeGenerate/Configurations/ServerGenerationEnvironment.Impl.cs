namespace SampleSystem.CodeGenerate;

public partial class ServerGenerationEnvironment :
        Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase,
        Framework.DomainDriven.BLLGenerator.IGenerationEnvironmentBase,
        Framework.DomainDriven.NHibernate.DALGenerator.IGenerationEnvironmentBase,
        Framework.DomainDriven.ProjectionGenerator.IGenerationEnvironmentBase,
        Framework.DomainDriven.ServiceModelGenerator.IAuditGenerationEnvironmentBase,
        Framework.DomainDriven.DTOGenerator.Audit.IAuditDTOGenerationEnvironmentBase
{
    Framework.DomainDriven.BLLCoreGenerator.IGeneratorConfigurationBase<
            Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase>
            Framework.DomainDriven.BLLCoreGenerator.IGeneratorConfigurationContainer.BLLCore => this.BLLCore;

    Framework.DomainDriven.DTOGenerator.Server.IServerGeneratorConfigurationBase<
            Framework.DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase>
            Framework.DomainDriven.DTOGenerator.Server.IGeneratorConfigurationContainer.ServerDTO => this.ServerDTO;
}
