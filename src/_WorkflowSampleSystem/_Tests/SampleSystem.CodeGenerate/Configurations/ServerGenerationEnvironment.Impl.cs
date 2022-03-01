namespace SampleSystem.CodeGenerate
{
    public partial class ServerGenerationEnvironment :

        Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase,

        Framework.DomainDriven.BLLGenerator.IGenerationEnvironmentBase,

        Framework.DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase,

        Framework.DomainDriven.DTOGenerator.Client.IClientGenerationEnvironmentBase,

        Framework.DomainDriven.ServiceModelGenerator.IGenerationEnvironmentBase,

        Framework.DomainDriven.NHibernate.DALGenerator.IGenerationEnvironmentBase,

        Framework.CustomReports.Generation.BLL.ICustomReportBLLGeneratorConfigurationContainer,

        Framework.CustomReports.Generation.Facade.ICustomReportServiceGenerationEnvironmentBase,

        Framework.DomainDriven.ProjectionGenerator.IGenerationEnvironmentBase,

        Framework.CustomReports.Generation.BLL.ICustomReportGenerationEnvironmentBase,

        Framework.DomainDriven.ServiceModelGenerator.IAuditGenerationEnvironmentBase,

        Framework.DomainDriven.DTOGenerator.Audit.IAuditDTOGenerationEnvironmentBase
    {
        Framework.DomainDriven.BLLCoreGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.BLLCoreGenerator.IGenerationEnvironmentBase> Framework.DomainDriven.BLLCoreGenerator.IGeneratorConfigurationContainer.BLLCore => this.BLLCore;

        Framework.DomainDriven.BLLGenerator.IGeneratorConfigurationBase<Framework.DomainDriven.BLLGenerator.IGenerationEnvironmentBase> Framework.DomainDriven.BLLGenerator.IGeneratorConfigurationContainer.BLL => this.BLL;

        Framework.DomainDriven.DTOGenerator.Server.IServerGeneratorConfigurationBase<Framework.DomainDriven.DTOGenerator.Server.IServerGenerationEnvironmentBase> Framework.DomainDriven.DTOGenerator.Server.IGeneratorConfigurationContainer.ServerDTO => this.ServerDTO;

        Framework.CustomReports.Generation.BLL.ICustomReportBLLGeneratorConfiguration Framework.CustomReports.Generation.BLL.ICustomReportBLLGeneratorConfigurationContainer.ReportBLL => this.ReportBLL;
    }
}
