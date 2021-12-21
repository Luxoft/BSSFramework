using System;

using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;
using Framework.DomainDriven.FacadeServiceProxyGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.Workflow.WebApi;

namespace Framework.Workflow.ClientGenerate
{
    public class MainFacadeServiceProxyWorkflow : Framework.DomainDriven.FacadeServiceProxyGenerator.GeneratorConfigurationBase<ClientGenerationEnvironment>
    {
        public MainFacadeServiceProxyWorkflow(ClientGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override ITypeScriptMethodPolicy Policy { get; } = new MainFacadePolicyBuilder();

        public override string Namespace { get; } = "Configurator.Client.Context.WorkflowService";

        protected override string NamespacePostfix => throw new NotImplementedException();

        public override Type BaseContract => typeof(WorkflowSLJsonController);

        protected override ICodeFileFactoryHeader<FileType> ClientContactFileFactoryHeader { get; } =

            new CodeFileFactoryHeader<FileType>(FileType.ClientContact, "", _ => "IWorkflowFacade");

        protected override ICodeFileFactoryHeader<FileType> SimpleClientFileFactoryHeader { get; } =

            new CodeFileFactoryHeader<FileType>(FileType.SimpleClientImpl, "", _ => "WorkflowFacadeClient");

        protected override ICodeFileFactoryHeader<FileType> ServiceProxyFileFactoryHeader { get; } =

            new CodeFileFactoryHeader<FileType>(FileType.ServiceProxy, "", _ => "WorkflowFacadeServiceProxy");
    }
}
