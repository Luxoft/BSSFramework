using System;

using Framework.Core;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.Workflow.Domain;

using FileType = Framework.DomainDriven.DTOGenerator.FileType;

namespace Framework.Workflow.TestGenerate
{
    public class ServerDTOGeneratorConfiguration : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

        public override bool IdentityIsReference { get; } = true;

        protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
        {
            var basePolicy = new DTOServiceGeneratePolicy<MainServiceGeneratorConfiguration>(this.Environment.MainService).Or(new WorkflowCustomGeneratePolicy());

            return new ServerDependencyGeneratePolicy(
                basePolicy.Add((type, role) => type == typeof(MassExecuteCommandRequest) && role == FileType.RichDTO),
                this.GetTypeMaps());
        }

        public override bool ForceGenerateProperties(Type domainType, DTOFileType fileType)
        {
            return true;
        }
    }
}
