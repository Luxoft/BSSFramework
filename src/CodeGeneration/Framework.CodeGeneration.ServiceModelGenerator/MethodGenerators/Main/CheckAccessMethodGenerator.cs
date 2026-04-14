using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.BLL.Services;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main;

public class CheckAccessMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType)
    : MethodGenerator<TConfiguration, BLLViewRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.CheckAccess;


    protected override string Name => $"Check{this.DomainType.Name}Access";

    protected override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();

    protected override bool IsEdit { get; } = false;

    protected override bool RequiredSecurity { get; } = false;


    protected override string GetComment() => $"Check {this.DomainType.Name} access";

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

        yield return domainObjectVarDecl;

        yield return this.Configuration.Environment.BLLCore.GetSecurityService(evaluateDataExpr.GetContext())
                         .ToMethodInvokeExpression(
                             nameof(IRootSecurityService.CheckAccess),
                             domainObjectVarDecl.ToVariableReferenceExpression(),
                             this.GetSecurityRuleParameter().ToVariableReferenceExpression())
                         .ToExpressionStatement();
    }
}
