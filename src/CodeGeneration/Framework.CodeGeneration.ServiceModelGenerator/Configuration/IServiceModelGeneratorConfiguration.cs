using System.CodeDom;

using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration;

public interface IServiceModelGeneratorConfiguration : ICodeGeneratorConfiguration, ICodeTypeReferenceService<FileType>
{
    IGeneratePolicy<MethodIdentity> GeneratePolicy { get; }

    string ImplementClassName { get; }

    IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType);

    bool HasMethods(Type domainType);

    IEnumerable<IServiceMethodGenerator> GetAccumulateMethodGenerators();

    CodeTypeReference EvaluateDataTypeReference { get; }

    bool UseRouteAction { get; }
}

public interface IServiceModelGeneratorConfiguration<out TEnvironment> : IServiceModelGeneratorConfiguration, ICodeGeneratorConfiguration<TEnvironment, FileType>
        where TEnvironment : IServiceModelGenerationEnvironment;
