using System.CodeDom;

using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.ServiceModelGenerator;

public interface IGeneratorConfigurationBase : IGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    IGeneratePolicy<MethodIdentity> GeneratePolicy { get; }

    string ImplementClassName { get; }

    IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType);

    bool HasMethods(Type domainType);

    IEnumerable<IServiceMethodGenerator> GetAccumMethodGenerators();

    CodeTypeReference EvaluateDataTypeReference { get; }

    bool UseRouteAction { get; }
}

public interface IGeneratorConfigurationBase<out TEnvironment> : IGeneratorConfigurationBase, IGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IGenerationEnvironmentBase
{
}
