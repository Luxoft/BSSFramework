using System.CodeDom;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.DTO.Extensions;
using Framework.BLL.Domain.Models;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.WebApiGenerator.Configuration._Base;
using Framework.CodeGeneration.WebApiGenerator.Extensions;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators._Base;
using Framework.Core;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Main;

public class FormatMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    private readonly DTOType resultDToType = DTOType.RichDTO;


    public FormatMethodGenerator(TConfiguration configuration, Type domainType, Type formatModel)
            : base(configuration, domainType, formatModel)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.GetWithFormat, this.ModelType);
    }


    public override MethodIdentity Identity { get; }


    protected override string Name =>
            $"Get{this.resultDToType.WithoutPostfix()}{this.DomainType.Name}With{this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Format, false)}";

    protected override string GetComment()
    {
        return $"Get {this.DomainType.Name} with format ({this.ModelType.Name})";
    }

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.resultDToType);

    protected override bool IsEdit { get; } = false;

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO
                         .GetCodeTypeReference(this.ModelType, DTOType.StrictDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "FormatModel");

    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var formatModelDecl = this.ModelType.ToTypeReference().ToVariableDeclarationStatement("formatModel", this.Parameter.ToVariableReferenceExpression().ToMethodInvokeExpression(this.Configuration.Environment.ServerDTO.ToDomainObjectMethodName, evaluateDataExpr.GetMappingService()));

        yield return formatModelDecl;

        yield return bllRefExpr.ToMethodInvokeExpression(this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Format, false),
                                                         formatModelDecl.ToVariableReferenceExpression())

                               .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.RichDTO), evaluateDataExpr.GetMappingService())
                               .ToMethodReturnStatement();
    }
}
