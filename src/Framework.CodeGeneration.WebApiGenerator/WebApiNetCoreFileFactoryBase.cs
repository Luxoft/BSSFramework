using System.CodeDom;

using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.FileFactory._Base;

namespace Framework.CodeGeneration.WebApiGenerator;

public class WebApiNetCoreFileFactoryBase<TConfiguration>(TConfiguration configuration, Type domainType) : FileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public sealed override FileType FileType { get; } = FileType.Implement;

    protected sealed override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.DomainType!.Name,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsClass = true,
               };
    }

    protected sealed override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        var result = new CodeTypeReference(typeof(ApiControllerBase<,>))
                     {
                             TypeArguments =
                             {
                                     this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference,
                                     this.Configuration.Environment.ServerDTO.DTOMappingServiceInterfaceTypeReference
                             }
                     };

        yield return result;
    }
}
