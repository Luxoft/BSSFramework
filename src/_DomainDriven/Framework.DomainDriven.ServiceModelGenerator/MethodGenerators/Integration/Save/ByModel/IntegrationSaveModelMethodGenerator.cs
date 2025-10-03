using System.CodeDom;

using CommonFramework;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.DomainDriven.DTOGenerator.Server;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class IntegrationSaveModelMethodGenerator<TConfiguration> : IntegrationMethodGenerator<TConfiguration, BLLIntegrationSaveRoleAttribute>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly Type _modelType;


    public IntegrationSaveModelMethodGenerator(TConfiguration configuration, Type domainType, Type modelType)
            : base(configuration, domainType)
    {
        if (modelType == null) throw new ArgumentNullException(nameof(modelType));

        this._modelType = modelType;

        this.Identity = new MethodIdentity(MethodIdentityType.IntegrationSaveByModel, this._modelType);
    }


    public override MethodIdentity Identity { get; }

    protected override string Name => this.DomainType.GetModelMethodName(this._modelType, ModelRole.IntegrationSave, true, "Save");

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

    protected override BLLIntegrationSaveRoleAttribute Attribute =>

            this._modelType.GetCustomAttribute<BLLIntegrationSaveRoleAttribute>() ?? new BLLIntegrationSaveRoleAttribute();

    protected sealed override DBSessionMode SessionMode =>

            this._modelType.GetCustomAttribute<DBSessionModeAttribute>().Maybe(attr => attr.SessionMode, () => base.SessionMode);


    protected override string GetComment()
    {
        return $"Save {this.DomainType.Name} by model {this._modelType.Name}";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO
                         .GetCodeTypeReference(this._modelType, ServerFileType.RichIntegrationDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "IntegrationSaveModel");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        foreach (var baseStatement in base.GetFacadeMethodInternalStatements(evaluateDataExpr, bllRefExpr))
        {
            yield return baseStatement;
        }

        var integrationSaveModelDecl = this._modelType.ToTypeReference().ToVariableDeclarationStatement("integrationSaveModel", this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return integrationSaveModelDecl;

        var integrationVersionProperty = this.DomainType.TryGetIntegrationVersionProperty();

        if (null == integrationVersionProperty)
        {
            yield return bllRefExpr.ToMethodInvokeExpression(this.Configuration.Environment.BLLCore.IntegrationSaveMethodName,integrationSaveModelDecl.ToVariableReferenceExpression())
                                   .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.IdentityDTO))
                                   .ToMethodReturnStatement();
        }
        else
        {
            var saveObjectDecl = this.ToDomainObjectVarDecl(integrationSaveModelDecl.ToVariableReferenceExpression().ToPropertyReference($"{nameof(IDomainObjectIntegrationSaveModel<object>.SavingObject)}"));

            yield return saveObjectDecl;

            var domainObjectVersion = new CodePropertyReferenceExpression(saveObjectDecl.ToVariableReferenceExpression(), integrationVersionProperty.Name);

            var savingModelRef = this.Parameter.ToVariableReferenceExpression().ToPropertyReference($"{nameof(IDomainObjectIntegrationSaveModel<object>.SavingObject)}");

            var dtoVersion = new CodePropertyReferenceExpression(savingModelRef, integrationVersionProperty.Name);

            var codeBinaryOperatorType = integrationVersionProperty.ToCodeBinaryOperator();

            var less = new CodeBinaryOperatorExpression(
                                                        domainObjectVersion,
                                                        codeBinaryOperatorType,
                                                        dtoVersion);

            yield return new CodeConditionStatement(less)
                         {
                                 TrueStatements =
                                 {
                                         savingModelRef.ToPropertyReference(integrationVersionProperty.Name).ToAssignStatement(saveObjectDecl.ToVariableReferenceExpression().ToPropertyReference(integrationVersionProperty.Name)),
                                         bllRefExpr.ToMethodInvokeExpression(
                                                                             this.Configuration.Environment.BLLCore.IntegrationSaveMethodName,
                                                                             integrationSaveModelDecl.ToVariableReferenceExpression())
                                 }
                         };

            yield return saveObjectDecl.ToVariableReferenceExpression()
                                       .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.IdentityDTO))
                                       .ToMethodReturnStatement();

        }
    }
}
