using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.DTOGenerator;
using Framework.DomainDriven.DTOGenerator.Client;
using Framework.DomainDriven.FacadeServiceProxyGenerator;
using Framework.DomainDriven.Generation.Domain;
using Framework.Graphviz;

namespace Framework.Workflow.ClientGenerate
{
    public class ClientDTOGeneratorWorkflow : ClientGeneratorConfigurationBase<ClientGenerationEnvironment>
    {
        public ClientDTOGeneratorWorkflow(ClientGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override string DataContractNamespace => this.Environment.DTODataContractNamespace;

        public override string Namespace { get; } = "Framework.Workflow.Generated.DTO";

        protected override IEnumerable<Type> GetDomainTypes()
        {
            return base.GetDomainTypes().Concat(new[]
            {
                typeof(GraphvizFormat)
            });
        }

        protected override IGeneratePolicy<RoleFileType> CreateGeneratePolicy()
        {
            var compositePolicy =
                new DTOClientServiceGeneratePolicy<MainFacadeServiceProxyWorkflow>(this.Environment.MainFacadeServiceProxy)
                    .Or(new CustomClientDTOUsedGeneratePolicy());

            return new ServiceProxyClientDependencyGeneratePolicy(compositePolicy, this.GetTypeMaps())
                  .Add(t => t == typeof(GraphvizFormat));
        }
    }
}
