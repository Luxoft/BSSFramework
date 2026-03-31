using System.CodeDom;

using Framework.CodeDom.Extensions;
using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.CodeGeneration.ServiceModelGenerator.Configuration.Audit;
using Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit._Base;
using Framework.Core;

namespace Framework.CodeGeneration.ServiceModelGenerator.MethodGenerators.Audit;

public class GetObjectPropertyRevisionsByDateRangeMethodGenerator<TConfiguration>(
    TConfiguration configuration,
    Type domainType,
    IAuditDTOGeneratorConfiguration dtoConfiguration)
    : GetObjectPropertyRevisionsMethodGeneratorBase<TConfiguration>(configuration, domainType, dtoConfiguration)
    where TConfiguration : class, IAuditGeneratorConfiguration<IAuditGenerationEnvironment>
{
    public override MethodIdentity Identity { get; } = MethodIdentityType.GetPropertyRevisionByDateRange;

    protected override string Name => $"Get{this.DomainType.Name}PropertyRevisionByDateRange";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters() => base.GetParameters().Concat([this.PeriodParameter]);

    private CodeParameterDeclarationExpression PeriodParameter => new(typeof(Period?), "period");


    protected override string GetComment() => $"Get {this.DomainType.Name} Property Revisions by period";

    protected override IEnumerable<CodeExpression> GetBLLMethodParameters() => base.GetBLLMethodParameters().Concat([this.PeriodParameter.ToVariableReferenceExpression()]);
}
