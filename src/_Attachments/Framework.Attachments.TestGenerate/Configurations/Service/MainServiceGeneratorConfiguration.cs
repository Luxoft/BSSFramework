using System;

using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.Attachments.TestGenerate
{
    public class MainServiceGeneratorConfiguration : MainGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        public MainServiceGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }


        public override string ServiceContractNamespace => this.Environment.ServiceContractNamespace;
    }
}
