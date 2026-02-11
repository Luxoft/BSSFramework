using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

public class BLLGeneratorConfiguration(ServerGenerationEnvironment environment) : Framework.DomainDriven.BLLGenerator.GeneratorConfigurationBase<
    ServerGenerationEnvironment>(environment)
{
    public override Type OperationContextType { get; } = typeof(SampleSystemOperationContext);

    /// <summary>
    /// Do not generate BLL Constructors
    /// </summary>
    public override bool GenerateBllConstructor(Type domainType) =>
        domainType switch
        {
            { } type when type == typeof(Country) => false,
            _ => base.GenerateBllConstructor(domainType)
        };

    public override bool SquashPropertyValidators(PropertyInfo property)
    {
        return property != typeof(Employee).GetProperty(nameof(Employee.ExternalId));
    }

    public override bool GenerateDomainServiceConstructor(Type domainType)
    {
        return !new[] { typeof(Employee) }.Contains(domainType);
    }
}
