using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.Core.Serialization;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Generation.Domain;
using Framework.OData;
using Framework.Persistent;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class GetODataTreeByQueryStringWithFilterMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly Type filterType;

    public GetODataTreeByQueryStringWithFilterMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType, Type filterType)
            : base(configuration, domainType, dtoType)
    {
        this.filterType = filterType;
        this.Identity = new MethodIdentity(MethodIdentityType.GetODataTreeByQueryStringWithFilter, filterType, this.DTOType);

        this.Name = this.filterType.Name.Skip(this.DomainType.Name).SkipLast("Model").Replace("OData", "")
                        .Pipe(filterBody => this.CreateName(false, "ODataQueryStringWith" + filterBody, "TreeBy"));

        var elementTypeRef = typeof(HierarchicalNode<,>).ToTypeReference(
                                                                         this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType),
                                                                         this.Configuration.Environment.GetIdentityType().ToTypeReference());

        this.ReturnType = typeof(SelectOperationResult<>).ToTypeReference(elementTypeRef);
    }


    public override MethodIdentity Identity { get; }

    protected override string Name { get; }

    protected override CodeTypeReference ReturnType { get; }

    protected override string GetComment()
    {
        return $"Get hierarchical data of type {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by odata string and filter ({this.filterType.Name})";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression("odataQueryString");

        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.filterType, Transfering.DTOType.StrictDTO)
                         .ToParameterDeclarationExpression("filter");
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var typedFilterDeсl = this.filterType.ToTypeReference().ToVariableDeclarationStatement("typedFilter", this.Parameters[1].ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return typedFilterDeсl;

        var selectMethod = typeof(SelectOperationResultExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression(nameof(SelectOperationResultExtensions.SelectN));

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

        var treeDecl = new CodeVariableDeclarationStatement("var", "odataTree", bllRefExpr.ToMethodReferenceExpression("GetTreeByOData").ToMethodInvokeExpression(typedSelectOperationDecl.ToVariableReferenceExpression(), typedFilterDeсl.ToVariableReferenceExpression(), this.GetFetchsExpression(evaluateDataExpr)));

        yield return selectOperationDecl;

        yield return typedSelectOperationDecl;

        yield return treeDecl;

        yield return treeDecl.ToVariableReferenceExpression()
                             .ToStaticMethodInvokeExpression(selectMethod, selectLambda)
                             .ToMethodReturnStatement();
    }
}
