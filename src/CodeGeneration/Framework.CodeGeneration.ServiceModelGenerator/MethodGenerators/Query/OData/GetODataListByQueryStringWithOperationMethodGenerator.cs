using System.CodeDom;

using CommonFramework;

using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.CodeDom.Extend;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.OData;
using Framework.OData.Typed;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData;

public class GetODataListByQueryStringWithOperationMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetODataListByQueryStringWithOperationMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
    {
        var elementTypeRef = this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);

        this.ReturnType = typeof(SelectOperationResult<>).ToTypeReference(elementTypeRef);

        this.Identity = new MethodIdentity(MethodIdentityType.GetODataListByQueryStringWithOperation, this.DTOType);
    }

    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(true, "Operation");

    protected sealed override CodeTypeReference ReturnType { get; }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr) => this.GetSecurityRuleParameter().ToVariableReferenceExpression();

    protected override string GetComment() => $"Get hierarchical data of type {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by operation";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression("odataQueryString");

        yield return this.GetSecurityRuleParameter();
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var selectMethod = typeof(SelectOperationResultExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression(nameof(SelectOperationResultExtensions.Select));

        var selectLambda = new CodeParameterDeclarationExpression { Name = this.DomainType.Name.ToStartLowerCase() }.Pipe(param => new CodeLambdaExpression
            {
                    Parameters = { param },
                    Statements = { param.ToVariableReferenceExpression().Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService())) }
            });

        var selectOperationExpr = evaluateDataExpr.GetContext().ToPropertyReference((IODataBLLContext c) => c.SelectOperationParser)
                                                  .ToMethodInvokeExpression((IParser<string, SelectOperation> parser) => parser.Parse(null), this.Parameter.ToVariableReferenceExpression());

        var selectOperationDecl = typeof(SelectOperation).ToTypeReference().ToVariableDeclarationStatement("selectOperation", selectOperationExpr);

        var expressionBuilderExpr = evaluateDataExpr.GetContext().ToPropertyReference("StandardExpressionBuilder");

        var typedSelectOperationDecl = typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToVariableDeclarationStatement(
         "typedSelectOperation",
         expressionBuilderExpr.ToStaticMethodInvokeExpression(
                                                              typeof(StandardExpressionBuilderExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("ToTyped", this.DomainType.ToTypeReference()),
                                                              selectOperationDecl.ToVariableReferenceExpression()));

        var treeDecl = new CodeVariableDeclarationStatement("var", "odataList", bllRefExpr.ToMethodReferenceExpression("GetObjectsByOData").ToMethodInvokeExpression(typedSelectOperationDecl.ToVariableReferenceExpression(), this.GetFetchRule()));

        yield return selectOperationDecl;

        yield return typedSelectOperationDecl;

        yield return treeDecl;

        yield return treeDecl.ToVariableReferenceExpression()
                             .ToStaticMethodInvokeExpression(selectMethod, selectLambda)
                             .ToMethodReturnStatement();
    }
}
