using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO.Extensions;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extend;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Remove.Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.Remove;

public class RemoveManyMethodGenerator<TConfiguration> : BaseRemoveMethodGenerator<TConfiguration>
        where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    private readonly RemoveMethodGenerator<TConfiguration> singleRemoveGenerator;

    public RemoveManyMethodGenerator(RemoveMethodGenerator<TConfiguration> singleRemoveGenerator)
            : base(singleRemoveGenerator.Configuration, singleRemoveGenerator.DomainType)
    {
        if (singleRemoveGenerator == null) throw new ArgumentNullException(nameof(singleRemoveGenerator));

        this.singleRemoveGenerator = singleRemoveGenerator;
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.RemoveMany;


    protected override string Name => "Remove" + this.DomainType.GetPluralizedDomainName();


    protected override string GetComment() => $"Remove {this.DomainType.GetPluralizedDomainName()}";

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
            yield return this.singleRemoveGenerator.GetFacadeMethodWithBLL(evaluateDataParameterExpr, bllParameterExpr);
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
                                new CodeThisReferenceExpression().ToMethodInvokeExpression(this.singleRemoveGenerator.InternalName, lambdaParam.ToVariableReferenceExpression(), evaluateDataExpr, bllRefExpr)
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
