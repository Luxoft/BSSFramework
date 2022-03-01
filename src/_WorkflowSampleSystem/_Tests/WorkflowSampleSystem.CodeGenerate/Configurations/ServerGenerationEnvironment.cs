using Framework.DomainDriven.DTOGenerator.Audit;

using WorkflowSampleSystem.CodeGenerate.Configurations.Services.Audit;

namespace WorkflowSampleSystem.CodeGenerate
{
    public partial class ServerGenerationEnvironment : GenerationEnvironmentBase
    {
        public readonly string DTODataContractNamespace = "WorkflowSampleSystem";


        public readonly BLLCoreGeneratorConfiguration BLLCore;

        public readonly BLLGeneratorConfiguration BLL;

        public readonly ServerDTO.ServerDTOGeneratorConfiguration ServerDTO;

        public readonly ClientDTO.ClientDTOGeneratorConfiguration ClientDTO;

        public readonly MainServiceGeneratorConfiguration MainService;

        public readonly QueryServiceGeneratorConfiguration QueryService;

        public readonly IntegrationGeneratorConfiguration IntegrationService;

        public readonly DALGeneratorConfiguration DAL;

        public readonly ReportBLLGeneratorConfiguration ReportBLL;

        public readonly CustomReportServiceGeneratorConfiguration CustomReportService;

        public readonly MainProjectionGeneratorConfiguration MainProjection;

        public readonly LegacyProjectionGeneratorConfiguration LegacyProjection;

        public readonly AuditServiceGeneratorConfiguration AuditService;


        public ServerGenerationEnvironment()
        {
            this.BLLCore = new BLLCoreGeneratorConfiguration(this);

            this.BLL = new BLLGeneratorConfiguration(this);

            this.ServerDTO = new ServerDTO.ServerDTOGeneratorConfiguration(this);

            this.ClientDTO = new ClientDTO.ClientDTOGeneratorConfiguration(this);

            this.MainService = new MainServiceGeneratorConfiguration(this);

            this.QueryService = new QueryServiceGeneratorConfiguration(this);

            this.IntegrationService = new IntegrationGeneratorConfiguration(this);

            this.DAL = new DALGeneratorConfiguration(this);

            this.ReportBLL = new ReportBLLGeneratorConfiguration(this);

            this.CustomReportService = new CustomReportServiceGeneratorConfiguration(this);

            this.MainProjection = new MainProjectionGeneratorConfiguration(this);

            this.LegacyProjection = new LegacyProjectionGeneratorConfiguration(this);

            this.AuditService = new AuditServiceGeneratorConfiguration(this);

            this.AuditDTO = new AuditDTOGeneratorConfiguration(this);
        }
        //
        // public IMappingSettings GetMappingSettings(DatabaseName dbName, AuditDatabaseName dbAuditName)
        // {
        //     IEnumerable<XDocument> mappingXmls = this.DAL.GetMappingGenerators().Select(mg => mg.Generate());
        //     return new MappingSettings<PersistentDomainObjectBase>(mappingXmls, dbName, dbAuditName);
        // }

        public IAuditDTOGeneratorConfigurationBase<IAuditDTOGenerationEnvironmentBase> AuditDTO { get; }
    }
}
