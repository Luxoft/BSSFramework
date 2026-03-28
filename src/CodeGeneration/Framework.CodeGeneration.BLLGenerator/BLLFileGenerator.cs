using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.FileFactory;
using Framework.CodeGeneration.BLLGenerator.FileFactory.DomainBLL;

namespace Framework.CodeGeneration.BLLGenerator;

public class BLLFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
    : BLLFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(configuration);

public class BLLFileGenerator<TConfiguration>(TConfiguration configuration) : CodeFileGenerator<TConfiguration>(configuration)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        yield return new BLLContextFileFactory<TConfiguration>(this.Configuration);

        foreach (var fileFactory in this.Configuration.Logics.GetFileFactories())
        {
            yield return fileFactory;
        }

        yield return new DefaultBLLFactoryFileFactory<TConfiguration>(this.Configuration);

        yield return new ImplementedBLLFactoryFileFactory<TConfiguration>(this.Configuration);

        if (this.Configuration.GenerateDTOFetchRuleExpander)
        {
            yield return new MainDTOFetchRuleExpanderBaseFileFactory<TConfiguration>(this.Configuration);
            yield return new MainDTOFetchRuleExpanderFileFactory<TConfiguration>(this.Configuration);
        }

        yield return new SecurityDomainBLLBaseFileFactory<TConfiguration>(this.Configuration);

        if (this.Configuration.GenerateValidation)
        {
            yield return new ValidationMapBaseFileFactory<TConfiguration>(this.Configuration);
            yield return new ValidationMapFileFactory<TConfiguration>(this.Configuration);

            yield return new ValidatorCompileCacheFileFactory<TConfiguration>(this.Configuration);

            yield return new ValidatorBaseFileFactory<TConfiguration>(this.Configuration);
            yield return new ValidatorFileFactory<TConfiguration>(this.Configuration);
            yield return new ValidatorInterfaceFileFactory<TConfiguration>(this.Configuration);
        }
    }
}
