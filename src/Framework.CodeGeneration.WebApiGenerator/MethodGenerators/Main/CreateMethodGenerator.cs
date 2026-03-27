using System.CodeDom;

using Framework.Application.Session;
using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.Models;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Extensions;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;
using Framework.Core;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main;

public class CreateMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLSaveRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public CreateMethodGenerator(TConfiguration configuration, Type domainType, Type createModel)
            : base(configuration, domainType, createModel)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.Create, createModel);
    }


    public override MethodIdentity Identity { get; }


    protected override string Name => this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Create, true);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.RichDTO);

    protected override bool IsEdit { get; } = true;

    protected override DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Read;


    protected override string GetComment()
    {
        return $"Create {this.DomainType.Name} by model ({this.ModelType.Name})";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.ModelType, DTOType.StrictDTO)
                         .ToParameterDeclarationExpression (this.DomainType.Name.ToStartLowerCase() + "CreateModel");
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var createModelDecl = this.ModelType.ToTypeReference().ToVariableDeclarationStatement("createModel", this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        var createObjectDecl = this.ToDomainObjectVarDecl(bllRefExpr.ToMethodInvokeExpression(

                                                           this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Create, false),
                                                           createModelDecl.ToVariableReferenceExpression()));

        yield return createModelDecl;

        yield return createObjectDecl;

        yield return bllRefExpr.ToMethodInvokeExpression("CheckAccess", createObjectDecl.ToVariableReferenceExpression()).ToExpressionStatement();

        yield return createObjectDecl.ToVariableReferenceExpression()
                                     .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.RichDTO), evaluateDataExpr.GetMappingService())
                                     .ToMethodReturnStatement();
    }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        var modelSecurityAttribute = this.ModelType.GetEditDomainObjectAttribute();

        return null == modelSecurityAttribute
                       ? base.GetBLLSecurityParameter(evaluateDataExpr)
                       : modelSecurityAttribute.SecurityRule;
    }
}
