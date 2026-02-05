using System.CodeDom;
using System.Reflection;

using Framework.CodeDom;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLGenerator;

public class SecurityDomainBLLBaseFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override FileType FileType => FileType.SecurityDomainBLLBase;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter();
        var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

        var contextParameter = this.GetContextParameter();
        var contextParameterRefExpr = contextParameter.ToVariableReferenceExpression();

        var securityProviderParameter = this.GetSecurityProviderParameter(genericDomainObjectParameter);
        var securityProviderParameterRefExpr = securityProviderParameter.ToVariableReferenceExpression();

        return new CodeTypeDeclaration
               {
                       TypeParameters =
                       {
                               genericDomainObjectParameter
                       },

                       TypeAttributes = TypeAttributes.Public,
                       Name = this.Name,
                       IsClass = true,
                       IsPartial = true,
                       BaseTypes =
                       {
                               typeof(DefaultSecurityDomainBLLBase<,,,>).ToTypeReference(this.Configuration.Environment.BLLCore.BLLContextInterfaceTypeReference,
                                   this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                   genericDomainObjectParameterTypeRef,
                                   this.Configuration.Environment.GetIdentityType().ToTypeReference())
                       },
                       Members =
                       {
                               new CodeConstructor
                               {
                                       Attributes = MemberAttributes.Public,
                                       Parameters = { contextParameter, securityProviderParameter },
                                       BaseConstructorArgs = { contextParameterRefExpr, securityProviderParameterRefExpr }
                               }
                       }
               };
    }
}
