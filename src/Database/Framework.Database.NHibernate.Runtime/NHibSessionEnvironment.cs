using Anch.Core.Auth;
using Anch.GenericQueryable.NHibernate;

using Framework.Core;
using Framework.Database.NHibernate.Mapping;
using Framework.Database.NHibernate.SqlExceptionProcessors;

using Microsoft.Extensions.DependencyInjection;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Envers.Configuration;
using NHibernate.Tool.hbm2ddl;

namespace Framework.Database.NHibernate;

public class NHibSessionEnvironment : IDisposable
{
    public NHibSessionEnvironment(
            IEnumerable<MappingSettings> mappingSettings,
            IEnumerable<IConfigurationInitializer> initializers,
            [FromKeyedServices(ICurrentUser.DefaultKey)] ICurrentUser defaultCurrentUser,
            NHibSessionEnvironmentSettings settings,
            IDalValidationIdentitySource dalValidationIdentitySource)
    {
        var cachedMappingSettings = (mappingSettings ?? throw new ArgumentNullException(nameof(mappingSettings))).ToList();

        this.TransactionTimeout = settings.TransactionTimeout;

        try
        {
            this.RegisteredTypes = cachedMappingSettings.Select(ms => ms.PersistentDomainObjectBaseType).ToHashSet();

            foreach (var initializer in cachedMappingSettings.Select(ms => ms.Initializer).Concat(initializers))
            {
                initializer.Initialize(this.Configuration);
            }

            this.Configuration.SessionFactory().ParsingLinqThrough<VisitedNHibQueryProvider>();

            this.Configuration.InitializeAudit(cachedMappingSettings, defaultCurrentUser);

            SchemaMetadataUpdater.QuoteTableAndColumns(this.Configuration, global::NHibernate.Dialect.Dialect.GetDialect(this.Configuration.Properties));

            this.InternalSessionFactory = this.Configuration.BuildSessionFactory();

            this.InternalExceptionExpander = new SqlExceptionProcessorInterceptor(this.InternalSessionFactory, this.Configuration, dalValidationIdentitySource);
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Could not initialize {nameof(NHibSessionEnvironment)}.", ex);
        }
    }

    internal TimeSpan TransactionTimeout { get; }

    internal ISessionFactory InternalSessionFactory { get; }

    internal HashSet<Type> RegisteredTypes { get; }

    internal IExceptionExpander InternalExceptionExpander { get; }

    public Configuration Configuration { get; } = new ();

    /// <inheritdoc />
    public void Dispose()
    {
        using (this.InternalSessionFactory)
        {
            AuditConfiguration.Remove(this.Configuration);
        }
    }
}
