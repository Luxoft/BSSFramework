using System.CodeDom;

using Framework.CodeGeneration.ServiceModelGenerator;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.FileFactory;
using Framework.Infrastructure;

namespace Framework.CodeGeneration.WebApiGenerator;

public class WebApiNetCoreFileFactoryBase<TConfiguration>(TConfiguration configuration, Type domainType) : FileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public sealed override FileType FileType { get; } = FileType.Implement;

    protected sealed override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.DomainType!.Name,
            Attributes = MemberAttributes.Public,
            IsPartial = true,
            IsClass = true,
        };

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
