using System.CodeDom;
using System.Reflection;
using Framework.CodeDom;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;

using nuSpec.Abstraction;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class SecurityDomainBLLBaseFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SecurityDomainBLLBaseFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.SecurityDomainBLLBase;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var genericDomainObjectParameter = this.GetDomainObjectCodeTypeParameter();
        var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

        var genericOperationParameter = this.GetOperationCodeTypeParameter();
        var genericOperationParameterTypeRef = genericOperationParameter.ToTypeReference();

        var contextParameter = this.GetContextParameter();
        var contextParameterRefExpr = contextParameter.ToVariableReferenceExpression();

        var securityProviderParameter = this.GetSecurityProviderParameter(genericDomainObjectParameter);
        var securityProviderParameterRefExpr = securityProviderParameter.ToVariableReferenceExpression();

        var specificationEvaluatorParameterTypeRef = typeof(ISpecificationEvaluator).ToTypeReference();
        var specificationEvaluatorParameter = specificationEvaluatorParameterTypeRef.ToParameterDeclarationExpression("specificationEvaluator = null");
        var specificationEvaluatorParameterArg = specificationEvaluatorParameterTypeRef.ToParameterDeclarationExpression("specificationEvaluator").ToVariableReferenceExpression();

        return new CodeTypeDeclaration
               {
                       TypeParameters =
                       {
                               genericDomainObjectParameter, genericOperationParameter
                       },

                       TypeAttributes = TypeAttributes.Abstract | TypeAttributes.Public,
                       Name = this.Name,
                       IsClass = true,
                       IsPartial = true,
                       BaseTypes =
                       {
                               typeof(DefaultSecurityDomainBLLBase<,,,,>).ToTypeReference(this.Configuration.BLLContextInterfaceTypeReference,
                                   this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                   genericDomainObjectParameterTypeRef,
                                   this.Configuration.Environment.GetIdentityType().ToTypeReference(),
                                   genericOperationParameterTypeRef)
                       },
                       Members =
                       {
                               new CodeConstructor
                               {
                                       Attributes = MemberAttributes.Family,
                                       Parameters = { contextParameter, specificationEvaluatorParameter },
                                       BaseConstructorArgs = { contextParameterRefExpr, specificationEvaluatorParameterArg }
                               },
                               new CodeConstructor
                               {
                                       Attributes = MemberAttributes.Family,
                                       Parameters = { contextParameter, securityProviderParameter, specificationEvaluatorParameter },
                                       BaseConstructorArgs = { contextParameterRefExpr, securityProviderParameterRefExpr, specificationEvaluatorParameterArg }
                               }
                       }
               };
    }
}
