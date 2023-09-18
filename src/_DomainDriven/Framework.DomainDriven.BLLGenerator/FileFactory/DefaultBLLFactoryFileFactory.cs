using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;

namespace Framework.DomainDriven.BLLGenerator;

public class DefaultBLLFactoryFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public DefaultBLLFactoryFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {

    }


    public override FileType FileType => FileType.DefaultBLLFactory;


    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var contextTypeRef = this.Configuration.BLLContextTypeReference;

        var contextParameter = contextTypeRef.ToParameterDeclarationExpression("context");
        var contextParameterRefExpr = contextParameter.ToVariableReferenceExpression();

        var contextFieldRefExpr = new CodeThisReferenceExpression().ToFieldReference("Context");

        var baseTypeRef = typeof(DefaultSecurityBLLFactory<,,>)
                .ToTypeReference(this.Configuration.BLLContextTypeReference,
                                 this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                 this.Configuration.Environment.GetIdentityType().ToTypeReference());



        var genericDomainObjectParameter = new CodeTypeParameter("TDomainObject");
        var genericDomainObjectParameterTypeRef = genericDomainObjectParameter.ToTypeReference();

        var interfaceBase = typeof(IDefaultSecurityBLLFactory<,>).
                ToTypeReference(
                                this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                this.Configuration.Environment.GetIdentityType().ToTypeReference());


        var implMethod = new CodeMemberMethod
                         {
                                 Attributes = MemberAttributes.Public | MemberAttributes.Override,
                                 ReturnType = typeof(IDefaultDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainObjectParameterTypeRef, this.Configuration.Environment.GetIdentityType().ToTypeReference()),
                                 Name = "Create",
                                 TypeParameters = { genericDomainObjectParameter },
                                 Statements =
                                 {
                                         this.Configuration.Environment.BLLCore
                                             .DefaultOperationDomainBLLBaseTypeReference
                                             .ToTypeReference(genericDomainObjectParameterTypeRef)
                                             .ToObjectCreateExpression(contextFieldRefExpr)
                                             .ToMethodReturnStatement()
                                 }
                         };


        return new CodeTypeDeclaration
               {
                       Name = this.Name,

                       Attributes = MemberAttributes.Public,
                       IsPartial = true,
                       Members =
                       {
                               new CodeConstructor
                               {
                                       Attributes = MemberAttributes.Public,
                                       Parameters = { contextParameter },
                                       BaseConstructorArgs = { contextParameterRefExpr }
                               },

                               implMethod
                               ////GetCreateBySecurityOperationMethod(),
                               ////GetCreateBySecurityProviderMethod(),
                               ////GetCreateBLLSecurityModeMethod()
                       },
                       BaseTypes = { baseTypeRef, interfaceBase }
               };
    }

    ////private CodeTypeMember GetCreateBySecurityProviderMethod()
    ////{
    ////    var genericDomainTypeRef = new CodeTypeParameter("TDomainObject");

    ////    var securityProvider = typeof(ISecurityProvider<>).ToTypeReference(genericDomainTypeRef.ToTypeReference());

    ////    var securityProviderParameterName = "securityProvider";


    ////    var genericDomainTypeRefExpr = genericDomainTypeRef.ToTypeReference();

    ////    var methodName = "Create";

    ////    var resultType = typeof(IDefaultDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainTypeRefExpr, this.Configuration.Environment.GetIdentityType().ToTypeReference());

    ////    var throwException = new CodeThrowExceptionStatement(new CodeObjectCreateExpression(new CodeTypeReference(typeof(System.NotImplementedException)), new CodeExpression[] { }));

    ////    return new CodeMemberMethod
    ////    {
    ////        Name = methodName,
    ////        Attributes = MemberAttributes.Public | MemberAttributes.Override,
    ////        ReturnType = resultType,
    ////        Statements = { throwException },
    ////        TypeParameters = { genericDomainTypeRef, },
    ////        Parameters = { new CodeParameterDeclarationExpression(securityProvider, securityProviderParameterName) }
    ////    };
    ////}


    ////private CodeTypeMember GetCreateBySecurityOperationMethod()
    ////{
    ////    var genericDomainTypeRef = new CodeTypeParameter("TDomainObject");

    ////    var securityOperationModeTypeRef = this.Configuration.Environment.SecurityOperationCodeType.ToTypeReference();

    ////    var securityOperationModeParamName = "securityOperation";


    ////    var genericDomainTypeRefExpr = genericDomainTypeRef.ToTypeReference();

    ////    var methodName = "Create";

    ////    var resultType = typeof(IDefaultDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainTypeRefExpr, this.Configuration.Environment.GetIdentityType().ToTypeReference());

    ////    var throwException = new CodeThrowExceptionStatement(new CodeObjectCreateExpression(new CodeTypeReference(typeof(System.NotImplementedException)), new CodeExpression[] { }));



    ////    return new CodeMemberMethod
    ////    {
    ////        Name = methodName,
    ////        Attributes = MemberAttributes.Public | MemberAttributes.Override,
    ////        ReturnType = resultType,
    ////        Statements = { throwException },
    ////        TypeParameters = { genericDomainTypeRef, },
    ////        Parameters = { new CodeParameterDeclarationExpression(securityOperationModeTypeRef, securityOperationModeParamName) }
    ////    };
    ////}

    ////private CodeTypeMember GetCreateBLLSecurityModeMethod()
    ////{
    ////    var genericDomainTypeRef = new CodeTypeParameter("TDomainObject");

    ////    var bllSecurityModeTypeRef = typeof(BLLSecurityMode).ToTypeReference();

    ////    var bllSecurityModeParamName = "bllSecurityMode";

    ////    var genericDomainTypeRefExpr = genericDomainTypeRef.ToTypeReference();

    ////    var methodName = "Create";

    ////    var resultType = typeof(IDefaultDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainTypeRefExpr, this.Configuration.Environment.GetIdentityType().ToTypeReference());

    ////    var throwException = new CodeThrowExceptionStatement(new CodeObjectCreateExpression(new CodeTypeReference(typeof(System.NotImplementedException)), new CodeExpression[] { }));
    ////    return new CodeMemberMethod
    ////    {
    ////        Name = methodName,
    ////        Attributes = MemberAttributes.Public,
    ////        ReturnType = resultType,
    ////        Statements = { throwException },
    ////        TypeParameters = { genericDomainTypeRef, },
    ////        Parameters = { new CodeParameterDeclarationExpression(bllSecurityModeTypeRef, bllSecurityModeParamName) }
    ////    };
    ////}

}
