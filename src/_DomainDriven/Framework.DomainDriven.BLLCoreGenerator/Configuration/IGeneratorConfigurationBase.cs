using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;
using Framework.Transfering;

namespace Framework.DomainDriven.BLLCoreGenerator;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IGenerationEnvironmentBase
{
}

#pragma warning disable S100 // Methods and properties should be named in camel case
public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    CodeTypeReference ActualRootSecurityServiceInterfaceType { get; }

    bool GenerateAuthServices { get; }

    /// <summary>
    /// Добавление глобальных валидаторов для классов
    /// </summary>
    bool GenerateExternalClassValidators { get; }

    /// <summary>
    /// Добавление глобальных валидаторов для свойств
    /// </summary>
    bool GenerateExternalPropertyValidators { get; }

    ReadOnlyCollection<Type> ValidationTypes { get; }

    Type DefaultBLLFactoryContainerType { get; }

    Type SecurityBLLFactoryType { get; }

    Type SecurityBLLFactoryContainerType { get; }

    IBLLFactoryContainerInterfaceGeneratorConfiguration Logics { get; }

    bool UseRemoveMappingExtension { get; }

    bool GenerateValidationMap { get; }

    bool GenerateValidator { get; }

    bool GenerateFetchService { get; }

    ReadOnlyCollection<Type> BLLDomainTypes { get; }

    ReadOnlyCollection<Type> SecurityServiceDomainTypes { get; }

    string GetOperationByModeMethodName { get; }

    CodeTypeReference BLLContextInterfaceTypeReference { get; }

    CodeTypeReference BLLFactoryInterfaceTypeReference { get; }

    CodeTypeReference DomainBLLBaseTypeReference { get; }

    CodeTypeReference SecurityDomainBLLBaseTypeReference { get; }

    CodeTypeReference DefaultOperationDomainBLLBaseTypeReference { get; }

    CodeTypeReference DefaultOperationSecurityDomainBLLBaseTypeReference { get; }

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

    IRootSecurityServiceGenerator RootSecurityServerGenerator { get; }

    IFetchPathFactory<FetchBuildRule.DTOFetchBuildRule> FetchPathFactory { get; }

    /// <summary>
    ///     Получает или возвращает флаг, указывающий на необходимость проверки уникальности путем запроса к БД.
    /// </summary>
    /// <value>
    ///     <c>true</c> если необходимо проверять уникальность путем запроса к БД; в противном случае, <c>false</c>.
    /// </value>
    bool UseDbUniquenessEvaluation { get; }

    string IntegrationSaveMethodName { get; }

    CodeTypeReference GetSecurityDomainBLLBaseTypeReference(Type type);

    CodeTypeReference GetSecurityHierarchyDomainBLLBaseTypeReference(Type type);

    IValidatorGenerator GetValidatorGenerator(Type domainType, CodeExpression validatorMapExpr);

    CodeExpression GetCreateDefaultBLLExpression(CodeExpression contextExpression, CodeTypeReference genericType);

    Type GetBLLSecurityModeType(Type domainType);

    IEnumerable<PropertyInfo> GetMappingProperties(Type domainType, MainDTOType fileType);

    bool HasSecurityContext(Type domainType);

    CodeMethodReferenceExpression GetGetSecurityProviderMethodReferenceExpression(CodeExpression contextExpression, Type domainType);

    /// <summary>
    /// Получение списка Generic-параметров для безопастности доменного объектка
    /// </summary>
    /// <param name="domainType"></param>
    /// <returns></returns>
    IEnumerable<CodeTypeParameter> GetDomainTypeSecurityParameters(Type domainType);

    /// <summary>
    /// Валидация виртуальных свойств (свойства, без одноимённого поля). По умолчанию включена только для свойств с хотя бы одним явно указаным атрибутом валидации "PropertyValidatorAttribute" или "IRestrictionAttribute"
    /// </summary>
    /// <param name="property">Cвойство</param>
    /// <returns></returns>
    bool AllowVirtualValidation(PropertyInfo property);

    /// <summary>
    /// Схлопывание пустого списка валидаторов класса
    /// </summary>
    /// <param name="domainType">Доменный тип</param>
    /// <returns></returns>
    bool SquashClassValidators(Type domainType);

    /// <summary>
    /// Схлопывание пустого списка валидаторов свойства
    /// </summary>
    /// <param name="property">Свойство</param>
    /// <returns></returns>
    bool SquashPropertyValidators(PropertyInfo property);

    bool GenerateDomainServiceConstructor(Type domainType);
}
#pragma warning restore S100 // Methods and properties should be named in camel case
