using Framework.CodeGeneration.BLLCoreGenerator.Configuration;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate.Configurations.BLLCore;

public class BLLCoreGeneratorConfiguration(ServerGenerationEnvironment environment)
    : BLLCoreGeneratorConfigurationBase<ServerGenerationEnvironment>(environment)
{
    public override Type FilterModelType { get; } = typeof(DomainObjectFilterModel<>);

    public override Type ContextFilterModelType { get; } = typeof(DomainObjectContextFilterModel<>);

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
