using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class RemoveMethodGenerator<TConfiguration> : BaseRemoveMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public RemoveMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.Remove;


    protected override string Name => "Remove" + this.DomainType.Name;


    protected override string GetComment()
    {
        return $"Remove {this.DomainType.Name}";
    }

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
        yield return new CodeThisReferenceExpression().ToMethodInvokeExpression(this.InternalName, this.Parameters.Select(decl => decl.ToVariableReferenceExpression()).Concat(new[] { evaluateDataExpr, bllRefExpr }))
                                                      .ToExpressionStatement();
    }


    public CodeMemberMethod GetFacadeMethodWithBLL(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
    {
        return new CodeMemberMethod
               {
                       Attributes = MemberAttributes.Family,
                       Name = this.InternalName,
                       ReturnType = this.ReturnType,
               }.WithParameters(this.Parameters.Concat(new[] { evaluateDataParameterExpr, bllParameterExpr }))
                .WithStatements(this.GetFacadeMethodWithBLLStatements(bllParameterExpr.ToVariableReferenceExpression()));
    }

    private IEnumerable<CodeStatement> GetFacadeMethodWithBLLStatements(CodeExpression bllRefExpr)
    {
        var domainObjectVarDecl = this.ToDomainObjectVarDeclById(bllRefExpr);

        yield return domainObjectVarDecl;

        yield return bllRefExpr.ToMethodInvokeExpression("Remove", domainObjectVarDecl.ToVariableReferenceExpression()).ToExpressionStatement();
    }
}
