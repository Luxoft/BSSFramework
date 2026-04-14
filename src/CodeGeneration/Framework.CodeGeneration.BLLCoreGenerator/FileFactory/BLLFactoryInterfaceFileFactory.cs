using System.CodeDom;

using Framework.BLL;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.Configuration;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

public class BLLFactoryInterfaceFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : FileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IBLLCoreGeneratorConfiguration<IBLLCoreGenerationEnvironment>
{
    public override FileType FileType => FileType.BLLFactoryInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration() =>
        new()
        {
            Name = this.Name,

            Attributes = MemberAttributes.Public,
            IsPartial = true,
            IsInterface = true
        };

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        var bllInterfaceTypeRef = this.Configuration.GetCodeTypeReference(this.DomainType, FileType.BLLInterface);

        yield return typeof(ISecurityBLLFactory<,>).ToTypeReference(bllInterfaceTypeRef, this.DomainType.ToTypeReference());
    }
}
