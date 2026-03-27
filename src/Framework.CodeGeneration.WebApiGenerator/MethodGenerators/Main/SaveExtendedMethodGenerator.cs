using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Models;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Extensions;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;
using Framework.Core;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main;

public class SaveExtendedMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLSaveRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public SaveExtendedMethodGenerator(TConfiguration configuration, Type domainType, Type extendedModel)
            : base(configuration, domainType, extendedModel)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.SaveByModel, this.ModelType);
    }


    public override MethodIdentity Identity { get; }


    protected override string Name => "Save" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Extended, true);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO);

    protected override bool IsEdit { get; } = true;


    protected override string GetComment()
    {
        return $"Save {this.DomainType.Name} by model {this.ModelType.Name}";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO
                         .GetCodeTypeReference(this.ModelType, DTOType.StrictDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "ExtendedModel");

    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var extendedModelDecl = this.ModelType.ToTypeReference().ToVariableDeclarationStatement("extendedModel", this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return extendedModelDecl;

        yield return bllRefExpr.ToMethodInvokeExpression("Save" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Extended, false),
                                                         extendedModelDecl.ToVariableReferenceExpression())

                               .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.IdentityDTO))
                               .ToMethodReturnStatement();
    }
}
