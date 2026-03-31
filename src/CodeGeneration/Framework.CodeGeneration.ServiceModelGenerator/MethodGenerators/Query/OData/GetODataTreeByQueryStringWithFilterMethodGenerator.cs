using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.BLL.Domain.Persistent;
using Framework.CodeDom.Extend;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;
using Framework.Core;
using Framework.FileGeneration.Configuration;

using OData.Domain;

using SelectOperationResultExtensions = Framework.BLL.OData.SelectOperationResultExtensions;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData;

public class GetODataTreeByQueryStringWithFilterMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
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

    protected override string GetComment() => $"Get hierarchical data of type {this.DomainType.GetPluralizedDomainName()} ({this.DTOType}) by odata string and filter ({this.filterType.Name})";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return typeof(string).ToTypeReference().ToParameterDeclarationExpression("odataQueryString");

        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.filterType, BLL.Domain.DTO.DTOType.StrictDTO)
                         .ToParameterDeclarationExpression("filter");
    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var typedFilterDeсl = this.filterType.ToTypeReference().ToVariableDeclarationStatement("typedFilter", this.Parameters[1].ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return typedFilterDeсl;

        var selectMethod = typeof(SelectOperationResultExtensions).ToTypeReferenceExpression().ToMethodReferenceExpression(nameof(SelectOperationResultExtensions.ChangeItem));

        var selectLambda = new CodeParameterDeclarationExpression { Name = this.DomainType.Name.ToStartLowerCase() }.Pipe(param => new CodeLambdaExpression
            {
                    Parameters = { param },
                    Statements = { param.ToVariableReferenceExpression().Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService())) }
            });

        var selectOperationDecl = typeof(SelectOperation<>).ToTypeReference(this.DomainType)
                                                           .ToVariableDeclarationStatement("selectOperation", this.GetSelectOperationExpression(evaluateDataExpr));

        var treeDecl = new CodeVariableDeclarationStatement("var", "odataTree", bllRefExpr.ToMethodReferenceExpression("GetTreeByOData").ToMethodInvokeExpression(selectOperationDecl.ToVariableReferenceExpression(), typedFilterDeсl.ToVariableReferenceExpression(), this.GetFetchRule()));

        yield return selectOperationDecl;

        yield return treeDecl;

        yield return treeDecl.ToVariableReferenceExpression()
                             .ToStaticMethodInvokeExpression(selectMethod, selectLambda)
                             .ToMethodReturnStatement();
    }
}
