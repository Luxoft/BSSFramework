using System.CodeDom;
using System.Collections.ObjectModel;
using System.Reflection;

using Framework.BLL.Domain.DTO;
using Framework.CodeGeneration.BLLGenerator.Fetching;
using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.BLLGenerator.Configuration;

public interface IBLLGeneratorConfiguration<out TEnvironment> : IBLLGeneratorConfiguration, ICodeGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IBLLGenerationEnvironment;

public interface IBLLGeneratorConfiguration : ICodeGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    Type OperationContextType { get; }

    bool UseDbUniquenessEvaluation { get; }

    ReadOnlyCollection<Type> ValidationTypes { get; }

    /// <summary>
    /// Add global validators for classes
    /// </summary>
    bool GenerateExternalClassValidators { get; }

    /// <summary>
    /// Add global validators for properties
    /// </summary>
    bool GenerateExternalPropertyValidators { get; }


    /// <summary>
    /// Validation of virtual properties (properties without a field of the same name). By default enabled only for properties with at least one explicitly specified validation attribute "PropertyValidatorAttribute" or "IRestrictionAttribute"
    /// </summary>
    /// <param name="property">Property</param>
    /// <returns></returns>
    bool AllowVirtualValidation(PropertyInfo property);

    CodeTypeReference GetSecurityDomainBLLBaseTypeReference(Type type);

    IValidatorGenerator GetValidatorGenerator(Type domainType, CodeExpression validatorMapExpr);

    CodeTypeReference SecurityDomainBLLBaseTypeReference { get; }

    IFetchPathFactory<ViewDTOType> FetchPathFactory { get; }

    IBLLFactoryContainerGeneratorConfiguration Logics { get; }

    CodeTypeReference BLLContextTypeReference { get; }

    bool GenerateDTOFetchRuleExpander { get; }

    bool GenerateBllConstructor(Type domainType);

    /// <summary>
    /// Collapsing an empty list of class validators
    /// </summary>
    /// <param name="domainType">Domain type</param>
    /// <returns></returns>
    bool SquashClassValidators(Type domainType);

    /// <summary>
    /// Collapsing an empty list of property validators
    /// </summary>
    /// <param name="property">Property</param>
    /// <returns></returns>
    bool SquashPropertyValidators(PropertyInfo property);
}
