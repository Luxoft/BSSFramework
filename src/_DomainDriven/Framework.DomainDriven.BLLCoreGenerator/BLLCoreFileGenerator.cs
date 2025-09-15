using CommonFramework;

using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;
using Framework.Projection;
using Framework.Security;

namespace Framework.DomainDriven.BLLCoreGenerator;

/// <inheritdoc />
public class BLLCoreFileGenerator : BLLCoreFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
{
    public BLLCoreFileGenerator(IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration)
            : base(configuration)
    {
    }
}

/// <inheritdoc />
public class BLLCoreFileGenerator<TConfiguration> : CodeFileGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLCoreFileGenerator(TConfiguration configuration)
            : base(configuration)
    {
    }


    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        yield return new BLLContextInterfaceFileFactory<TConfiguration>(this.Configuration);

        yield return new SecurityDomainBLLBaseFileFactory<TConfiguration>(this.Configuration);

        foreach (var fileFactory in this.Configuration.Logics.GetFileFactories())
        {
            yield return fileFactory;
        }


        if (this.Configuration.GenerateAuthServices)
        {
            yield return new SecurityRuleHelperFileFactory<TConfiguration>(this.Configuration);

            yield return new RootSecurityServiceFileFactory<TConfiguration>(this.Configuration);
            yield return new RootSecurityServiceBaseFileFactory<TConfiguration>(this.Configuration);
            yield return new RootSecurityServiceInterfaceFileFactory<TConfiguration>(this.Configuration);

            yield return new RootSecurityPathContainerFileFactory<TConfiguration>(this.Configuration);

            foreach (var domainType in this.Configuration.SecurityServiceDomainTypes)
            {
                var useDependencySecurity = this.Configuration.Environment.GetProjectionEnvironment(domainType).Maybe(v => v.UseDependencySecurity);

                var isGenericProjection = !useDependencySecurity && domainType.HasSecurityNodeInterfaces() && domainType.IsProjection();

                if (!isGenericProjection)
                {
                    yield return new DomainSecurityServiceFileFactory<TConfiguration>(this.Configuration, domainType);
                }
            }
        }

        if (this.Configuration.GenerateValidation)
        {
            yield return new ValidationMapBaseFileFactory<TConfiguration>(this.Configuration);
            yield return new ValidationMapFileFactory<TConfiguration>(this.Configuration);

            yield return new ValidatorCompileCacheFileFactory<TConfiguration>(this.Configuration);

            yield return new ValidatorBaseFileFactory<TConfiguration>(this.Configuration);
            yield return new ValidatorFileFactory<TConfiguration>(this.Configuration);
            yield return new ValidatorInterfaceFileFactory<TConfiguration>(this.Configuration);
        }

        if (this.Configuration.GenerateFetchService)
        {
            yield return new MainFetchServiceBaseFileFactory<TConfiguration>(this.Configuration);
            yield return new MainFetchServiceFileFactory<TConfiguration>(this.Configuration);
        }
    }
}
