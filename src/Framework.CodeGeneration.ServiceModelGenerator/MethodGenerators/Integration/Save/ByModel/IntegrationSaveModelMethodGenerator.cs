using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Models;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.DTOGenerator.Server;
using Framework.CodeGeneration.DTOGenerator.Server.FileFactory._Helpers;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Integration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Save.Base;
using Framework.Core;
using Framework.Database;
using Framework.Database.Attr;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Integration.Save.ByModel;

public class IntegrationSaveModelMethodGenerator<TConfiguration> : IntegrationMethodGenerator<TConfiguration, BLLIntegrationSaveRoleAttribute>
        where TConfiguration : class, IIntegrationGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly Type modelType;


    public IntegrationSaveModelMethodGenerator(TConfiguration configuration, Type domainType, Type modelType)
            : base(configuration, domainType)
    {
        if (modelType == null) throw new ArgumentNullException(nameof(modelType));

        this.modelType = modelType;

        this.Identity = new MethodIdentity(MethodIdentityType.IntegrationSaveByModel, this.modelType);
    }


    public override MethodIdentity Identity { get; }

    protected override string Name => this.DomainType.GetModelMethodName(this.modelType, ModelRole.IntegrationSave, true, "Save");

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

    protected override BLLIntegrationSaveRoleAttribute Attribute =>

            this.modelType.GetCustomAttribute<BLLIntegrationSaveRoleAttribute>() ?? new BLLIntegrationSaveRoleAttribute();

    protected sealed override DBSessionMode SessionMode =>

            this.modelType.GetCustomAttribute<DbSessionModeAttribute>().Maybe(attr => attr.SessionMode, () => base.SessionMode);


    protected override string GetComment() => $"Save {this.DomainType.Name} by model {this.modelType.Name}";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO
                         .GetCodeTypeReference(this.modelType, ServerFileType.RichIntegrationDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "IntegrationSaveModel");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        foreach (var baseStatement in base.GetFacadeMethodInternalStatements(evaluateDataExpr, bllRefExpr))
        {
            yield return baseStatement;
        }

        var integrationSaveModelDecl = this.modelType.ToTypeReference().ToVariableDeclarationStatement("integrationSaveModel", this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

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
