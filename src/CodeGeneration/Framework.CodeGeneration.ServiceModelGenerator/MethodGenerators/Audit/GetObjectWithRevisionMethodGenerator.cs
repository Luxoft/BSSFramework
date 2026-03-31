using System.CodeDom;

using CommonFramework;

using Framework.BLL.Domain.DTO;
using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;
using Framework.CodeGeneration.ServiceModelGenerator.Extensions;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Main.View._Base;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit;

public class GetObjectWithRevisionMethodGenerator<TConfiguration> : ViewMethodGenerator<TConfiguration>
        where TConfiguration : class, IAuditGeneratorConfigurationBase<IAuditGenerationEnvironmentBase>
{
    public GetObjectWithRevisionMethodGenerator(TConfiguration configuration, Type domainType, ViewDTOType dtoType)
            : base(configuration, domainType, dtoType) =>
        this.Identity = new MethodIdentity(MethodIdentityType.GetRevision, this.DTOType);

    public override MethodIdentity Identity { get; }

    protected override string Name => this.CreateName(false, "Revision", "With");

    protected override CodeTypeReference ReturnType => this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, this.DTOType);

    private CodeParameterDeclarationExpression RevisionParameter => new(typeof (long), "revision");

    protected override string GetComment() => $"Get {this.DomainType.Name} ({this.DTOType}) by revision";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        yield return this.Configuration.Environment.ServerDTO.GetCodeTypeReference(this.DomainType, Framework.BLL.Domain.DTO.DTOType.IdentityDTO)
                         .ToParameterDeclarationExpression(this.DomainType.Name.ToStartLowerCase() + "Identity");

        yield return this.RevisionParameter;
    }


    protected override IEnumerable<CodeStatement> GetFacadeMethodInternalStatements(CodeExpression evaluateDataExpr, CodeExpression bllRefExpr)
    {
        var domainObjectDecl = this.ToDomainObjectVarDecl(bllRefExpr.ToMethodInvokeExpression("GetObjectByRevision",

                                                              this.Parameter.ToVariableReferenceExpression().ToPropertyReference(this.Configuration.Environment.IdentityProperty), this.RevisionParameter.ToVariableReferenceExpression()));

        yield return domainObjectDecl;

        yield return domainObjectDecl.ToVariableReferenceExpression()
                                     .Pipe(source => this.ConvertToDTO(source, evaluateDataExpr.GetMappingService()))
                                     .ToMethodReturnStatement();
    }
}
