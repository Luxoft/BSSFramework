using System.CodeDom;

using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

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
