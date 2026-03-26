using System.CodeDom;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class RemoveManyMethodGenerator<TConfiguration> : BaseRemoveMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly RemoveMethodGenerator<TConfiguration> _singleRemoveGenerator;

    public RemoveManyMethodGenerator(RemoveMethodGenerator<TConfiguration> singleRemoveGenerator)
            : base(singleRemoveGenerator.Configuration, singleRemoveGenerator.DomainType)
    {
        if (singleRemoveGenerator == null) throw new ArgumentNullException(nameof(singleRemoveGenerator));

        this._singleRemoveGenerator = singleRemoveGenerator;
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.RemoveMany;


    protected override string Name => "Remove" + this.DomainType.GetPluralizedDomainName();


    protected override string GetComment()
    {
        return $"Remove {this.DomainType.GetPluralizedDomainName()}";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.IdentTypeRef
                         .ToArrayReference()
                         .ToParameterDeclarationExpression("idents");
    }

    protected override IEnumerable<CodeMemberMethod> GetFacadeMethods(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
    {
        foreach (var method in base.GetFacadeMethods(evaluateDataParameterExpr, bllParameterExpr))
        {
            yield return method;
        }

        if (!this.Attribute.CountType.HasFlag(CountType.Single))
        {
            yield return this._singleRemoveGenerator.GetFacadeMethodWithBLL(evaluateDataParameterExpr, bllParameterExpr);
        }
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var convertLambda = new CodeParameterDeclarationExpression { Name = this.DomainType.Name.ToStartLowerCase() }.Pipe(lambdaParam =>

                new CodeLambdaExpression
                {
                        Parameters =
                        {
                                lambdaParam
                        },
                        Statements =
                        {
                                new CodeThisReferenceExpression().ToMethodInvokeExpression(this._singleRemoveGenerator.InternalName, lambdaParam.ToVariableReferenceExpression(), evaluateDataExpr, bllRefExpr)
                        }
                });

        yield return this.Parameter
                         .ToVariableReferenceExpression()
                         .ToStaticMethodInvokeExpression(

                                                         typeof(CommonFramework.EnumerableExtensions)
                                                                 .ToTypeReferenceExpression()
                                                                 .ToMethodReferenceExpression("Foreach"), convertLambda)

                         .ToExpressionStatement();
    }
}
