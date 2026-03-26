using System.CodeDom;
using System.Collections.ObjectModel;

using Framework.CodeGeneration.BLLCoreGenerator.Configuration.BLLFactoryContainer;
using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, FileType>
    where TEnvironment : IGenerationEnvironmentBase;

#pragma warning disable S100 // Methods and properties should be named in camel case
public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    CodeTypeReference ActualRootSecurityServiceInterfaceType { get; }

    Type DefaultBLLFactoryContainerType { get; }

    Type SecurityBLLFactoryType { get; }

    Type SecurityBLLFactoryContainerType { get; }

    IBLLFactoryContainerInterfaceGeneratorConfiguration Logics { get; }

    ReadOnlyCollection<Type> BLLDomainTypes { get; }

    CodeTypeReference BLLContextInterfaceTypeReference { get; }

    CodeTypeReference BLLFactoryInterfaceTypeReference { get; }

    Type FilterModelType { get; }

    Type ContextFilterModelType { get; }

    Type ODataFilterModelType { get; }

    Type ODataContextFilterModelType { get; }

    Type CreateModelType { get; }

    Type FormatModelType { get; }

    /// <summary>
    /// Тип базовой модели для изменения единичного объекта
    /// </summary>
    Type ChangeModelType { get; }

    /// <summary>
    /// Тип базовой модели для изменения коллекции объектов
    /// </summary>
    Type MassChangeModelType { get; }

    Type ExtendedModelType { get; }

    Type IntegrationSaveModelType { get; }

    /// <summary>
    ///     Получает или возвращает флаг, указывающий на необходимость проверки уникальности путем запроса к БД.
    /// </summary>
    /// <value>
    ///     <c>true</c> если необходимо проверять уникальность путем запроса к БД; в противном случае, <c>false</c>.
    /// </value>
    bool UseDbUniquenessEvaluation { get; }

    string IntegrationSaveMethodName { get; }

    CodeExpression GetSecurityService(CodeExpression contextExpression);
}
#pragma warning restore S100 // Methods and properties should be named in camel case
