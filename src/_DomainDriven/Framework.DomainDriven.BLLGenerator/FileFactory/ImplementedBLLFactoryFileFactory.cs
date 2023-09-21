using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;
using Framework.Security;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLLGenerator;

public class ImplementedBLLFactoryFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ImplementedBLLFactoryFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }

    public override FileType FileType => FileType.ImplementedBLLFactory;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        var contextTypeRef = this.Configuration.BLLContextTypeReference;

        var contextParameter = contextTypeRef.ToParameterDeclarationExpression("context");
        var contextParameterRefExpr = contextParameter.ToVariableReferenceExpression();

        var baseTypeRef = typeof(ImplementedSecurityBLLFactory<,,>)
                .ToTypeReference(this.Configuration.BLLContextTypeReference,
                                 this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                 this.Configuration.Environment.GetIdentityType().ToTypeReference());

        var interfaceBase = typeof(IDefaultSecurityBLLFactory<,>).
                ToTypeReference(
                                this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(),
                                this.Configuration.Environment.GetIdentityType().ToTypeReference());

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

                               this.GetCreateMethod(),
                       },

                       BaseTypes = { baseTypeRef, interfaceBase }
               };
    }

    private CodeTypeMember GetCreateMethod()
    {
        var genericDomainTypeRef = new CodeTypeParameter("TDomainObject");

        var genericDomainTypeRefExpr = genericDomainTypeRef.ToTypeReference();

        var methodName = "CreateDefault";

        var resultType = typeof(IDefaultSecurityDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainTypeRefExpr, this.Configuration.Environment.GetIdentityType().ToTypeReference());

        var contextFieldRefExpr = new CodeThisReferenceExpression().ToFieldReference("Context");

        var parameter = typeof(ISecurityProvider<>).ToTypeReference(genericDomainTypeRefExpr).ToParameterDeclarationExpression("securityProvider");

        return new CodeMemberMethod
               {
                       Name = methodName,
                       Attributes = MemberAttributes.Family | MemberAttributes.Override,
                       ReturnType = resultType,
                       Parameters = { parameter },
                       Statements =
                       {
                           this.Configuration.Environment.BLLCore.DefaultOperationSecurityDomainBLLBaseTypeReference.ToTypeReference(genericDomainTypeRefExpr).ToObjectCreateExpression(contextFieldRefExpr, parameter.ToVariableReferenceExpression()).ToMethodReturnStatement()
                       },
                       TypeParameters = { genericDomainTypeRef }
               };
    }
}
