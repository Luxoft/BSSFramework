using System.CodeDom;

using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.DomainDriven.Generation.Domain;
using Framework.Security;
using Framework.SecuritySystem;
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

        var baseTypeRef = typeof(DefaultSecurityBLLFactory<,,>)
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
                               this.GetCreateBySecurityProviderMethod(),
                               this.GetCreateBySecurityOperationMethod(),
                               this.GetCreateBLLSecurityModeMethod()

                       },

                       BaseTypes = { baseTypeRef, interfaceBase }
               };
    }

    private CodeTypeMember GetCreateMethod()
    {
        var genericDomainTypeRef = new CodeTypeParameter("TDomainObject");

        var genericDomainTypeRefExpr = genericDomainTypeRef.ToTypeReference();

        var methodName = "Create";

        var contextRefExpr = new CodeThisReferenceExpression().ToPropertyReference((IBLLContextContainer<object> c) => c.Context);

        var resultType = typeof(IDefaultDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainTypeRefExpr, this.Configuration.Environment.GetIdentityType().ToTypeReference());

        var request = from domainType in this.Configuration.DomainTypes

                      let condition = new CodeValueEqualityOperatorExpression(genericDomainTypeRefExpr.ToTypeOfExpression(), domainType.ToTypeOfExpression())

                      let bllRef = contextRefExpr.ToPropertyReference((IBLLFactoryContainerContext<object> c) => c.Logics)
                                                 .ToPropertyReference(domainType.Name)

                      select Tuple.Create((CodeExpression)condition, (CodeStatement)bllRef.ToCastExpression(resultType).ToMethodReturnStatement());

        var lastSwitchElement = this.Configuration.Environment.BLLCore
                                    .DefaultOperationSecurityDomainBLLBaseTypeReference
                                    .ToTypeReference(genericDomainTypeRefExpr)
                                    .ToObjectCreateExpression(contextRefExpr)
                                    .ToMethodReturnStatement();

        return new CodeMemberMethod
               {
                       Name = methodName,
                       Attributes = MemberAttributes.Public | MemberAttributes.Override,
                       ReturnType = resultType,
                       Statements = { request.ToSwitchExpressionStatement(lastSwitchElement) },
                       TypeParameters = { genericDomainTypeRef }
               };
    }

    private CodeTypeMember GetCreateBySecurityProviderMethod()
    {
        var genericDomainTypeRef = new CodeTypeParameter("TDomainObject");

        var securityProvider = typeof(ISecurityProvider<>).ToTypeReference(genericDomainTypeRef.ToTypeReference());

        var securityProviderParameterName = "securityProvider";


        var genericDomainTypeRefExpr = genericDomainTypeRef.ToTypeReference();

        var methodName = "Create";

        var contextRefExpr = new CodeThisReferenceExpression().ToPropertyReference((IBLLContextContainer<object> c) => c.Context);

        var resultType = typeof(IDefaultSecurityDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainTypeRefExpr, this.Configuration.Environment.GetIdentityType().ToTypeReference());

        var request = from domainType in this.Configuration.DomainTypes

                      let condition = new CodeValueEqualityOperatorExpression(genericDomainTypeRefExpr.ToTypeOfExpression(), domainType.ToTypeOfExpression())

                      let bllRef = contextRefExpr.ToPropertyReference((IBLLFactoryContainerContext<object> c) => c.Logics)
                                                 .ToPropertyReference(domainType.Name + "Factory")

                      let bllMethod = bllRef.ToMethodInvokeExpression("Create", new CodeVariableReferenceExpression(securityProviderParameterName).ToCastExpression(typeof(ISecurityProvider<>).ToTypeReference(domainType.ToTypeReference())))

                      let isSecurity = domainType.IsSecurity()

                      let securityMethod = bllMethod

                      let withoutSecurityMethod = bllRef.ToMethodInvokeExpression("Create")

                      select Tuple.Create((CodeExpression)condition, (CodeStatement)((isSecurity ? securityMethod : withoutSecurityMethod).ToCastExpression(resultType).ToMethodReturnStatement()));

        var lastSwitchElement = this.Configuration.Environment.BLLCore
                                    .DefaultOperationSecurityDomainBLLBaseTypeReference
                                    .ToTypeReference(genericDomainTypeRefExpr)
                                    .ToObjectCreateExpression(contextRefExpr)
                                    .ToMethodReturnStatement();


        return new CodeMemberMethod
               {
                       Name = methodName,
                       Attributes = MemberAttributes.Public | MemberAttributes.Override,
                       ReturnType = resultType,
                       Statements = { request.ToSwitchExpressionStatement(lastSwitchElement) },
                       TypeParameters = { genericDomainTypeRef, },
                       Parameters = { new CodeParameterDeclarationExpression(securityProvider, securityProviderParameterName) }
               };
    }

    private CodeTypeMember GetCreateBySecurityOperationMethod()
    {
        var genericDomainTypeRef = new CodeTypeParameter("TDomainObject");

        var securityOperationModeParamName = "securityOperation";


        var genericDomainTypeRefExpr = genericDomainTypeRef.ToTypeReference();

        var methodName = "Create";

        var contextRefExpr = new CodeThisReferenceExpression().ToPropertyReference((IBLLContextContainer<object> c) => c.Context);

        var resultType = typeof(IDefaultSecurityDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainTypeRefExpr, this.Configuration.Environment.GetIdentityType().ToTypeReference());

        var request = from domainType in this.Configuration.DomainTypes

                      let condition = new CodeValueEqualityOperatorExpression(genericDomainTypeRefExpr.ToTypeOfExpression(), domainType.ToTypeOfExpression())

                      let bllRef = contextRefExpr.ToPropertyReference((IBLLFactoryContainerContext<object> c) => c.Logics)
                                                 .ToPropertyReference(domainType.Name + "Factory")

                      let bllMethod = bllRef.ToMethodInvokeExpression("Create", new CodeVariableReferenceExpression(securityOperationModeParamName))

                      let isSecurity = domainType.IsSecurity()

                      let securityMethod = bllMethod

                      let withoutSecurityMethod = bllRef.ToMethodInvokeExpression("Create")

                      select Tuple.Create((CodeExpression)condition, (CodeStatement)((isSecurity ? securityMethod : withoutSecurityMethod).ToCastExpression(resultType).ToMethodReturnStatement()));

        var lastSwitchElement = this.Configuration.Environment.BLLCore
                                    .DefaultOperationSecurityDomainBLLBaseTypeReference
                                    .ToTypeReference(genericDomainTypeRefExpr)
                                    .ToObjectCreateExpression(contextRefExpr)
                                    .ToMethodReturnStatement();


        return new CodeMemberMethod
               {
                       Name = methodName,
                       Attributes = MemberAttributes.Public | MemberAttributes.Override,
                       ReturnType = resultType,
                       Statements = { request.ToSwitchExpressionStatement(lastSwitchElement) },
                       TypeParameters = { genericDomainTypeRef, },
                       Parameters = { new CodeParameterDeclarationExpression(typeof(SecurityOperation), securityOperationModeParamName) }
               };
    }

    private CodeTypeMember GetCreateBLLSecurityModeMethod()
    {
        var genericDomainTypeRef = new CodeTypeParameter("TDomainObject");

        var bllSecurityModeTypeRef = typeof(BLLSecurityMode).ToTypeReference();

        var bllSecurityModeParamName = "bllSecurityMode";

        var genericDomainTypeRefExpr = genericDomainTypeRef.ToTypeReference();

        var methodName = "Create";

        var contextRefExpr = new CodeThisReferenceExpression().ToPropertyReference((IBLLContextContainer<object> c) => c.Context);

        var resultType = typeof(IDefaultSecurityDomainBLLBase<,,>).ToTypeReference(this.Configuration.Environment.PersistentDomainObjectBaseType.ToTypeReference(), genericDomainTypeRefExpr, this.Configuration.Environment.GetIdentityType().ToTypeReference());

        var request = from domainType in this.Configuration.DomainTypes

                      let condition = new CodeValueEqualityOperatorExpression(genericDomainTypeRefExpr.ToTypeOfExpression(), domainType.ToTypeOfExpression())

                      let bllRef = contextRefExpr.ToPropertyReference((IBLLFactoryContainerContext<object> c) => c.Logics)
                                                 .ToPropertyReference(domainType.Name + "Factory")

                      let bllMethod = bllRef.ToMethodInvokeExpression("Create", new CodeVariableReferenceExpression(bllSecurityModeParamName))

                      let isSecurity = domainType.IsSecurity()

                      let securityMethod = bllMethod

                      let withoutSecurityMethod = bllRef.ToMethodInvokeExpression("Create")

                      select Tuple.Create((CodeExpression)condition, (CodeStatement)((isSecurity ? securityMethod : withoutSecurityMethod).ToCastExpression(resultType).ToMethodReturnStatement()));

        var lastSwitchElement = this.Configuration.Environment.BLLCore
                                    .DefaultOperationSecurityDomainBLLBaseTypeReference
                                    .ToTypeReference(genericDomainTypeRefExpr)
                                    .ToObjectCreateExpression(contextRefExpr)
                                    .ToCastExpression(resultType)
                                    .ToMethodReturnStatement();


        return new CodeMemberMethod
               {
                       Name = methodName,
                       Attributes = MemberAttributes.Public | MemberAttributes.Override,
                       ReturnType = resultType,
                       Statements = { request.ToSwitchExpressionStatement(lastSwitchElement) },
                       TypeParameters = { genericDomainTypeRef, },
                       Parameters = { new CodeParameterDeclarationExpression(bllSecurityModeTypeRef, bllSecurityModeParamName) },

               };
    }

}
