using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Security;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class GeneratorConfigurationBase<TEnvironment> : GeneratorConfiguration<TEnvironment, FileType>, IGeneratorConfigurationBase<TEnvironment>
        where TEnvironment : class, IGenerationEnvironmentBase
{
    protected GeneratorConfigurationBase(TEnvironment environment)
            : base(environment)
    {
        this.EvaluateDataTypeReference =
                typeof(EvaluatedData<,>).ToTypeReference(this.Environment.BLLCore.BLLContextInterfaceTypeReference,
                                                         this.Environment.ServerDTO.GetCodeTypeReference(null, DTOGenerator.Server.ServerFileType.ServerDTOMappingServiceInterface));
    }


    public virtual IGeneratePolicy<MethodIdentity> GeneratePolicy { get; } = DefaultServiceGeneratePolicy.Value;

    public abstract string ImplementClassName { get; }

    public virtual string ServiceContractNamespace { get; } = null;
        
    protected virtual ICodeFileFactoryHeader<FileType> ImplementServiceFileFactoryHeader { get; } =

        new CodeFileFactoryHeader<FileType>(FileType.Implement, @"Implement\", domainType => domainType.Name + "Service");


    public virtual CodeTypeReference EvaluateDataTypeReference { get; }


    protected override IEnumerable<ICodeFileFactoryHeader<FileType>> GetFileFactoryHeaders()
    {
        return new[]
               {
                       this.ImplementServiceFileFactoryHeader,
               };
    }

    protected override IEnumerable<Type> GetDomainTypes()
    {
        return this.Environment.BLL.DomainTypes;
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
