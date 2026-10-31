using System.CodeDom;
using System.Collections.ObjectModel;

using Anch.SecuritySystem;

using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.BLLCoreGenerator.Configuration;

public interface IBLLCoreGeneratorConfiguration<out TEnvironment> : IBLLCoreGeneratorConfiguration, ICodeGeneratorConfiguration<TEnvironment, FileType>
    where TEnvironment : IBLLCoreGenerationEnvironment;

#pragma warning disable S100 // Methods and properties should be named in camel case
public interface IBLLCoreGeneratorConfiguration : ICodeGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    Type DefaultBLLFactoryContainerType { get; }

    Type SecurityBLLFactoryType { get; }

    Type SecurityBLLFactoryContainerType { get; }

    IBLLFactoryContainerInterfaceGeneratorConfiguration Logics { get; }

    ReadOnlyCollection<Type> BLLDomainTypes { get; }

    CodeTypeReference BLLContextInterfaceTypeReference { get; }

    CodeTypeReference BLLFactoryInterfaceTypeReference { get; }

    Type? FilterModelType { get; }

    Type? ContextFilterModelType { get; }

    Type? ODataFilterModelType { get; }

    Type? ODataContextFilterModelType { get; }

    Type? CreateModelType { get; }

    Type? FormatModelType { get; }

    /// <summary>
    /// Type of the base model for changing a single object
    /// </summary>
    Type? ChangeModelType { get; }

    /// <summary>
    /// Type of the base model for changing a collection of objects
    /// </summary>
    Type? MassChangeModelType { get; }

    Type? ExtendedModelType { get; }

    Type? IntegrationSaveModelType { get; }

    bool GenerateValidation { get; }

    /// <summary>
    ///     Gets or sets a flag indicating that uniqueness must be checked by querying the database.
    /// </summary>
    /// <value>
    ///     <c>true</c> if uniqueness must be checked by querying the database; otherwise, <c>false</c>.
    /// </value>
    bool UseDbUniquenessEvaluation { get; }

    string IntegrationSaveMethodName { get; }

    CodeExpression GetSecurityService(CodeExpression contextExpression);

    CodeExpression GetSecurityCodeExpression(SecurityRule securityRule);
}
#pragma warning restore S100 // Methods and properties should be named in camel case
