using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.Serialization;

namespace Framework.Attachments.TestGenerate
{
    public class ServerDTOGeneratorConfiguration : ServerGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public ServerDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override ClientDTORole MapToDomainRole { get; } = ClientDTORole.All;

        public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

        public override string Namespace { get; } = "Framework.Attachments.Generated.DTO";

        public override Type VersionType => typeof(long);

        public override bool IdentityIsReference { get; } = true;

        protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
        {
            return new DTORoleGeneratePolicy(DTORole.Client);
        }

        public override bool ForceGenerateProperties(Type domainType, DTOFileType fileType)
        {
            return true;
        }
    }
}
