using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.ServiceModelGenerator;
using Framework.Security;
using Framework.Transfering;

using SampleSystem.Domain;

namespace SampleSystem.CodeGenerate;

/// <summary>
/// Генератор кастомного фасадного метода по ComplexChange-модели
/// </summary>
public class ComplexChangeMethodGenerator : ModelMethodGenerator<MainServiceGeneratorConfiguration, BLLSaveRoleAttribute>
{
    public ComplexChangeMethodGenerator(MainServiceGeneratorConfiguration configuration, Type domainType, Type changeModel)
            : base(configuration, domainType, changeModel)
    {
        this.Identity = new MethodIdentity(SampleSystemMethodIdentityType.ComplexChange, this.ModelType);
    }


    public override MethodIdentity Identity { get; }


    protected override string Name => this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Change, true);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

    protected override bool IsEdit { get; } = true;


    protected override string GetComment()
    {
        return $"Change {this.DomainType.Name} by model ({this.ModelType.Name})";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO
                         .GetCodeTypeReference(this.ModelType, DTOType.StrictDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "ChangeModel");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var changeModelDecl = this.ModelType.ToTypeReference().ToVariableDeclarationStatement("changeModel", this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return changeModelDecl;

        var domainObjRef = changeModelDecl.ToVariableReferenceExpression().ToPropertyReference(nameof(DomainObjectComplexChangeModel<PersistentDomainObjectBase>.PrimaryChangingObject));

        yield return bllRefExpr.ToMethodInvokeExpression("CheckAccess", domainObjRef).ToExpressionStatement();

        var domainObjectsRef = changeModelDecl.ToVariableReferenceExpression().ToPropertyReference(nameof(DomainObjectComplexChangeModel<PersistentDomainObjectBase>.SecondaryChangingObjects));

        yield return new CodeParameterDeclarationExpression { Name = "secondaryDomainObject" }.Pipe(iterator => new CodeForeachStatement
            {
                    Source = domainObjectsRef,
                    Iterator = iterator,
                    Statements =
                    {
                            bllRefExpr.ToMethodInvokeExpression("CheckAccess", iterator.ToVariableReferenceExpression()).ToExpressionStatement()
                    }
            });

        yield return bllRefExpr.ToMethodInvokeExpression(this.DomainType.GetModelMethodName(this.ModelType, SampleSystemModelRole.ComplexChange, false),
                                                         changeModelDecl.ToVariableReferenceExpression())

                               .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.IdentityDTO))
                               .ToMethodReturnStatement();
    }

    protected override object GetBLLSecurityParameter()
    {
        var modelSecurityAttribute = this.ModelType.GetEditDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter();
        }

        return modelSecurityAttribute.SecurityOperationCode;
    }
}
