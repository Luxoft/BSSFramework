using System.Reflection;

using Framework.CodeGeneration.BLLGenerator.Configuration;

using SampleSystem.Domain._Validation._Operation;
using SampleSystem.Domain.Directories;
using SampleSystem.Domain.Employee;

namespace SampleSystem.CodeGenerate.Configurations.BLL;

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
}
