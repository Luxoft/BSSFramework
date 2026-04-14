using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Extensions;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View;
using OData.Domain;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Query.OData.Base;

public abstract class GetByODataQueryWithFilterMethodGeneratorBase<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
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
        var selectOperationDecl = typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToVariableDeclarationStatement("selectOperation", this.GetSelectOperationExpression(evaluateDataExpr));

        var typedFilterDecl = this.FilterType.ToTypeReference().ToVariableDeclarationStatement("typedFilter",
            this.Parameters[1].ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        var preResultDecl = typeof(SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference().ToVariableDeclarationStatement("preResult",
            bllRefExpr.ToMethodInvokeExpression("GetObjectsByOData", selectOperationDecl.ToVariableReferenceExpression(), typedFilterDecl.ToVariableReferenceExpression(), this.GetFetchRule()));

        var preResultDeclRefExpr = preResultDecl.ToVariableReferenceExpression();


        yield return selectOperationDecl;

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
