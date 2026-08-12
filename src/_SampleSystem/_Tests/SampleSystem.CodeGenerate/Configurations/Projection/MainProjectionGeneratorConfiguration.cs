using System.CodeDom;
using System.Reflection;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ProjectionGenerator.Configuration;
using Framework.Core;

using SampleSystem.Domain._CustomProjectionAttribute;

namespace SampleSystem.CodeGenerate.Configurations.Projection;

public class MainProjectionGeneratorConfiguration(ServerGenerationEnvironment environment)
    : ProjectionGeneratorConfigurationBase<ServerGenerationEnvironment>(environment, environment.MainProjectionEnvironment)
{
    public override IEnumerable<CodeAttributeDeclaration> GetDomainTypeAttributeDeclarations(Type domainType)
    {
        foreach (var baseAttr in base.GetDomainTypeAttributeDeclarations(domainType))
        {
            yield return baseAttr;
        }

        if (domainType.HasAttribute<ExampleCustomProjectionAttribute>())
        {
            yield return typeof(ExampleCustomProjectionAttribute).ToTypeReference().ToAttributeDeclaration();
        }
    }

    public override IEnumerable<CodeAttributeDeclaration> GetPropertyAttributeDeclarations(PropertyInfo property)
    {
        foreach (var baseAttr in base.GetPropertyAttributeDeclarations(property))
        {
            yield return baseAttr;
        }

        if (property.HasAttribute<ExampleCustomProjectionPropertyAttribute>())
        {
            yield return typeof(ExampleCustomProjectionPropertyAttribute).ToTypeReference().ToAttributeDeclaration();
        }
    }
}
