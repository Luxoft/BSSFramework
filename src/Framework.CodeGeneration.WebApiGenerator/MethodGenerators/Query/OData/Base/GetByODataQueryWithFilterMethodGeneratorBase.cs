using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Extensions;
using Framework.CodeDom;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Extensions;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.View._Base;
using Framework.OData;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Query.OData.Base;

public abstract class GetByODataQueryWithFilterMethodGeneratorBase<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly Type FilterType;


    protected GetByODataQueryWithFilterMethodGeneratorBase(TConfiguration configuration, Type domainType, ViewDTOType dtoType, Type filterType)
            : base(configuration, domainType, dtoType)
    {
        if (filterType == null) throw new ArgumentNullException(nameof(filterType));

        this.FilterType = filterType;

        this.ReturnType = typeof(SelectOperationResult<>).ToTypeReference(this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType));
    }

    protected sealed override CodeTypeReference ReturnType { get; }

    protected abstract CodeExpression GetSelectOperationExpression(CodeExpression evaluateDataExpr);

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        var modelSecurityAttribute = this.FilterType.GetViewDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter(evaluateDataExpr);
        }

        return modelSecurityAttribute.SecurityRule;

    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var selectOperationDecl = typeof(SelectOperation).ToTypeReference().ToVariableDeclarationStatement("selectOperation", this.GetSelectOperationExpression(evaluateDataExpr));

        var expressionBuilderExpr = evaluateDataExpr.GetContext().ToPropertyReference("StandardExpressionBuilder");

        var typedSelectOperationDecl = typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToVariableDeclarationStatement("typedSelectOperation",

            expressionBuilderExpr.ToStaticMethodInvokeExpression(typeof(StandardExpressionBuilderExtensions).ToTypeReferenceExpression()
                                                                         .ToMethodReferenceExpression("ToTyped", this.DomainType.ToTypeReference()),

                                                                 selectOperationDecl.ToVariableReferenceExpression()));

        var typedFilterDecl = this.FilterType.ToTypeReference().ToVariableDeclarationStatement("typedFilter",
            this.Parameters[1].ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        var preResultDecl = typeof(SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference().ToVariableDeclarationStatement("preResult",
            bllRefExpr.ToMethodInvokeExpression("GetObjectsByOData", typedSelectOperationDecl.ToVariableReferenceExpression(), typedFilterDecl.ToVariableReferenceExpression(), this.GetFetchRule()));

        var preResultDeclRefExpr = preResultDecl.ToVariableReferenceExpression();


        yield return selectOperationDecl;

        yield return typedSelectOperationDecl;

        yield return typedFilterDecl;

        yield return preResultDecl;

        yield return

                this.ReturnType.ToObjectCreateExpression(
                                                         preResultDeclRefExpr.ToPropertyReference((SelectOperationResult<object> res) => res.Items)
                                                                             .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService())),
                                                         preResultDeclRefExpr.ToPropertyReference((SelectOperationResult<object> res) => res.TotalCount))
                    .ToMethodReturnStatement();
    }
}
