using System.CodeDom;

using Anch.SecuritySystem.Providers;

using Framework.BLL;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLGenerator.Configuration;
using Framework.FileGeneration.Configuration;

namespace Framework.CodeGeneration.BLLGenerator.FileFactory;

public class DefaultBLLFactoryFileFactory<TConfiguration>(TConfiguration configuration) : FileFactory<TConfiguration>(configuration, null)
    where TConfiguration : class, IBLLGeneratorConfiguration<IBLLGenerationEnvironment>
{
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


        var parameter = typeof(ISecurityProvider<>).ToTypeReference(genericDomainObjectParameterTypeRef).ToParameterDeclarationExpression("securityProvider");


        var implMethod = new CodeMemberMethod
        {
            Attributes = MemberAttributes.Public | MemberAttributes.Override,
            ReturnType = typeof(IDefaultSecurityDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainObjectParameterTypeRef, this.Configuration.Environment.GetIdentityType().ToTypeReference()),
            Name = "Create",
            Parameters = { parameter },
            TypeParameters = { genericDomainObjectParameter },
            Statements =
                                 {
                                         this.Configuration
                                             .SecurityDomainBLLBaseTypeReference
                                             .ToTypeReference(genericDomainObjectParameterTypeRef)
                                             .ToObjectCreateExpression(contextFieldRefExpr, parameter.ToVariableReferenceExpression())
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
                       },
            BaseTypes = { baseTypeRef, interfaceBase }
        };
    }
}

