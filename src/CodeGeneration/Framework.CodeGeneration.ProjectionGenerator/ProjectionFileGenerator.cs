using Framework.CodeGeneration.ProjectionGenerator.Configuration;
using Framework.CodeGeneration.ProjectionGenerator.Extensions;
using Framework.CodeGeneration.ProjectionGenerator.FileFactory;

namespace Framework.CodeGeneration.ProjectionGenerator;

public class ProjectionFileGenerator(IProjectionGeneratorConfiguration<IProjectionGenerationEnvironment> configuration)
    : ProjectionFileGenerator<IProjectionGeneratorConfiguration<IProjectionGenerationEnvironment>>(configuration);

public class ProjectionFileGenerator<TConfiguration>(TConfiguration configuration) : CodeFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IProjectionGeneratorConfiguration<IProjectionGenerationEnvironment>
{
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
