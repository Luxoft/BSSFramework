using Framework.DomainDriven.DTOGenerator.Audit;
using Framework.DomainDriven.ServiceModelGenerator;

namespace Framework.Configuration.TestGenerate
{
    public class AuditDTOGeneratorConfiguration : AuditDTOGeneratorConfigurationBase<ServerGenerationEnvironment>
    {
        private const string RevisionDTOName = "Configuration";


        public AuditDTOGeneratorConfiguration(ServerGenerationEnvironment environment)
            : base(environment)
        {
        }

        public override string Namespace => this.Environment.AuditService.Namespace;

        protected override string DomainObjectPropertyRevisionsDTOPrefixName => RevisionDTOName;

        protected override string PropertyRevisionDTOPrefixName => RevisionDTOName;
    }
}
