using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

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
