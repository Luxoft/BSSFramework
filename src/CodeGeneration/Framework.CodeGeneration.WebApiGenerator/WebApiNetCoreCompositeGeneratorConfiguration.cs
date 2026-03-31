using System.CodeDom;
using System.Collections.Immutable;

using CommonFramework;

using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.WebApiGenerator;

public class WebApiNetCoreCompositeGeneratorConfiguration : IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    private readonly List<IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>> source;

    public WebApiNetCoreCompositeGeneratorConfiguration(
        IServiceModelGenerationEnvironment environment,
        List<IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>> source,
        string? nameSpace = null)
    {
        this.Environment = environment;

        this.source = source;

        this.Namespace = nameSpace ?? $"{this.Environment.TargetSystemName}.WebApi.Controllers";

        this.DomainTypes = [.. source.SelectMany(z => z.DomainTypes).Distinct()];

        this.GeneratePolicy = source.Select(z => z.GeneratePolicy).All();

        this.EvaluateDataTypeReference = source.Select(z => z.EvaluateDataTypeReference).First();

        this.UseRouteAction = source.Select(v => v.UseRouteAction).Distinct().Single();
    }

    public virtual string Namespace { get; }

    public ImmutableArray<Type> DomainTypes { get; }

    public IGeneratePolicy<MethodIdentity> GeneratePolicy { get; }

    public string? ImplementClassName { get; }

    public CodeTypeReference EvaluateDataTypeReference { get; }

    public bool UseRouteAction { get; }

    public IServiceModelGenerationEnvironment Environment { get; }

    public string GetTypeName(Type domainType, FileType fileType) =>
            this.source.Select(z => z.GetTypeName(domainType, fileType)).FirstOrDefault(z => !string.IsNullOrWhiteSpace(z));

    public CodeTypeReference GetCodeTypeReference(Type domainType, FileType fileType) =>
            this.source.Select(z => z.GetCodeTypeReference(domainType, fileType)).FirstOrDefault();

    public ICodeFileFactoryHeader GetFileFactoryHeader(FileType fileType, bool raiseIfNotFound = true) =>
            this.source.Select(z => z.GetFileFactoryHeader(fileType, false)).FirstOrDefault();

    public IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType) =>
            this.source.SelectMany(z => z.GetMethodGenerators(domainType));

    public bool HasMethods(Type domainType) =>
            this.source.Any(z => z.HasMethods(domainType));

    public IEnumerable<IServiceMethodGenerator> GetAccumulateMethodGenerators() => throw new NotImplementedException();
}
