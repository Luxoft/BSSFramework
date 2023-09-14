using System.CodeDom;
using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class BLLContextInterfaceFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public BLLContextInterfaceFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.BLLContextInterface;


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

    protected override System.Collections.Generic.IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        var securityServiceFieldTypeRef = this.Configuration.GetCodeTypeReference(null, FileType.RootSecurityServiceInterface);

        yield return new CodeTypeReference(typeof(IAccessDeniedExceptionServiceContainer));

        yield return new CodeTypeReference(typeof(ISecurityServiceContainer<>)) { TypeArguments = { securityServiceFieldTypeRef } };
        yield return typeof(IBLLFactoryContainerContext<>).ToTypeReference(this.Configuration.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface));

        yield return new CodeTypeReference(typeof(IFetchServiceContainer<,>)) { TypeArguments = { this.Configuration.Environment.PersistentDomainObjectBaseType, typeof(FetchBuildRule).ToTypeReference() } };


        foreach (var baseType in this.Configuration.RootSecurityServerGenerator.GetBLLContextBaseTypes())
        {
            yield return baseType;
        }

    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        yield return new CodeMemberProperty
                     {
                             Name = "Logics",
                             Attributes = MemberAttributes.New,
                             Type = this.Configuration.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface),
                             HasGet = true
                     };
    }
}
