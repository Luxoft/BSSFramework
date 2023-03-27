using System.CodeDom;

using Framework.DomainDriven.Generation;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.Projection;

namespace Framework.DomainDriven.WebApiGenerator.NetCore;

public class WebApiNetCoreFileGenerator : CodeFileGenerator<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>
{
    private readonly bool generateMainDomainObjectController;

    private readonly List<CodeAttributeDeclaration> additionalControllerAttributes;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebApiNetCoreFileGenerator"/> class.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    /// <param name="generateMainDomainObjectController">if set to <c>true</c> [generate main domain object controller] if projection exists.</param>
    /// <param name="additionalControllerAttributes">Add attributes to controller</param>
    public WebApiNetCoreFileGenerator(
            IGeneratorConfigurationBase<IGenerationEnvironmentBase> configuration,
            bool generateMainDomainObjectController = true,
            List<CodeAttributeDeclaration> additionalControllerAttributes = null)
            : base(configuration)
    {
        this.generateMainDomainObjectController = generateMainDomainObjectController;
        this.additionalControllerAttributes = additionalControllerAttributes;
    }

    protected override IEnumerable<ICodeFile> GetInternalFileGenerators()
    {
        var domainTypes = this.Configuration.DomainTypes.Where(this.Configuration.HasMethods).ToHashSet();

        if (this.generateMainDomainObjectController)
        {
            var types = domainTypes
                        .Select(z => z.GetProjectionSourceType(false))
                        .Where(z => null != z)
                        .Where(z => !domainTypes.Contains(z))
                        .OrderBy(z => z.FullName);

            foreach (var domainType in types)
            {
                yield return new WebApiNetCoreFileFactory<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(
                 this.Configuration,
                 domainType,
                 this.additionalControllerAttributes);
            }
        }

        foreach (var domainType in domainTypes)
        {
            yield return new WebApiNetCoreFileFactory<IGeneratorConfigurationBase<IGenerationEnvironmentBase>>(
             this.Configuration,
             domainType,
             this.additionalControllerAttributes);
        }
    }
}
