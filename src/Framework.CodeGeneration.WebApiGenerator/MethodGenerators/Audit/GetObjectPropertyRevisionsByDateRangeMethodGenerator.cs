using System.CodeDom;

using Framework.CodeDom;
using Framework.CodeGeneration.DTOGenerator.Audit.Configuration;
using Framework.CodeGeneration.WebApiGenerator.Configuration.Audit;

using Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Audit._Base;
using Framework.Core;

namespace Framework.CodeGeneration.WebApiGenerator.MethodGenerators.Audit;

public class GetObjectPropertyRevisionsByDateRangeMethodGenerator<TConfiguration> : GetObjectPropertyRevisionsMethodGeneratorBase<TConfiguration>
        where TConfiguration : class, IAuditGeneratorConfigurationBase<IAuditGenerationEnvironmentBase>
{
    public GetObjectPropertyRevisionsByDateRangeMethodGenerator(TConfiguration configuration, Type domainType, IAuditDTOGeneratorConfigurationBase dtoConfiguration)
            : base(configuration, domainType, dtoConfiguration)
    {
    }

    public override MethodIdentity Identity { get; } = MethodIdentityType.GetPropertyRevisionByDateRange;

    protected override string Name => $"Get{this.DomainType.Name}PropertyRevisionByDateRange";

    protected override IEnumerable<CodeParameterDeclarationExpression> GetParameters()
    {
        return base.GetParameters().Concat([this.PeriodParameter]);
    }

    private CodeParameterDeclarationExpression PeriodParameter => new(typeof(Period?), "period");


    protected override string GetComment()
    {
        return $"Get {this.DomainType.Name} Property Revisions by period";
    }

    protected override IEnumerable<CodeExpression> GetBLLMethodParameters()
    {
        return base.GetBLLMethodParameters().Concat([this.PeriodParameter.ToVariableReferenceExpression()]);
    }

}
