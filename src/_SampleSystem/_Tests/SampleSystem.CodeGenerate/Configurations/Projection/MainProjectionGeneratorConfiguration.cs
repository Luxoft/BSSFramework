using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

using Framework.CodeDom;
using Framework.Core;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

public class MainProjectionGeneratorConfiguration : Framework.DomainDriven.ProjectionGenerator.GeneratorConfigurationBase<ServerGenerationEnvironment>
{
    public MainProjectionGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment, environment.MainProjectionEnvironment)
    {
    }

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
