using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator.GeneratePolicy;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;
using Framework.Infrastructure.Service;

namespace Framework.CodeGeneration.ServiceModelGenerator.Configuration;

public abstract class ServiceModelGeneratorBase<TEnvironment> : CodeGeneratorConfiguration<TEnvironment, FileType>, IServiceModelGeneratorConfiguration<TEnvironment>
    where TEnvironment : class, IServiceModelGenerationEnvironment
{
    protected ServiceModelGeneratorBase(TEnvironment environment)
        : base(environment)
    {
        this.EvaluateDataTypeReference =
            typeof(EvaluatedData<,>).ToTypeReference(
                this.Environment.BLLCore.BLLContextInterfaceTypeReference,
                this.Environment.ServerDTO.GetCodeTypeReference(null, DTOGenerator.Server.ServerFileType.ServerDTOMappingServiceInterface));

        this.GeneratePolicy = new DefaultServiceGeneratePolicy(this.Environment);
    }


    public virtual IGeneratePolicy<MethodIdentity> GeneratePolicy { get; }

    public abstract string ImplementClassName { get; }

    protected virtual ICodeFileFactoryHeader<FileType> ImplementServiceFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.Implement, @"Implement\", domainType => domainType!.Name + "Service");


    public virtual CodeTypeReference EvaluateDataTypeReference { get; }

    public bool UseRouteAction { get; } = true;

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders() =>
    [
        this.ImplementServiceFileFactoryHeader
    ];

    protected override IEnumerable<Type> GetDomainTypes() => this.Environment.BLLCore.BLLDomainTypes;

    public abstract IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType);

    public bool HasMethods(Type domainType) => this.GetActualMethodGenerators(domainType).Any();

    public virtual IEnumerable<IServiceMethodGenerator> GetAccumulateMethodGenerators()
    {
        yield break;
    }
}
