using System.CodeDom;

using Framework.BLL;
using Framework.CodeDom;
using Framework.CodeGeneration.BLLCoreGenerator.Configuration;
using Framework.CodeGeneration.BLLCoreGenerator.FileFactory.__Base;

namespace Framework.CodeGeneration.BLLCoreGenerator.FileFactory;

public class BLLFactoryInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLFactoryInterfaceFileFactory(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {

    }


    public override FileType FileType => FileType.BLLFactoryInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,

                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsInterface = true
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        var bllInterfaceTypeRef = this.Configuration.GetCodeTypeReference(this.DomainType, FileType.BLLInterface);

        yield return typeof(ISecurityBLLFactory<,>).ToTypeReference(bllInterfaceTypeRef, this.DomainType.ToTypeReference());
    }
}
