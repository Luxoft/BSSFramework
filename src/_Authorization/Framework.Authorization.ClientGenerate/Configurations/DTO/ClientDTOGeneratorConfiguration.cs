using System;

using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.FacadeServiceProxyGenerator;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.Authorization.ClientGenerate
{
    public class ClientDTOGeneratorConfiguration : ClientGeneratorConfigurationBase<ClientGenerationEnvironment>
    {
        public ClientDTOGeneratorConfiguration(ClientGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

        protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
        {
            var compositePolicy =
                new DTOClientServiceGeneratePolicy<MainFacadeServiceProxyConfiguration>(this.Environment.MainFacadeServiceProxy)
                    .Or(new CustomClientDTOUsedGeneratePolicy());

            return new ServiceProxyClientDependencyGeneratePolicy(compositePolicy, this.GetTypeMaps());
        }
    }
}
