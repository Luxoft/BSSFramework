using System.CodeDom;

using Framework.BLL.ServiceModel.Service;
using Framework.CodeDom;
using Framework.CodeGeneration.Configuration;
using Framework.CodeGeneration.FileFactory;
using Framework.CodeGeneration.GeneratePolicy;
using Framework.CodeGeneration.WebApiGenerator.GeneratePolicy;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;

namespace Framework.CodeGeneration.WebApiGenerator.Configuration._Base;

public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    protected GeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this.EvaluateDataTypeReference =
                typeof(EvaluatedData<,>).ToTypeReference(this.Environment.BLLCore.BLLContextInterfaceTypeReference,
                                                         this.Environment.ServerDTO.GetCodeTypeReference(null, DTOGenerator.Server.ServerFileType.ServerDTOMappingServiceInterface));

        this.GeneratePolicy = new DefaultServiceGeneratePolicy(this.Environment);
    }


    public virtual IGeneratePolicy<MethodIdentity> GeneratePolicy { get; }

    public abstract string ImplementClassName { get; }

    protected virtual ICodeFileFactoryHeader<FileType> ImplementServiceFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.Implement, @"Implement\", domainType => domainType.Name + "Service");


    public virtual CodeTypeReference EvaluateDataTypeReference { get; }

    public bool UseRouteAction { get; } = true;

    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        return
        [
            this.ImplementServiceFileFactoryHeader
        ];
    }

    protected override IEnumerable<Type> GetDomainTypes()
    {
        return this.Environment.BLLCore.BLLDomainTypes;
    }


    public abstract IEnumerable<IServiceMethodGenerator> GetMethodGenerators(Type domainType);

    public bool HasMethods(Type domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return this.GetActualMethodGenerators(domainType).Any();
    }

    public virtual IEnumerable<IServiceMethodGenerator> GetAccumMethodGenerators()
    {
        yield break;
    }
}
