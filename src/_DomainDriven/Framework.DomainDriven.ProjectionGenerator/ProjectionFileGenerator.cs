using System;
using System.Collections.Generic;

using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.ProjectionGenerator
{
    public class ProjectionFileGenerator : ProjectionFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
    {
        public ProjectionFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
            : base(configuration)
        {
        }
    }

    public class ProjectionFileGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
    {
        public ProjectionFileGenerator(TConfiguration configuration)
            : base(configuration)
        {
        }


        protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
        {
            foreach (var projectionType in this.Configuration.DomainTypes)
            {
                if (this.Configuration.Environment.HasCustomProjectionProperties(projectionType))
                {
                    yield return new CustomProjectionFileFactoryBase<TConfiguration>(this.Configuration, projectionType);
                }

                yield return new ProjectionFileFactory<TConfiguration>(this.Configuration, projectionType);
            }
        }
    }
}
