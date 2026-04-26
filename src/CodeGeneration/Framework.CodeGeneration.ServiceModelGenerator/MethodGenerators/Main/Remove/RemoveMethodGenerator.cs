using System.CodeDom;

using Anch.Core;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Remove.Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Remove;

public class RemoveMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType) : BaseRemoveMethodGenerator<TConfiguration>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.Remove;


    protected override string Name => "Remove" + this.DomainType.Name;


    protected override string GetComment() => $"Remove {this.DomainType.Name}";

    protected override IEnumerable<CodeMemberMethod> GetFacadeMethods(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
    {
        foreach (var method in base.GetFacadeMethods(evaluateDataParameterExpr, bllParameterExpr))
        {
            yield return method;
        }

        yield return this.GetFacadeMethodWithBLL(evaluateDataParameterExpr, bllParameterExpr);
    }


    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.IdentTypeRef
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Ident");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return new CodeThisReferenceExpression().ToMethodInvokeExpression(this.InternalName, this.Parameters.Select(decl => decl.ToVariableReferenceExpression()).Concat(
                                                                                    [evaluateDataExpr, bllRefExpr]))
                                                      .ToExpressionStatement();
    }


    public CodeMemberMethod GetFacadeMethodWithBLL(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr) =>
        new CodeMemberMethod
            {
                Attributes = MemberAttributes.Family,
                Name = this.InternalName,
                ReturnType = this.ReturnType,
            }.WithParameters(this.Parameters.Concat([evaluateDataParameterExpr, bllParameterExpr]))
             .WithStatements(this.GetFacadeMethodWithBLLStatements(bllParameterExpr.ToVariableReferenceExpression()));

    private IEnumerable<CodeStatement> GetFacadeMethodWithBLLStatements(CodeExpression bllRefExpr)
    {
        var domainObjectVarDecl = this.ToDomainObjectVarDeclById(bllRefExpr);

        yield return domainObjectVarDecl;

        yield return bllRefExpr.ToMethodInvokeExpression("Remove", domainObjectVarDecl.ToVariableReferenceExpression()).ToExpressionStatement();
    }
}
