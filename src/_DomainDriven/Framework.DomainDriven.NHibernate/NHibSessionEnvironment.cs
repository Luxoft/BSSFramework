using System.Linq;
using Framework.DomainDriven.DALExceptions;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.NHibernate.SqlExceptionProcessors;
using Framework.Exceptions;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Envers.Configuration;
using NHibernate.Tool.hbm2ddl;

namespace Framework.DomainDriven.NHibernate;

public class NHibSessionEnvironment : IDisposable
{
    private readonly Configuration cfg;

    public NHibSessionEnvironment(
            IEnumerable<MappingSettings> mappingSettings,
            IEnumerable<IConfigurationInitializer> initializers,
            IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService,
            INHibSessionEnvironmentSettings settings,
            IDalValidationIdentitySource dalValidationIdentitySource)
    {
        var cachedMappingSettings = (mappingSettings ?? throw new ArgumentNullException(nameof(mappingSettings))).ToList();

        this.TransactionTimeout = settings.TransactionTimeout;

        try
        {
            this.cfg = new Configuration();

            this.RegisteredTypes = cachedMappingSettings.Select(ms => ms.PersistentDomainObjectBaseType).ToHashSet();

            foreach (var initializer in cachedMappingSettings.Select(ms => ms.Initializer).Concat(initializers))
            {
                initializer.Initialize(this.cfg);
            }

            this.Configuration.SessionFactory().ParsingLinqThrough<VisitedNHibQueryProvider>();

            this.cfg.InitializeAudit(cachedMappingSettings, auditRevisionUserAuthenticationService);

            SchemaMetadataUpdater.QuoteTableAndColumns(this.cfg, Dialect.GetDialect(this.cfg.Properties));

            this.InternalSessionFactory = this.cfg.BuildSessionFactory();

            this.ExceptionProcessor = new SqlExceptionProcessorInterceptor(this.InternalSessionFactory, this.cfg, dalValidationIdentitySource);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Could not initialize ServiceFactory.", ex);
        }
    }

    internal TimeSpan TransactionTimeout { get; }

    internal ISessionFactory InternalSessionFactory { get; }

    internal HashSet<Type> RegisteredTypes { get; }

    internal IExceptionProcessor ExceptionProcessor { get; }

    public Configuration Configuration => this.cfg;

    /// <inheritdoc />
    public void Dispose()
    {
        using (this.InternalSessionFactory)
        {
            AuditConfiguration.Remove(this.cfg);
        }
    }
}
