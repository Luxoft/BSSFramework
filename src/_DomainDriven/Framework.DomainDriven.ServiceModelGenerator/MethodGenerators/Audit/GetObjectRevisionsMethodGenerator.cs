using System.CodeDom;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.SecuritySystem;
using Framework.Transfering;


namespace Framework.DomainDriven.ServiceModelGenerator;

public class GetObjectRevisionsMethodGenerator<TConfiguration> : MethodGenerator<TConfiguration, BLLViewRoleAttribute>
        where TConfiguration : class, IAuditGeneratorConfigurationBase<IAuditGenerationEnvironmentBase>
{
    public GetObjectRevisionsMethodGenerator(TConfiguration configuration, Type domainType)
            : base(configuration, domainType)
    {
    }


    public override MethodIdentity Identity { get; } = MethodIdentityType.GetRevisions;

    protected override SecurityRule SecurityRule { get; } = SecurityRule.View;

    protected override string Name => $"Get{this.DomainType.Name}Revisions";

    protected override CodeTypeReference ReturnType => typeof(DefaultDomainObjectRevisionDTO).ToTypeReference();


    protected override string GetComment()
    {
        return $"Get {this.DomainType.Name} revisions";
    }

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, DTOType.IdentityDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

    }

    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        yield return typeof(DefaultDomainObjectRevisionDTO)
                     .ToTypeReference()
                     .ToObjectCreateExpression(bllRefExpr.ToMethodInvokeExpression("GetObjectRevisions", this.Parameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty)))
                     .ToMethodReturnStatement();
    }
}
