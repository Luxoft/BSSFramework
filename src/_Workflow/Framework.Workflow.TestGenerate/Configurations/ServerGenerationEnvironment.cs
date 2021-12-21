using System;
using System.Linq;

using Framework.DomainDriven;
using Framework.DomainDriven.NHibernate;
using Framework.Persistent;
using Framework.Workflow.Domain;

namespace Framework.Workflow.TestGenerate
{
    public partial class ServerGenerationEnvironment : GenerationEnvironmentBase
    {
        public readonly string ServiceContractNamespace = "http://workflow.luxoft.com/Facade";


        public readonly BLLCoreGeneratorConfiguration BLLCore;

        public readonly BLLGeneratorConfiguration BLL;

        public readonly ServerDTOGeneratorConfiguration ServerDTO;
        public readonly MainServiceGeneratorConfiguration MainService;

        public readonly DALGeneratorConfiguration DAL;

        public readonly ProjectionGeneratorConfiguration Projection;

        public ServerGenerationEnvironment()
            : this(new DatabaseName(typeof(PersistentDomainObjectBase).GetTargetSystemName()))
        {
        }

        public ServerGenerationEnvironment(DatabaseName databaseName)
        {
            this.BLLCore = new BLLCoreGeneratorConfiguration(this);

            this.BLL = new BLLGeneratorConfiguration(this);

            this.ServerDTO = new ServerDTOGeneratorConfiguration(this);

            this.MainService = new MainServiceGeneratorConfiguration(this);

            this.DAL = new DALGeneratorConfiguration(this);

            this.DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));

            this.Projection = new ProjectionGeneratorConfiguration(this);
        }


        public override Type SecurityOperationCodeType { get; } = typeof(WorkflowSecurityOperationCode);

        public override Type OperationContextType { get; } = typeof(WorkflowOperationContext);


        public DatabaseName DatabaseName { get; }


        public IMappingSettings MappingSettings => new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), this.DatabaseName, false);


        public IMappingSettings GetMappingSettings(DatabaseName dbName, AuditDatabaseName dbAuditName)
        {
            return new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), dbName, dbAuditName);
        }

        public IMappingSettings GetMappingSettingsWithoutAudit(DatabaseName dbName)
        {
            return new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), dbName, false);
        }


        public static readonly ServerGenerationEnvironment Default = new ServerGenerationEnvironment();
    }
}
