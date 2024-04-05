using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.Security;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.BLLCoreGenerator;

public class SecurityOperationHelperFileFactory<TConfiguration> : FileFactory<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SecurityOperationHelperFileFactory(TConfiguration configuration)
            : base(configuration, null)
    {
    }

    public override FileType FileType => FileType.SecurityOperationHelper;

    protected override CodeTypeDeclaration GetCodeTypeDeclaration()
    {
        return new CodeTypeDeclaration
               {
                       Attributes = MemberAttributes.Public,
                       Name = this.Name
               }.MarkAsStatic();
    }

    protected override System.Collections.Generic.IEnumerable<CodeTypeMember> GetMembers()
    {
        yield return this.GetRegisterMethod();
    }

    private CodeMemberMethod GetRegisterMethod()
    {
        var serviceCollectionParameter = new CodeParameterDeclarationExpression(typeof(IServiceCollection), "services");

        var domainObjectSecurityOperationInfoRequest =

            from domainType in this.Configuration.DomainTypes

            let viewSecurityOperation = domainType.GetViewSecurityOperation()

            let editSecurityOperation = domainType.GetEditSecurityOperation()

            where viewSecurityOperation != null || editSecurityOperation != null

            select this.GetRegisterStatement(serviceCollectionParameter.ToVariableReferenceExpression(), domainType, viewSecurityOperation, editSecurityOperation);

        return new CodeMemberMethod
               {
                   Attributes = MemberAttributes.Public | MemberAttributes.Static,

                   Name = "RegisterDomainObjectSecurityOperations",

                   Parameters = { serviceCollectionParameter }
               }.Self(v => v.Statements.AddRange(domainObjectSecurityOperationInfoRequest.ToArray()));
    }
    private CodeExpressionStatement GetRegisterStatement(CodeExpression serviceCollectionExpr, Type domainType, SecurityOperation viewSecurityOperation, SecurityOperation editSecurityOperation)
    {
        var createExpr = typeof(DomainObjectSecurityModeInfo).ToTypeReference().ToObjectCreateExpression(
            domainType.ToTypeOfExpression(),
            viewSecurityOperation.Maybe(v => this.Configuration.GetSecurityCodeExpression(v)) ?? new CodePrimitiveExpression(),
            editSecurityOperation.Maybe(v => this.Configuration.GetSecurityCodeExpression(v)) ?? new CodePrimitiveExpression());

        var addSingletonMethod = typeof(ServiceCollectionServiceExtensions).ToTypeReferenceExpression()
                                                                        .ToMethodReferenceExpression(
                                                                            nameof(ServiceCollectionServiceExtensions.AddSingleton));

        return serviceCollectionExpr.ToStaticMethodInvokeExpression(addSingletonMethod, createExpr).ToExpressionStatement();
    }
}
