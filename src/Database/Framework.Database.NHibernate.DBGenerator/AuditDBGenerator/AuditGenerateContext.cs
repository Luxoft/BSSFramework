using System.Data.Common;

using Framework.Database.NHibernate._MappingSettings;
using Framework.Database.NHibernate.DBGenerator.Contracts;

using NHibernate.Cfg;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;

namespace Framework.Database.NHibernate.DBGenerator.AuditDBGenerator;

struct AuditGenerateContext
{
    private readonly IDatabaseScriptGeneratorContext context;


    public string AuditScheme { get; private set; }
    public List<PersistentClass> PersistentClasses { get; private set; }
    public DatabaseMetadata AuditDatabaseMetadata { get; private set; }
    public DatabaseMetadata OriginalDatabaseMetadata { get; private set; }
    public global::NHibernate.Dialect.Dialect Dialect { get; private set; }
    public IMapping Mapping { get; private set; }
    public string DefaultCatalog { get; private set; }
    public Configuration Configuration { get; private set; }
    public DbConnection Connection { get; private set; }
    public ManagedProviderConnectionHelper AuditConnectionHelper { get; private set; }
    public ManagedProviderConnectionHelper OriginalConnectionHelper { get; private set; }
    public DbConnection OriginalConnection { get; private set; }
    public List<MappingSettings> MappingSettings { get; private set; }

    public IDatabaseScriptGeneratorContext Context
    {
        get { return this.context; }
    }

    public AuditGenerateContext(
            List<PersistentClass> auditClassMappings,
            DatabaseMetadata auditDatabaseMetadata,
            global::NHibernate.Dialect.Dialect dialect,
            IMapping mapping,
            string defaultCatalog,
            Configuration cfg,
            string auditScheme,
            List<MappingSettings> metadataProviders,
            IDatabaseScriptGeneratorContext context) : this()
    {
        this.context = context;
        this.MappingSettings = metadataProviders;
        if (auditScheme == null)
        {
            throw new ArgumentNullException(nameof(auditScheme));
        }

        this.AuditScheme = auditScheme;
        this.PersistentClasses = auditClassMappings;
        this.AuditDatabaseMetadata = auditDatabaseMetadata;

        this.Dialect = dialect;
        this.Mapping = mapping;
        this.DefaultCatalog = defaultCatalog;
        this.Configuration = cfg;

        this.AuditConnectionHelper = new ManagedProviderConnectionHelper(cfg.Properties);
        this.AuditConnectionHelper.Prepare();

        this.Connection = this.AuditConnectionHelper.Connection;

        this.OriginalConnectionHelper = new ManagedProviderConnectionHelper(cfg.Properties);
        this.OriginalConnectionHelper.Prepare();

        this.OriginalDatabaseMetadata = this.OriginalDatabaseMetadata;

        this.OriginalConnection = this.OriginalConnectionHelper.Connection;
        this.OriginalDatabaseMetadata = new DatabaseMetadata(this.OriginalConnection, dialect);
    }
}
