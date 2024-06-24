using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLGenerator;

public class BLLFileGenerator : BLLFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
{
    public BLLFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
            : base(configuration)
    {
    }
}

public class BLLFileGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLFileGenerator(TConfiguration configuration)
            : base(configuration)
    {

    }


    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        yield return new BLLContextFileFactory<TConfiguration>(this.Configuration);

        foreach (var fileFactory in this.Configuration.Logics.GetFileFactories())
        {
            yield return fileFactory;
        }

        yield return new DefaultBLLFactoryFileFactory<TConfiguration>(this.Configuration);

        yield return new ImplementedBLLFactoryFileFactory<TConfiguration>(this.Configuration);
    }
}
