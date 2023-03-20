using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.OData;
using Framework.Security;
using Framework.Transfering;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class GetByODataQueryWithFilterMethodGeneratorBase<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    protected readonly Type FilterType;


    protected GetByODataQueryWithFilterMethodGeneratorBase(TConfiguration configuration, Type domainType, ViewDTOType dtoType, [NotNull] Type filterType)
            : base(configuration, domainType, dtoType)
    {
        if (filterType == null) throw new ArgumentNullException(nameof(filterType));

        this.FilterType = filterType;

        this.ReturnType = typeof(SelectOperationResult<>).ToTypeReference(this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType));
    }

    protected sealed override CodeTypeReference ReturnType { get; }

    protected abstract CodeExpression GetSelectOperationExpression(CodeExpression evaluateDataExpr);

    protected override object GetBLLSecurityParameter()
    {
        var modelSecurityAttribute = this.FilterType.GetViewDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter();
        }

        return modelSecurityAttribute.SecurityOperationCode;

    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var selectOperationDecl = typeof(SelectOperation).ToTypeReference().ToVariableDeclarationStatement("selectOperation", this.GetSelectOperationExpression(evaluateDataExpr));

        var expressionBuilderExpr = evaluateDataExpr.GetContext().ToPropertyReference("StandartExpressionBuilder");

        var typedSelectOperationDecl = typeof(SelectOperation<>).ToTypeReference(this.DomainType).ToVariableDeclarationStatement("typedSelectOperation",

            expressionBuilderExpr.ToStaticMethodInvokeExpression(typeof(StandartExpressionBuilderExtensions).ToTypeReferenceExpression()
                                                                         .ToMethodReferenceExpression("ToTyped", this.DomainType.ToTypeReference()),

                                                                 selectOperationDecl.ToVariableReferenceExpression()));

        var typedFilterDecl = this.FilterType.ToTypeReference().ToVariableDeclarationStatement("typedFilter",
            this.Parameters[1].ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        var preResultDecl = typeof(SelectOperationResult<>).MakeGenericType(this.DomainType).ToTypeReference().ToVariableDeclarationStatement("preResult",
            bllRefExpr.ToMethodInvokeExpression("GetObjectsByOData", typedSelectOperationDecl.ToVariableReferenceExpression(), typedFilterDecl.ToVariableReferenceExpression(), this.GetFetchsExpression(evaluateDataExpr)));

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
