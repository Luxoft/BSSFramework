using System.Data.Common;
using Framework.DomainDriven.DBGenerator.Contracts;

using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Engine;
using NHibernate.Mapping;
using NHibernate.Tool.hbm2ddl;

namespace Framework.DomainDriven.NHibernate;

struct AuditGenerateContext
{
    private readonly IDatabaseScriptGeneratorContext _context;


    public string AuditScheme { get; private set; }
    public IReadOnlyList<PersistentClass> PersistentClasses { get; private set; }
    public DatabaseMetadata AuditDatabaseMetadata { get; private set; }
    public DatabaseMetadata OriginalDatabaseMetadata { get; private set; }
    public Dialect Dialect { get; private set; }
    public IMapping Mapping { get; private set; }
    public string DefaultCatalog { get; private set; }
    public Configuration Configuration { get; private set; }
    public DbConnection Connection { get; private set; }
    public ManagedProviderConnectionHelper AuditConnectionHelper { get; private set; }
    public ManagedProviderConnectionHelper OriginalConnectionHelper { get; private set; }
    public DbConnection OriginalConnection { get; private set; }
    public IReadOnlyList<IMappingSettings> MappingSettings { get; private set; }

    public IDatabaseScriptGeneratorContext Context
    {
        get { return this._context; }
    }

    public AuditGenerateContext(
            IReadOnlyList<PersistentClass> auditClassMappings,
            DatabaseMetadata auditDatabaseMetadata,
            Dialect dialect,
            IMapping mapping,
            string defaultCatalog,
            Configuration cfg,
            string auditScheme,
            IReadOnlyList<IMappingSettings> metadataProviders,
            IDatabaseScriptGeneratorContext context) : this()
    {
        this._context = context;
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
