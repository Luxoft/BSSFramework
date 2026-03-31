using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.Models;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration._Base;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators._Base;
using Framework.Database;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main;

public class GetChangeModelMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetChangeModelMethodGenerator(TConfiguration configuration, Type domainType, Type changeModel)
            : base(configuration, domainType, changeModel) =>
        this.Identity = new MethodIdentity(MethodIdentityType.GetChangeModel, this.ModelType);

    public override MethodIdentity Identity { get; }

    protected override string Name => "Get" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Change, true);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.ModelType, DTOType.RichDTO);

    protected override bool IsEdit { get; } = true;

    protected override DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Read;


    protected override string GetComment() => $"Get {this.DomainType.Name} change model ({this.ModelType.Name})";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO
                         .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Ident");

    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var domainObjectVarDecl = this.ToDomainObjectVarDeclById(bllRefExpr);

        yield return domainObjectVarDecl;

        yield return bllRefExpr.ToMethodInvokeExpression(

                                                         "Get" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Change, false),

                                                         domainObjectVarDecl.ToVariableReferenceExpression())

                               .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.RichDTO), evaluateDataExpr.GetMappingService())
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
