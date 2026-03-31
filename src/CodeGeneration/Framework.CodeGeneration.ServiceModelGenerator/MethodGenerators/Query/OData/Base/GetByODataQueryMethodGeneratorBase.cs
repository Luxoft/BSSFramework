using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;
using OData.Domain;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData.Base;

public abstract class GetByODataQueryMethodGeneratorBase<TConfiguration> : ViewMethodGenerator<TConfiguration>
    where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected GetByODataQueryMethodGeneratorBase(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
        : base(configuration, domainType, dtoType) =>
        this.ReturnType = typeof(SelectOperationResult<>).ToTypeReference(this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType));

    protected sealed override CodeTypeReference ReturnType { get; }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var selectOperationDecl = typeof(SelectOperation<>).ToTypeReference(this.DomainType)
                                                           .ToVariableDeclarationStatement("selectOperation", this.GetSelectOperationExpression(evaluateDataExpr));

        var preResultDecl = typeof(SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference().ToVariableDeclarationStatement(
            "preResult",

            bllRefExpr.ToMethodInvokeExpression("GetObjectsByOData", selectOperationDecl.ToVariableReferenceExpression(), this.GetFetchRule()));

        var preResultDeclRefExpr = preResultDecl.ToVariableReferenceExpression();


        yield return selectOperationDecl;

        yield return preResultDecl;

        yield return

            this.ReturnType.ToObjectCreateExpression(
                    preResultDeclRefExpr.ToPropertyReference((SelectOperationResult<object> res) => res.Items)
                                        .Pipe(source => this.ConvertToDTOList(source, evaluateDataExpr.GetMappingService())),
                    preResultDeclRefExpr.ToPropertyReference((SelectOperationResult<object> res) => res.TotalCount))
                .ToMethodReturnStatement();
    }
}
