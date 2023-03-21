using System;
using System.CodeDom;
using System.Collections.Generic;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLLCoreGenerator;
using Framework.Security;
using Framework.Transfering;

namespace Framework.DomainDriven.ServiceModelGenerator;

public class GetChangeModelMethodGenerator<TConfiguration> : ModelMethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IGeneratorConfigurationBase<IGenerationEnvironmentBase>
{
    public GetChangeModelMethodGenerator(TConfiguration configuration, Type domainType, Type changeModel)
            : base(configuration, domainType, changeModel)
    {
        this.Identity = new MethodIdentity(MethodIdentityType.GetChangeModel, this.ModelType);
    }


    public override MethodIdentity Identity { get; }

    protected override string Name => "Get" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Change, true);

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.ModelType, DTOType.RichDTO);

    protected override bool IsEdit { get; } = true;

    protected override DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Read;


    protected override string GetComment()
    {
        return $"Get {this.DomainType.Name} change model ({this.ModelType.Name})";
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

                                                         "Get" + this.DomainType.GetModelMethodName(this.ModelType, ModelRole.Change, false),

                                                         domainObjectVarDecl.ToVariableReferenceExpression())

                               .ToStaticMethodInvokeExpression(this.GetConvertToDTOMethod(DTOType.RichDTO), evaluateDataExpr.GetMappingService())
                               .ToMethodReturnStatement();
    }

    protected override object GetBLLSecurityParameter()
    {
        var modelSecurityAttribute = this.ModelType.GetEditDomainObjectAttribute();

        if (null == modelSecurityAttribute)
        {
            return base.GetBLLSecurityParameter();
        }

        return modelSecurityAttribute.SecurityOperationCode;
    }
}
