using System;

using Framework.Authorization.WebApi;
using Framework.DomainDriven.DTOGenerator.TypeScript.Facade;

namespace Framework.Authorization.ClientGenerate
{
    public class MainFacadeServiceProxyConfiguration : Framework.DomainDriven.FacadeServiceProxyGenerator.GeneratorConfigurationBase<ClientGenerationEnvironment>
    {
        public MainFacadeServiceProxyConfiguration(ClientGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override ITypeScriptMethodPolicy Policy { get; } = new MainFacadePolicyBuilder();

        public override string Namespace { get; } = "Configurator.Client.Context.AuthService";

        protected override string NamespacePostfix => throw new NotImplementedException();

        public override Type BaseContract => typeof(AuthSLJsonController);
    }
}
