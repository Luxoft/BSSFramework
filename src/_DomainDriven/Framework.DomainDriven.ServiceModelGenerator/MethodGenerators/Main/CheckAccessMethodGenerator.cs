using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using SecuritySystem;
using Framework.Transfering;

using SecuritySystem.Providers;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class CheckAccessMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public CheckAccessMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.CheckAccess;


    protected override string Name => $"Check{this.DomainType.Name}Access";

    protected override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();

    protected override bool IsEdit { get; } = false;

    protected override bool RequiredSecurity { get; } = false;


    protected override string GetComment()
    {
        return $"Check {this.DomainType.Name} access";
    }


    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO
                         .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Ident");

        yield return this.GetSecurityRuleParameter();
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var domainObjectVarDecl = this.ToDomainObjectVarDeclById(bllRefExpr);
        var method = typeof(SecurityProviderBaseExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression(nameof(SecurityProviderBaseExtensions.CheckAccess));

        yield return domainObjectVarDecl;

        yield return this.Configuration.Environment.BLLCore.GetGetSecurityProviderMethodReferenceExpression(evaluateDataExpr.GetContext(), this.DomainType)
                         .ToMethodInvokeExpression(this.GetSecurityRuleParameter().ToVariableReferenceExpression())
                         .ToStaticMethodInvokeExpression(method, domainObjectVarDecl.ToVariableReferenceExpression(), evaluateDataExpr.GetContext().ToPropertyReference((IAccessDeniedExceptionServiceContainer c) => c.AccessDeniedExceptionService))
                         .ToExpressionStatement();
    }
}
