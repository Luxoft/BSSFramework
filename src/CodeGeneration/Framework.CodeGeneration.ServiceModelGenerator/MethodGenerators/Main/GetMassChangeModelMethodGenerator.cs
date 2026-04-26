using System.CodeDom;

using Anch.Core;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.Extensions;
using Framework.BLL.Domain.Models;
using Framework.BLL.Domain.ServiceRole;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.BLLCoreGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.Database;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main;

public class GetMassChangeModelMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IServiceModelGeneratorConfiguration<IServiceModelGenerationEnvironment>
{
    public GetMassChangeModelMethodGenerator(TConfiguration configuration, Type domainType, Type changeModel)
            : base(configuration, domainType, changeModel) =>
        this.Identity = new MethodIdentity(MethodIdentityType.GetMassChangeModel, this.ModelType);

    public override MethodIdentity Identity { get; }

    protected override string Name => "Get" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.MassChange, true);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.ModelType, DTOType.RichDTO);

    protected override bool IsEdit { get; } = true;

    protected override DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Read;


    protected override string GetComment() => $"Get Mass {this.DomainType.Name} change model ({this.ModelType.Name})";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO
                         .GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO).ToArrayReference()
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Ident");

    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var domainObjectsVarDecl = this.DomainType.ToTypeReference().ToListReference().ToVariableDeclarationStatement("domainObjects", bllRefExpr.ToMethodInvokeExpression(
         "GetListByIdents",
         this.Parameter.ToVariableReferenceExpression()));

        yield return domainObjectsVarDecl;

        yield return bllRefExpr.ToMethodInvokeExpression(

                                                         "Get" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.MassChange, false),

                                                         domainObjectsVarDecl.ToVariableReferenceExpression())

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
