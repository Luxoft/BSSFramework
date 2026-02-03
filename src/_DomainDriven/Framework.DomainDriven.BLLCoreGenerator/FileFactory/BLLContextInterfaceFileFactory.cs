using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class BLLContextInterfaceFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
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

    protected override IEnumerable<CodeTypeReference> GetBaseTypes()
    {
        var securityServiceFieldTypeRef = this.Configuration.ActualRootSecurityServiceInterfaceType;

        yield return new CodeTypeReference(typeof(IAccessDeniedExceptionServiceContainer));

        yield return new CodeTypeReference(typeof(ISecurityServiceContainer<>)) { TypeArguments = { securityServiceFieldTypeRef } };
        yield return typeof(IBLLFactoryContainerContext<>).ToTypeReference(this.Configuration.GetCodeTypeReference(null, FileType.BLLFactoryContainerInterface));
    }

    protected override IEnumerable<CodeTypeMember> GetMembers()
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
