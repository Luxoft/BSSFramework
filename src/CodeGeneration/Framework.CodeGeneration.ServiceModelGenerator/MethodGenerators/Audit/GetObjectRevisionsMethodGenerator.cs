using System.CodeDom;

using Anch.Core;

using Framework.BLL.Domain.DTO;
using Framework.BLL.Domain.ServiceRole;
using Framework.BLL.DTOMapping.Domain;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit;

public class GetObjectRevisionsMethodGenerator<TConfiguration>(TConfiguration configuration, Type domainType)
    : MethodGenerator<TConfiguration, BLLViewRoleAttribute>(configuration, domainType)
    where TConfiguration : class, IAuditGeneratorConfiguration<IAuditGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.GetRevisions;

    protected override string Name => $"Get{this.DomainType.Name}Revisions";

    protected override CodeTypeReference ReturnType => typeof(DefaultDomainObjectRevisionDTO).ToTypeReference();

    protected override bool IsEdit { get; } = false;


    protected override string GetComment() => $"Get {this.DomainType.Name} revisions";

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
