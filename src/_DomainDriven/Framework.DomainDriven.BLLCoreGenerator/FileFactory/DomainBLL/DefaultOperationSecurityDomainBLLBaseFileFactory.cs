using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class DefaultOperationSecurityDomainBLLBaseFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public DefaultOperationSecurityDomainBLLBaseFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.DefaultOperationSecurityDomainBLLBase;


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

                       Name = this.Name,

                       IsClass = true,

                       IsPartial = true,

                       BaseTypes =
                       {
                               new CodeTypeReference()
                               {
                                       BaseType = this.Configuration.GetCodeTypeReference(null, FileType.SecurityDomainBLLBase).BaseType,
                                       TypeArguments = { genericDomainObjectParameterTypeRef, typeof(BLLBaseOperation) }
                               }
                       },
                       Members =
                       {
                               new CodeConstructor
                               {
                                       Attributes = MemberAttributes.Public,
                                       Parameters = { contextParameter },
                                       BaseConstructorArgs = { contextParameterRefExpr }
                               },

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
