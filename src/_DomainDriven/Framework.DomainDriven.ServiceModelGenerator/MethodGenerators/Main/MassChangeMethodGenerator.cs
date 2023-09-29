using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.Security;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class MassChangeMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLSaveRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public MassChangeMethodGenerator(TConfiguration configuration, Type domainType, Type changeModel)
            : base(configuration, domainType, changeModel)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.MassChange, this.ModelType);
    }


    public override MethodIdentity Identity { get; }


    protected override string Name => this.DomainType.GetModelMethodName(this.ModelType, ModelRole.MassChange, true);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO).ToListReference();

    protected override bool IsEdit { get; } = true;


    protected override string GetComment()
    {
        return $"Mass change {this.DomainType.Name} by model ({this.ModelType.Name})";
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

        var domainObjectsRef = changeModelDecl.ToVariableReferenceExpression().ToPropertyReference((IDomainObjectMassChangeModel<object> model) => model.ChangingObjects);

        yield return new CodeParameterDeclarationExpression { Name = "domainObject" }.Pipe(iterator => new CodeForeachStatement
            {
                    Source = domainObjectsRef,
                    Iterator = iterator,
                    Statements =
                    {
                            bllRefExpr.ToMethodInvokeExpression("CheckAccess", iterator.ToVariableReferenceExpression()).ToExpressionStatement()
                    }
            });

        yield return bllRefExpr.ToMethodInvokeExpression(this.DomainType.GetModelMethodName(this.ModelType, ModelRole.MassChange, false),
                                                         changeModelDecl.ToVariableReferenceExpression())

                               .ToStaticMethodInvokeExpression(this.GetConvertToDTOListMethod(DTOType.IdentityDTO))
                               .ToMethodReturnStatement();
    }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        var modelSecurityAttribute = this.ModelType.GetEditDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter(evaluateDataExpr);
        }

        return modelSecurityAttribute.SecurityOperation;
    }
}
