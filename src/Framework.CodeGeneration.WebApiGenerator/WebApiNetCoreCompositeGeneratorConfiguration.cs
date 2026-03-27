using System.CodeDom;

using CommonFramework;

using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.WebApiGenerator;

public class WebApiNetCoreCompositeGeneratorConfiguration : IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly IList<IGeneratorConfigurationBase<IGenerationEnvironmentBase>> source;

    public WebApiNetCoreCompositeGeneratorConfiguration(
            IGenerationEnvironmentBase environment,
            IList<IGeneratorConfigurationBase<IGenerationEnvironmentBase>> source,
            string? nameSpace = null)
    {
        this.Environment = environment;

        this.source = source;

        this.Namespace = nameSpace ?? $"{this.Environment.TargetSystemName}.WebApi.Controllers";

        this.DomainTypes = source.SelectMany(z => z.DomainTypes).Distinct().ToReadOnlyCollection();

        this.GeneratePolicy = Enumerable.All(source.Select(z => z.GeneratePolicy));

        this.EvaluateDataTypeReference = source.Select(z => z.EvaluateDataTypeReference).First();

        this.UseRouteAction = source.Select(v => v.UseRouteAction).Distinct().Single();
    }

    public virtual string Namespace { get; }

    public IReadOnlyCollection<Type> DomainTypes { get; }

    public IGeneratePolicy<MethodIdentity> GeneratePolicy { get; }

    public string? ImplementClassName { get; }

    public CodeTypeReference EvaluateDataTypeReference { get; }

    public bool UseRouteAction { get; }

    public IGenerationEnvironmentBase Environment { get; }

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

    public IEnumerable<IServiceMethodGenerator> GetAccumMethodGenerators() => throw new NotImplementedException();
}
