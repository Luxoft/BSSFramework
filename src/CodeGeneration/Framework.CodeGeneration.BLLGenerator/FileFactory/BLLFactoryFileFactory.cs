using System.CodeDom;
using Framework.BLL;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.CodeGeneration.BLLGenerator.FileFactory.__Base;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class BLLFactoryFileFactory<TConfiguration>(TConfiguration configuration, Type domainType) : FileFactory<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
{
    public override FileType FileType => FileType.BLLFactory;


    public CodeTypeReference BLLInterfaceRef => this.Configuration.Environment.BLLCore.GetCodeTypeReference(this.DomainType, BLLCoreGenerator.FileType.BLLInterface);

    public CodeTypeReference BLLRef => this.Configuration.GetCodeTypeReference(this.DomainType, FileType.BLL);

    public CodeTypeReference BLLFactoryInterfaceRef => this.Configuration.Environment.BLLCore.GetCodeTypeReference(this.DomainType, BLLCoreGenerator.FileType.BLLFactoryInterface);


    private CodeTypeReference GetBaseReference()
    {
        return typeof(SecurityBLLFactory<,,,>).ToTypeReference(this.Configuration.BLLContextTypeReference, this.BLLInterfaceRef, this.BLLRef, this.DomainType.ToTypeReference());
    }

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return this.Configuration.GetBLLContextContainerCodeTypeDeclaration(this.Name, false, this.GetBaseReference());
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return this.BLLFactoryInterfaceRef;
    }
}
