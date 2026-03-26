using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.Security;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class ChangeMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLSaveRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public ChangeMethodGenerator(TConfiguration configuration, Type domainType, Type changeModel)
            : base(configuration, domainType, changeModel)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.Change, this.ModelType);
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

        var domainObjRef = changeModelDecl.ToVariableReferenceExpression().ToPropertyReference((IDomainObjectChangeModel<object> model) => model.ChangingObject);

        yield return bllRefExpr.ToMethodInvokeExpression("CheckAccess", domainObjRef).ToExpressionStatement();

        yield return bllRefExpr.ToMethodInvokeExpression(this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Change, false),
                                                         changeModelDecl.ToVariableReferenceExpression())

                               .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.IdentityDTO))
                               .ToMethodReturnStatement();
    }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        var modelSecurityAttribute = this.ModelType.GetEditDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter(evaluateDataExpr);
        }

        return modelSecurityAttribute.SecurityRule;
    }
}
