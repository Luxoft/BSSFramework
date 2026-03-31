using System.Reflection;

using Framework.CodeGeneration.BLLGenerator.Configuration;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

public class BLLGeneratorConfiguration(ServerGenerationEnvironment environment) : BLLGeneratorConfigurationBase<
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

    public override bool SquashPropertyValidators(PropertyInfo property) => property != typeof(Employee).GetProperty(nameof(Employee.ExternalId));

    public override bool GenerateDomainServiceConstructor(Type domainType) => !new[] { typeof(Employee) }.Contains(domainType);
}
