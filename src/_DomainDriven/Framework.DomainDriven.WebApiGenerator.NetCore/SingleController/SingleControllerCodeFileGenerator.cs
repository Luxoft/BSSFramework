using Framework.DomainDriven.Generation;
using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.DomainDriven.WebApiGenerator.NetCore.SingleController;

public class SingleControllerCodeFileGenerator : SingleControllerCodeFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
{
    public SingleControllerCodeFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
            : base(configuration)
    {
    }
}

public class SingleControllerCodeFileGenerator<TConfiguration> : ServiceModelFileGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SingleControllerCodeFileGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }

    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        foreach (var baseFileGenerator in base.GetInternalFileGenerators())
        {
            if (baseFileGenerator is ImplementFileFactory<TConfiguration> fileFactory)
            {
                yield return new SingleControllerCodeFileFactory<TConfiguration>(this.Configuration, fileFactory.DomainType);
            }
        }
    }
}
