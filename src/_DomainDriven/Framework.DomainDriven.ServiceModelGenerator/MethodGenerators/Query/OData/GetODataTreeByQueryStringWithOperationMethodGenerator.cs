using System.CodeDom;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;
using Framework.OData;
using Framework.Persistent;
using SecuritySystem;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class GetODataTreeByQueryStringWithOperationMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetODataTreeByQueryStringWithOperationMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
    {
        var elementTypeRef = typeof(HierarchicalNode<,>).ToTypeReference(
                                                                         this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType),
                                                                         this.Configuration.Environment.GetIdentityType().ToTypeReference());

        this.ReturnType = typeof(SelectOperationResult<>).ToTypeReference(elementTypeRef);

        this.Identity = new MethodIdentity(MethodIdentityType.GetODataTreeByQueryStringWithOperation, this.DTOType);
    }

    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(false, "Operation", "TreeBy");

    protected sealed override CodeTypeReference ReturnType { get; }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        return this.GetSecurityRuleParameter().ToVariableReferenceExpression();
    }

    protected override string GetComment()
    {
        return $"Get hierarchical data of type {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by operation";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression("odataQueryString");

        yield return new CodeParameterDeclarationExpression
                     {
                             Name = "securityRule",
                             Type = typeof(DomainSecurityRule.ClientSecurityRule).ToTypeReference()
                     };
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var selectMethod = typeof(SelectOperationResultExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression(nameof(SelectOperationResultExtensions.ChangeItem));

        var selectLambda = new CodeParameterDeclarationExpression { Name = this.DomainType.Name.ToStartLowerCase() }.Pipe(param => new CodeLambdaExpression
            {
                    Parameters = { param },
                    Statements = { param.ToVariableReferenceExpression().Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService())) }
            });

        var selectOperationExpr = evaluateDataExpr.GetContext().ToPropertyReference((IODataBLLContext c) => c.SelectOperationParser)
                                                  .ToMethodInvokeExpression((IParser<string, SelectOperation> parser) => parser.Parse(null), this.Parameter.ToVariableReferenceExpression());

        var selectOperationDecl = typeof(SelectOperation).ToTypeReference().ToVariableDeclarationStatement("selectOperation", selectOperationExpr);

        var expressionBuilderExpr = evaluateDataExpr.GetContext().ToPropertyReference("StandartExpressionBuilder");

        var typedSelectOperationDecl = typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToVariableDeclarationStatement(
         "typedSelectOperation",
         expressionBuilderExpr.ToStaticMethodInvokeExpression(
                                                              typeof(StandartExpressionBuilderExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression("ToTyped", this.DomainType.ToTypeReference()),
                                                              selectOperationDecl.ToVariableReferenceExpression()));

        var treeDecl = new CodeVariableDeclarationStatement("var", "odataTree", bllRefExpr.ToMethodReferenceExpression("GetTreeByOData").ToMethodInvokeExpression(typedSelectOperationDecl.ToVariableReferenceExpression(), this.GetFetchRule()));

        yield return selectOperationDecl;

        yield return typedSelectOperationDecl;

        yield return treeDecl;

        yield return treeDecl.ToVariableReferenceExpression()
                             .ToStaticMethodInvokeExpression(selectMethod, selectLambda)
                             .ToMethodReturnStatement();
    }
}
