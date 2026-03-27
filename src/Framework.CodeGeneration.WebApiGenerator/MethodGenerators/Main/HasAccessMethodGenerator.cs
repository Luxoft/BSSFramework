using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.BLL.Services;
using Framework.CodeDom;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Extensions;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;
using Framework.Core;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main;

public class HasAccessMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType)
    : MethodGenerator<TConfiguration, BLLViewRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.HasAccess;


    protected override string Name => $"Has{this.DomainType.Name}Access";

    protected override CodeTypeReference ReturnType { get; } = typeof(bool).ToTypeReference();

    protected override bool IsEdit { get; } = false;

    protected override bool RequiredSecurity { get; } = false;


    protected override string GetComment()
    {
        return $"Check access for {this.DomainType.Name}";
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

        yield return domainObjectVarDecl;

        yield return this.Configuration.Environment.BLLCore.GetSecurityService(evaluateDataExpr.GetContext())
                         .ToMethodInvokeExpression(
                             nameof(IRootSecurityService.HasAccess),
                             domainObjectVarDecl.ToVariableReferenceExpression(),
                             this.GetSecurityRuleParameter().ToVariableReferenceExpression())
                         .ToMethodReturnStatement();
    }
}
