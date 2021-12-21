using System;
using Framework.Authorization.TestGenerate;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.FacadeServiceProxyGenerator;

namespace Framework.Authorization.ClientGenerate
{
    public class ClientGenerationEnvironment : GenerationEnvironmentBase, Framework.DomainDriven.FacadeServiceProxyGenerator.IGenerationEnvironmentBase
    {
        public readonly ClientDTOGeneratorConfiguration ClientDTO;

        public readonly MainFacadeServiceProxyConfiguration MainFacadeServiceProxy;

        public ClientGenerationEnvironment()
        {
            this.ClientDTO = new ClientDTOGeneratorConfiguration(this);

            this.MainFacadeServiceProxy = new MainFacadeServiceProxyConfiguration(this);
        }

        IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase> IGenerationEnvironmentBase.ClientDTO => this.ClientDTO;
    }
}
