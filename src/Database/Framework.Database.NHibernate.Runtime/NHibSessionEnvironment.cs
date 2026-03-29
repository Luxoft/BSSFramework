using CommonFramework.Auth;
using Framework.Core;
using Framework.Database.NHibernate._MappingSettings;
using Framework.Database.NHibernate.Audit;
using Framework.Database.NHibernate.SqlExceptionProcessors;

using GenericQueryable.NHibernate;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Envers.Configuration;
using NHibernate.Tool.hbm2ddl;

namespace Framework.Database.NHibernate;

public class NHibSessionEnvironment : IDisposable
{
    private readonly Configuration cfg;

    public NHibSessionEnvironment(
            IEnumerable<MappingSettings> mappingSettings,
            IEnumerable<IConfigurationInitializer> initializers,
            [FromKeyedServices(ICurrentUser.DefaultKey)]ICurrentUser defaultCurrentUser,
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

            this.cfg.InitializeAudit(cachedMappingSettings, defaultCurrentUser);

            SchemaMetadataUpdater.QuoteTableAndColumns(this.cfg, global::NHibernate.Dialect.Dialect.GetDialect(this.cfg.Properties));

            this.InternalSessionFactory = this.cfg.BuildSessionFactory();

            this.InternalExceptionExpander = new SqlExceptionProcessorInterceptor(this.InternalSessionFactory, this.cfg, dalValidationIdentitySource);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Could not initialize ServiceFactory.", ex);
        }
    }

    internal TimeSpan TransactionTimeout { get; }

    internal ISessionFactory InternalSessionFactory { get; }

    internal HashSet<Type> RegisteredTypes { get; }

    internal IExceptionExpander InternalExceptionExpander { get; }

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
