using System.CodeDom;

using CommonFramework;

using Framework.BLL;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;
using Framework.Core;

using OData;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;

public abstract class ViewMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
    : MethodGenerator<TConfiguration, BLLViewRoleBaseAttribute>(configuration, domainType)
    where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public readonly ViewDTOType DTOType = dtoType;

    protected sealed override bool IsEdit { get; } = false;

    protected ViewDTOType MaxFetchLevel => this.DTOType.Min(((IMaxFetchContainer)this.Attribute).MaxFetch);

    protected CodeExpression GetFetchRule()
    {
        var args = new[] { this.MaxFetchLevel.ToPrimitiveExpression() };

        return typeof(DTOFetchRule<>).MakeGenericType(this.DomainType)
                                     .ToTypeReference()
                                     .ToObjectCreateExpression(args);
    }

    protected string CreateName(bool pluralize, string postfix, string postfixName = "By")
    {
        var bodyPostfix = postfix.MaybeString(s => postfixName + s);

        return $"Get{this.GetDTOPrefix()}{this.DomainType.Name.Pipe(pluralize, s => s.ToPluralize())}{bodyPostfix}";
    }

    protected CodeExpression GetSelectOperationExpression(CodeExpression evaluateDataExpr) =>
        evaluateDataExpr.GetContext()
                        .ToPropertyReference("SelectOperationParser")
                        .ToMethodReferenceExpression(nameof(ISelectOperationParser.Parse), this.DomainType.ToTypeReference())
                        .ToMethodInvokeExpression(this.Parameter.ToVariableReferenceExpression());

    protected virtual string GetDTOPrefix() => this.DTOType == ViewDTOType.ProjectionDTO ? string.Empty : this.DTOType.WithoutPostfix();

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclById(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr, bool withFetchs, CodeParameterDeclarationExpression parameter)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));

        if (withFetchs)
        {
            return this.ToDomainObjectVarDecl(this.Configuration.GetByIdExpr(bllRefExpr, parameter.ToVariableReferenceExpression(), this.GetFetchRule()));
        }
        else
        {
            return base.ToDomainObjectVarDeclById(bllRefExpr, parameter);
        }
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclById(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr, bool withFetchs) => this.ToDomainObjectVarDeclById(evaluateDataExpr, bllRefExpr, withFetchs, this.Parameter);

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclByName(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr, CodeParameterDeclarationExpression parameter)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));

        return this.ToDomainObjectVarDecl(this.Configuration.GetByNameExpr(bllRefExpr, parameter.ToVariableReferenceExpression(), this.GetFetchRule()));
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclByName(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr) => this.ToDomainObjectVarDeclByName(evaluateDataExpr, bllRefExpr, this.Parameter);

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclByCode(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr, CodeParameterDeclarationExpression parameter)
    {
        if (evaluateDataExpr == null) throw new ArgumentNullException(nameof(evaluateDataExpr));
        if (bllRefExpr == null) throw new ArgumentNullException(nameof(bllRefExpr));
        if (parameter == null) throw new ArgumentNullException(nameof(parameter));

        return this.ToDomainObjectVarDecl(this.Configuration.GetByCodeExpr(bllRefExpr, parameter.ToVariableReferenceExpression(), this.GetFetchRule()));
    }

    protected CodeVariableDeclarationStatement ToDomainObjectVarDeclByCode(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr) => this.ToDomainObjectVarDeclByCode(evaluateDataExpr, bllRefExpr, this.Parameter);

    protected CodeMethodReferenceExpression GetConvertToDTOMethod() => this.GetConvertToDTOMethod(this.DTOType);

    protected CodeMethodReferenceExpression GetConvertToDTOListMethod() => this.GetConvertToDTOListMethod(this.DTOType);

    protected CodeExpression ConvertToDTO(CodeExpression sourceExpr, CodeExpression mappingServiceExpr) => sourceExpr.ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(), mappingServiceExpr);

    protected CodeExpression ConvertToDTOList(CodeExpression sourceExpr, CodeExpression mappingServiceExpr) => sourceExpr.ToStaticMethodInvokeExpression(this.GetConvertToDTOListMethod(), mappingServiceExpr);
}
