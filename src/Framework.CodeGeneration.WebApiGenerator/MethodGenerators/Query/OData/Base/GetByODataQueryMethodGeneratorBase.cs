using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Extensions;
using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main.View._Base;
using Framework.OData;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Query.OData.Base;

public abstract class GetByODataQueryMethodGeneratorBase<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected GetByODataQueryMethodGeneratorBase(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType)
    {
        this.ReturnType = typeof(SelectOperationResult<>).ToTypeReference(this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType));
    }


    protected sealed override CodeTypeReference ReturnType { get; }


    protected abstract CodeExpression GetSelectOperationExpression(CodeExpression evaluateDataExpr);

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var selectOperationDecl = typeof(SelectOperation).ToTypeReference().ToVariableDeclarationStatement("selectOperation", this.GetSelectOperationExpression(evaluateDataExpr));

        var expressionBuilderExpr = evaluateDataExpr.GetContext().ToPropertyReference("StandardExpressionBuilder");

        var typedSelectOperationDecl = typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToVariableDeclarationStatement("typedSelectOperation",

            expressionBuilderExpr.ToStaticMethodInvokeExpression(typeof(StandardExpressionBuilderExtensions).ToTypeReferenceExpression()
                                                                         .ToMethodReferenceExpression("ToTyped", this.DomainType.ToTypeReference()),

                                                                 selectOperationDecl.ToVariableReferenceExpression()));

        var preResultDecl = typeof(SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference().ToVariableDeclarationStatement("preResult",
            bllRefExpr.ToMethodInvokeExpression("GetObjectsByOData", typedSelectOperationDecl.ToVariableReferenceExpression(), this.GetFetchRule()));

        var preResultDeclRefExpr = preResultDecl.ToVariableReferenceExpression();


        yield return selectOperationDecl;

        yield return typedSelectOperationDecl;

        yield return preResultDecl;

        yield return

                this.ReturnType.ToObjectCreateExpression(
                                                         preResultDeclRefExpr.ToPropertyReference((SelectOperationResult<object> res) => res.Items)
                                                                             .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService())),
                                                         preResultDeclRefExpr.ToPropertyReference((SelectOperationResult<object> res) => res.TotalCount))
                    .ToMethodReturnStatement();
    }
}
