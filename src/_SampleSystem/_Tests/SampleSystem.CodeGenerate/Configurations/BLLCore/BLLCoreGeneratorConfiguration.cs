using System.Reflection;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

public class BLLCoreGeneratorConfiguration(ServerGenerationEnvironment environment)
    : Framework.DomainDriven.BLLCoreGenerator.GeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override Type FilterModelType { get; } = typeof(DomainObjectFilterModel<>);

    public override Type ODataFilterModelType { get; } = typeof(DomainObjectODataFilterModel<>);

    public override Type ODataContextFilterModelType { get; } = typeof(DomainObjectODataContextFilterModel<>);

    public override Type ContextFilterModelType { get; } = typeof(DomainObjectContextFilterModel<>);

    public override Type CreateModelType { get; } = typeof(DomainObjectCreateModel<>);

    public override Type FormatModelType { get; } = typeof(DomainObjectFormatModel<>);

    /// <inheritdoc />
    public override Type ChangeModelType { get; } = typeof(DomainObjectChangeModel<>);

    /// <inheritdoc />
    public override Type MassChangeModelType { get; } = typeof(DomainObjectMassChangeModel<>);

    /// <summary>
    /// Тип базовой произвольной модели для изменений объектов (пример, для которого будет расширена генерация)
    /// </summary>
    public Type ComplexChangeModelType { get; } = typeof(DomainObjectComplexChangeModel<>);


    public override Type IntegrationSaveModelType { get; } = typeof(DomainObjectIntegrationSaveModel<>);

    /// <inheritdoc />
    public override bool UseDbUniquenessEvaluation { get; } = false;
}
