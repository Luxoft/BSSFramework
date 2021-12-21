using System;
using System.Linq;

using Framework.DomainDriven;
using Framework.DomainDriven.Generation.Domain;
using Framework.DomainDriven.NHibernate;
using Framework.Configuration.Domain;

using Framework.Persistent;

namespace Framework.Configuration.TestGenerate
{
    public partial class ServerGenerationEnvironment : GenerationEnvironmentBase
    {
        public readonly BLLCoreGeneratorConfiguration BLLCore;

        public readonly BLLGeneratorConfiguration BLL;

        public readonly ServerDTOGeneratorConfiguration ServerDTO;

        public readonly AuditDTOGeneratorConfiguration AuditDTO;

        public readonly MainServiceGeneratorConfiguration MainService;

        public readonly QueryServiceGeneratorConfiguration QueryService;

        public readonly DALGeneratorConfiguration DAL;

        public readonly AuditServiceGeneratorConfiguration AuditService;

        public ServerGenerationEnvironment()
            :this(new DatabaseName(typeof(PersistentDomainObjectBase).GetTargetSystemName()))
        {
        }

        public ServerGenerationEnvironment(DatabaseName databaseName)
        {
            this.BLLCore = new BLLCoreGeneratorConfiguration(this);

            this.BLL = new BLLGeneratorConfiguration(this);

            this.ServerDTO = new ServerDTOGeneratorConfiguration(this);

            this.MainService = new MainServiceGeneratorConfiguration(this);

            this.QueryService = new QueryServiceGeneratorConfiguration(this);

            this.DAL = new DALGeneratorConfiguration(this);

            this.AuditService = new AuditServiceGeneratorConfiguration(this);

            this.AuditDTO = new AuditDTOGeneratorConfiguration(this);

            this.DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
        }

        public DatabaseName DatabaseName { get; }

        public IMappingSettings MappingSettings => new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), this.DatabaseName, true);

        public IMappingSettings GetMappingSettings(DatabaseName dbName, AuditDatabaseName dbAuditName)
        {
            return new MappingSettings<PersistentDomainObjectBase>(this.DAL.GetMappingGenerators().Select(mg => mg.Generate()), dbName, dbAuditName);
        }

        public static readonly ServerGenerationEnvironment Default = new ServerGenerationEnvironment();
    }
}
