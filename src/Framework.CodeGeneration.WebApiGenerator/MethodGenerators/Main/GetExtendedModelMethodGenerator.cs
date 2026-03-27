using System.CodeDom;

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

public class GetExtendedModelMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetExtendedModelMethodGenerator(TConfiguration configuration, Type domainType, Type extendedModel)
            : base(configuration, domainType, extendedModel)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.GetWithExtended, this.ModelType);
    }


    public override MethodIdentity Identity { get; }

    protected override string Name => "Get" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Extended, true);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.ModelType, DTOType.RichDTO);

    protected override bool IsEdit { get; } = false;


    protected override string GetComment()
    {
        return $"Get {this.DomainType.Name} with extended data ({this.ModelType.Name})";
    }

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

                                                         "Get" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Extended, false),

                                                         domainObjectVarDecl.ToVariableReferenceExpression())

                               .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.RichDTO), evaluateDataExpr.GetMappingService())
                               .ToMethodReturnStatement();
    }

    protected override object GetBLLSecurityParameter(CodeExpression evaluateDataExpr)
    {
        var modelSecurityAttribute = this.ModelType.GetViewDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter(evaluateDataExpr);
        }

        return modelSecurityAttribute.SecurityRule;
    }
}
