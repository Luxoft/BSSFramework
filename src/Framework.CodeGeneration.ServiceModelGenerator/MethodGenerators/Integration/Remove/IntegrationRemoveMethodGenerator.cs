using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator.MethodGenerators.Integration.Remove;

public class IntegrationRemoveMethodGenerator<TConfiguration> : IntegrationMethodGenerator<TConfiguration, BLLIntegrationRemoveRoleAttribute>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly CodeTypeReference _identTypeRef;


    public IntegrationRemoveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
        this._identTypeRef = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.IntegrationRemove;


    protected override string Name => "Remove" + this.DomainType.Name;

    protected override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();


    protected override string GetComment()
    {
        return $"Remove {this.DomainType.Name}";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this._identTypeRef.ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Ident");
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        foreach (var facadeMethodInternalStatement in base.GetFacadeMethodInternalStatements(evaluateDataExpr, bllRefExpr))
        {
            yield return facadeMethodInternalStatement;
        }

        var domainObjectVarDecl = this.ToDomainObjectVarDeclById(bllRefExpr);

        yield return domainObjectVarDecl;

        yield return bllRefExpr.ToMethodInvokeExpression("Remove", domainObjectVarDecl.ToVariableReferenceExpression()).ToExpressionStatement();
    }
}
