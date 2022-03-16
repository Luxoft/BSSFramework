using Framework.DomainDriven.DTOGenerator.Audit;

namespace AttachmentsSampleSystem.CodeGenerate
{
    public partial class ServerGenerationEnvironment : GenerationEnvironmentBase
    {
        public readonly string DTODataContractNamespace = "AttachmentsSampleSystem";


        public readonly BLLCoreGeneratorConfiguration BLLCore;

        public readonly BLLGeneratorConfiguration BLL;

        public readonly ServerDTO.ServerDTOGeneratorConfiguration ServerDTO;

        public readonly MainServiceGeneratorConfiguration MainService;

        public readonly QueryServiceGeneratorConfiguration QueryService;

        public readonly DALGeneratorConfiguration DAL;


        public ServerGenerationEnvironment()
        {
            this.BLLCore = new BLLCoreGeneratorConfiguration(this);

            this.BLL = new BLLGeneratorConfiguration(this);

            this.ServerDTO = new ServerDTO.ServerDTOGeneratorConfiguration(this);

            this.MainService = new MainServiceGeneratorConfiguration(this);

            this.QueryService = new QueryServiceGeneratorConfiguration(this);

            this.DAL = new DALGeneratorConfiguration(this);
        }
    }
}
