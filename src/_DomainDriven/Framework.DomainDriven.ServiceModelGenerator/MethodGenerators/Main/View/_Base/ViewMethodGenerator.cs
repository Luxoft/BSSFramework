using System.CodeDom;

using CommonFramework;

using Framework.Core;
using Framework.CodeDom;
using Framework.DomainDriven.BLL;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public abstract class ViewMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLViewRoleBaseAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public readonly ViewDTOType DTOType;


    protected ViewMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType)
    {
        this.DTOType = dtoType;
    }


    protected sealed override bool IsEdit { get; } = false;

    protected ViewDTOType MaxFetchLevel => this.DTOType.Min(((IMaxFetchContainer)this.Attribute).MaxFetch);


    protected CodeExpression GetFetchsExpression(CodeExpression evaluateDataExpr)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));

        return this.GetFetchsExpressionC(evaluateDataExpr.GetContext());
    }

    protected CodeExpression GetFetchsExpressionC(CodeExpression contextExpr)
    {
        if (contextExpr == null) throw new ArgumentNullException(nameof(contextExpr));

        var fetchParams = new[] { this.MaxFetchLevel.ToPrimitiveExpression() };

        return contextExpr.GetFetchContainerExpr(this.DomainType, fetchParams);
    }

    protected string CreateName(bool pluralize, string postfix, string postfixName = "By")
    {
        var bodyPostfix = postfix.MaybeString(s => postfixName + s);

        return $"Get{this.GetDTOPrefix()}{this.DomainType.Name.Pipe(pluralize, s => s.ToPluralize())}{bodyPostfix}";
    }


    protected virtual string GetDTOPrefix()
    {
        return this.DTOType == ViewDTOType.ProjectionDTO ? string.Empty : this.DTOType.WithoutPostfix();
    }


    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclById(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr, bool withFetchs, CodeParameterDeclarationExpression parameter)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));

        if (withFetchs)
        {
            return this.ToDomainObjectVarDecl(this.Configuration.GetByIdExpr(bllRefExpr, parameter.ToVariableReferenceExpression(), this.GetFetchsExpressionC(evaluateDataExpr.GetContext())));
        }
        else
        {
            return base.ToDomainObjectVarDeclById(bllRefExpr, parameter);
        }
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclById(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr, bool withFetchs)
    {
        return this.ToDomainObjectVarDeclById(evaluateDataExpr, bllRefExpr, withFetchs, this.Parameter);
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclByName(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr, CodeParameterDeclarationExpression parameter)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));

        return this.ToDomainObjectVarDecl(this.Configuration.GetByNameExpr(bllRefExpr, parameter.ToVariableReferenceExpression(), this.GetFetchsExpressionC(evaluateDataExpr.GetContext())));
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclByName(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        return this.ToDomainObjectVarDeclByName(evaluateDataExpr, bllRefExpr, this.Parameter);
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclByCode(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr, CodeParameterDeclarationExpression parameter)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));

        return this.ToDomainObjectVarDecl(this.Configuration.GetByCodeExpr(bllRefExpr, parameter.ToVariableReferenceExpression(), this.GetFetchsExpressionC(evaluateDataExpr.GetContext())));
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclByCode(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        return this.ToDomainObjectVarDeclByCode(evaluateDataExpr, bllRefExpr, this.Parameter);
    }


    protected CodeMethodReferenceExpression GetConvertToDTOMethod()
    {
        return this.GetConvertToDTOMethod(this.DTOType);
    }

    protected CodeMethodReferenceExpression GetConvertToDTOListMethod()
    {
        return this.GetConvertToDTOListMethod(this.DTOType);
    }

    protected CodeExpression ConvertToDTO(CodeExpression sourceExpr, CodeExpression mappingServiceExpr)
    {
        return sourceExpr.ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(), mappingServiceExpr);
    }

    protected CodeExpression ConvertToDTOList(CodeExpression sourceExpr, CodeExpression mappingServiceExpr)
    {
        return sourceExpr.ToStaticMethodInvokeExpression(this.GetConvertToDTOListMethod(), mappingServiceExpr);
    }
}
