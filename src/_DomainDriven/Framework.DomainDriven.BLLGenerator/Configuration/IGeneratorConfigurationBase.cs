using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.DomainDriven.Generation.Domain;
using Framework.Transfering;

namespace Framework.DomainDriven.BLLGenerator;

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IGenerationEnvironmentBase
{
}

public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    Type OperationContextType { get; }

    bool UseDbUniquenessEvaluation { get; }

    ReadOnlyCollection<Type> ValidationTypes { get; }

    /// <summary>
    /// Добавление глобальных валидаторов для классов
    /// </summary>
    bool GenerateExternalClassValidators { get; }

    /// <summary>
    /// Добавление глобальных валидаторов для свойств
    /// </summary>
    bool GenerateExternalPropertyValidators { get; }


    /// <summary>
    /// Валидация виртуальных свойств (свойства, без одноимённого поля). По умолчанию включена только для свойств с хотя бы одним явно указаным атрибутом валидации "PropertyValidatorAttribute" или "IRestrictionAttribute"
    /// </summary>
    /// <param name="property">Cвойство</param>
    /// <returns></returns>
    bool AllowVirtualValidation(PropertyInfo property);

    CodeTypeReference GetSecurityDomainBLLBaseTypeReference(Type type);

    IValidatorGenerator GetValidatorGenerator(Type domainType, CodeExpression validatorMapExpr);

    bool GenerateValidation { get; }

    CodeTypeReference SecurityDomainBLLBaseTypeReference { get; }

    IFetchPathFactory<ViewDTOType> FetchPathFactory { get; }
    IBLLFactoryContainerGeneratorConfiguration Logics { get; }

    CodeTypeReference BLLContextTypeReference { get; }

    bool GenerateDTOFetchRuleExpander { get; }

    bool GenerateBllConstructor(Type domainType);

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
}
