using System.CodeDom;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class RootSecurityServiceInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public RootSecurityServiceInterfaceFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.RootSecurityServiceInterface;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Name = this.Name,
                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       IsInterface = true,
               };
    }

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        yield return this.Configuration.RootSecurityServerGenerator.GetGenericRootSecurityServiceInterfaceType();
        yield return this.Configuration.GetCodeTypeReference(null, FileType.RootSecurityServicePathContainerInterface);
    }
}
