using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;

using Framework.CodeDom;
using Framework.Core;
using Framework.DomainDriven.DTOGenerator.Audit;

namespace Framework.DomainDriven.ServiceModelGenerator
{
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
            return base.GetParameters().Concat(new[] { this.PeriodParameter });
        }

        private CodeParameterDeclarationExpression PeriodParameter => new CodeParameterDeclarationExpression(typeof(Period?), "period");


        protected override string GetComment()
        {
            return $"Get {this.DomainType.Name} Property Revisions by period";
        }

        protected override IEnumerable<CodeExpression> GetBLLMethodParameters()
        {
            return base.GetBLLMethodParameters().Concat(new[] { this.PeriodParameter.ToVariableReferenceExpression() });
        }

    }
}
