using System.CodeDom;
using System.Reflection;

using Framework.CodeGeneration.Configuration;

namespace Framework.CodeGeneration.ProjectionGenerator.Configuration;

public interface IProjectionGeneratorConfiguration<out TEnvironment> : IProjectionGeneratorConfiguration, ICodeGeneratorConfiguration<TEnvironment, FileType>
    where TEnvironment : IProjectionGenerationEnvironment;

public interface IProjectionGeneratorConfiguration : ICodeGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    /// <summary>
    /// Generate a set accessor for IsOneToOne properties
    /// </summary>
    bool OneToOneSetter { get; }

    /// <summary>
    /// Generating public constructors for projections
    /// </summary>
    bool GeneratePublicCtors { get; }

    /// <summary>
    /// Getting generated attributes for a projection type
    /// </summary>
    /// <param name="domainType">Property</param>
    /// <returns></returns>
    IEnumerable<CodeAttributeDeclaration> GetDomainTypeAttributeDeclarations(Type domainType);

    /// <summary>
    /// Getting generated attributes for a projection property
    /// </summary>
    /// <param name="property">Property</param>
    /// <returns></returns>
    IEnumerable<CodeAttributeDeclaration> GetPropertyAttributeDeclarations(PropertyInfo property);
}
