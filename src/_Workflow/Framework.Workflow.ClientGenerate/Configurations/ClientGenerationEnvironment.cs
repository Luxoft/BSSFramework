using System;

using Framework.Workflow.TestGenerate;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.FacadeServiceProxyGenerator;
using Framework.Projection;

namespace Framework.Workflow.ClientGenerate
{
    public class ClientGenerationEnvironment : GenerationEnvironmentBase, Framework.DomainDriven.FacadeServiceProxyGenerator.IGenerationEnvironmentBase
    {
        public readonly ClientDTOGeneratorWorkflow ClientDTO;

        public readonly MainFacadeServiceProxyWorkflow MainFacadeServiceProxy;

        public ClientGenerationEnvironment()
        {
            this.ClientDTO = new ClientDTOGeneratorWorkflow(this);

            this.MainFacadeServiceProxy = new MainFacadeServiceProxyWorkflow(this);
        }

        protected override IProjectionEnvironment GetProjectionEnvironment()
        {
            return new AlreadyImplementedRuntimeProjectionEnvironment(base.GetProjectionEnvironment());
        }


        IClientGeneratorConfigurationBase<IClientGenerationEnvironmentBase> IGenerationEnvironmentBase.ClientDTO => this.ClientDTO;
    }
}
