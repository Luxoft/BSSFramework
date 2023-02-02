using Framework.DomainDriven.DTOGenerator.Audit;

using SampleSystem.CodeGenerate.Configurations.Services.Audit;

namespace SampleSystem.CodeGenerate
{
    public partial class ServerGenerationEnvironment : GenerationEnvironmentBase
    {
        public readonly string DTODataContractNamespace = "SampleSystem";


        public readonly BLLCoreGeneratorConfiguration BLLCore;

        public readonly BLLGeneratorConfiguration BLL;

        public readonly ServerDTO.ServerDTOGeneratorConfiguration ServerDTO;

        public readonly MainServiceGeneratorConfiguration MainService;

        public readonly QueryServiceGeneratorConfiguration QueryService;

        public readonly IntegrationGeneratorConfiguration IntegrationService;

        public readonly DALGeneratorConfiguration DAL;

        public readonly MainProjectionGeneratorConfiguration MainProjection;

        public readonly LegacyProjectionGeneratorConfiguration LegacyProjection;

        public readonly AuditServiceGeneratorConfiguration AuditService;


        public ServerGenerationEnvironment()
        {
            this.BLLCore = new BLLCoreGeneratorConfiguration(this);

            this.BLL = new BLLGeneratorConfiguration(this);

            this.ServerDTO = new ServerDTO.ServerDTOGeneratorConfiguration(this);

            this.MainService = new MainServiceGeneratorConfiguration(this);

            this.QueryService = new QueryServiceGeneratorConfiguration(this);

            this.IntegrationService = new IntegrationGeneratorConfiguration(this);

            this.DAL = new DALGeneratorConfiguration(this);

            this.MainProjection = new MainProjectionGeneratorConfiguration(this);

            this.LegacyProjection = new LegacyProjectionGeneratorConfiguration(this);

            this.AuditService = new AuditServiceGeneratorConfiguration(this);

            this.AuditDTO = new AuditDTOGeneratorConfiguration(this);
        }

        public IAuditDTOGeneratorConfigurationBase<IAuditDTOGenerationEnvironmentBase> AuditDTO { get; }
    }
}
