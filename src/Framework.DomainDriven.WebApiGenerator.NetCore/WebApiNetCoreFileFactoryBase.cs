using System.CodeDom;

using Framework.DomainDriven.ServiceModel.Service;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.DomainDriven.WebApiNetCore;

namespace Framework.DomainDriven.WebApiGenerator.NetCore;

public class WebApiNetCoreFileFactoryBase<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public WebApiNetCoreFileFactoryBase(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }

    public sealed override FileType FileType { get; } = FileType.Implement;

    protected sealed override IEnumerable<string> GetImportedNamespaces() => base.GetImportedNamespaces().Concat(new[] { this.Configuration.Environment.ServerDTO.Namespace });

    protected sealed override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.DomainType.Name,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsClass = true,
               };
    }

    protected sealed override IEnumerable<CodeTypeReference> GetBaseTypes()
    {

        var evaluateDataTypeReference = this.GetEvaluateDataTypeReference();

        var result = new CodeTypeReference(typeof(ApiControllerBase<,>))
                     {
                             TypeArguments =
                             {
                                     this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference,
                                     evaluateDataTypeReference
                             }
                     };

        yield return result;
    }

    private CodeTypeReference GetEvaluateDataTypeReference() => new CodeTypeReference(typeof(EvaluatedData<,>))
                                                                {
                                                                        TypeArguments =
                                                                        {
                                                                                this.Configuration.Environment.BLLCore
                                                                                    .BLLContextInterfaceTypeReference,
                                                                                this.Configuration.Environment.ServerDTO
                                                                                    .DTOMappingServiceInterfaceTypeReference
                                                                        }
                                                                };
}
