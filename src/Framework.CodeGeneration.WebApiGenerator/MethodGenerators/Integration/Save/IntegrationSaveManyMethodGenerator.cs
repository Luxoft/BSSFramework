using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO.Extensions;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom;
using Framework.CodeDom.Extend;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Configuration.Integration;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Integration.Save.ByModel._Base;
using Framework.Core;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Integration.Save;

public class IntegrationSaveManyMethodGenerator<TConfiguration> : IntegrationBaseSaveMethodGenerator<TConfiguration>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly IntegrationSaveMethodGenerator<TConfiguration> singleSaveGenerator;

    public IntegrationSaveManyMethodGenerator(IntegrationSaveMethodGenerator<TConfiguration> singleSaveGenerator)
            : base(singleSaveGenerator.Configuration, singleSaveGenerator.DomainType)
    {
        if (singleSaveGenerator == null) throw new ArgumentNullException(nameof(singleSaveGenerator));

        this.singleSaveGenerator = singleSaveGenerator;
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.IntegrationSaveMany;

    protected override string Name => "Save" + this.DomainType.GetPluralizedDomainName();

    protected override CodeTypeReference ReturnType => this.IdentTypeRef.ToEnumerableReference();


    protected override string GetComment()
    {
        return $"Save {this.DomainType.GetPluralizedDomainName()}";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.RichIntegrationTypeRef
                         .ToArrayReference()
                         .ToParameterDeclarationExpression(this.DomainType.GetPluralizedDomainName().ToStartLowerCase());
    }

    protected override IEnumerable<CodeMemberMethod> GetFacadeMethods(CodeParameterDeclarationExpression evaluateDataParameterExpr, CodeParameterDeclarationExpression bllParameterExpr)
    {
        foreach (var method in base.GetFacadeMethods(evaluateDataParameterExpr, bllParameterExpr))
        {
            yield return method;
        }

        if (!this.Attribute.CountType.HasFlag(CountType.Single))
        {
            yield return this.singleSaveGenerator.GetFacadeMethodWithBLL(evaluateDataParameterExpr, bllParameterExpr);
        }
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));

        foreach (var baseStatement in base.GetFacadeMethodInternalStatements(evaluateDataExpr, bllRefExpr))
        {
            yield return baseStatement;
        }

        var convertLambda = new CodeParameterDeclarationExpression { Name = this.DomainType.Name.ToStartLowerCase() }.Pipe(lambdaParam =>

                new CodeLambdaExpression
                {
                        Parameters =
                        {
                                lambdaParam
                        },
                        Statements =
                        {
                                new CodeThisReferenceExpression().ToMethodInvokeExpression(this.singleSaveGenerator.InternalName, lambdaParam.ToVariableReferenceExpression(), evaluateDataExpr, bllRefExpr )
                        }
                });

        yield return this.Parameter
                         .ToVariableReferenceExpression()
                         .ToStaticMethodInvokeExpression(

                                                         typeof(CoreEnumerableExtensions)
                                                                 .ToTypeReferenceExpression()
                                                                 .ToMethodReferenceExpression("ToList"), convertLambda)

                         .ToMethodReturnStatement();
    }
}
