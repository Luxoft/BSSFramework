using System.CodeDom;

using Anch.Core;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Integration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Save.Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Remove;

public class IntegrationRemoveMethodGenerator<TConfiguration> : IntegrationMethodGenerator<TConfiguration, BLLIntegrationRemoveRoleAttribute>
        where TConfiguration : class, IIntegrationGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    private readonly CodeTypeReference identTypeRef;


    public IntegrationRemoveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType) =>
        this.identTypeRef = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

    public override MethodIdentity Identity { get; } = MethodIdentityType.IntegrationRemove;


    protected override string Name => "Remove" + this.DomainType.Name;

    protected override CodeTypeReference ReturnType { get; } = typeof(void).ToTypeReference();


    protected override string GetComment() => $"Remove {this.DomainType.Name}";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.identTypeRef.ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Ident");
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
