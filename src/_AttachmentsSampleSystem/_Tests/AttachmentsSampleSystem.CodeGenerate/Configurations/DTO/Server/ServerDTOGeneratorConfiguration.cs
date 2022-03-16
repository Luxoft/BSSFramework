using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;

using ServiceModelGenerator = Framework.DomainDriven.ServiceModelGenerator;

namespace AttachmentsSampleSystem.CodeGenerate.ServerDTO
{
    public class ServerDTOGeneratorConfiguration : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

        protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
        {
            var primitivePolicy = new ServiceModelGenerator.DTOServiceGeneratePolicy<MainServiceGeneratorConfiguration>(this.Environment.MainService)
                    .Or(new ServiceModelGenerator.DTOServiceGeneratePolicy<QueryServiceGeneratorConfiguration>(this.Environment.QueryService));

            return new ServerDependencyGeneratePolicy(primitivePolicy, this.GetTypeMaps());
        }
    }
}
