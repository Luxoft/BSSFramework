using System;
using System.Collections.Generic;

using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.FacadeServiceProxyGenerator
{
    public class FacadeServiceProxyGeneratorFileGenerator : FacadeServiceProxyGeneratorFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
    {
        public FacadeServiceProxyGeneratorFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
            : base(configuration)
        {
        }
    }

    public class FacadeServiceProxyGeneratorFileGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public FacadeServiceProxyGeneratorFileGenerator(TConfiguration configuration)
            : base(configuration)
        {
        }


        protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
        {
            yield return new ClientContactFileFactory<TConfiguration>(this.Configuration);
            yield return new SimpleClientImplFileFactory<TConfiguration>(this.Configuration);
            yield return new ServiceProxyFileFactory<TConfiguration>(this.Configuration);
        }
    }
}
