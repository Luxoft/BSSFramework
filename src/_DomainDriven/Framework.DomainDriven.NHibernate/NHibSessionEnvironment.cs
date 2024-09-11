#nullable enable

using System.Data;

using Framework.Core;
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
            IEnumerable<IMappingSettings> mappingSettings,
            IEnumerable<IConfigurationInitializer> initializers,
            IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService,
            INHibSessionEnvironmentSettings settings,
            IDalValidationIdentitySource dalValidationIdentitySource)
    {
        var cachedMappingSettings = mappingSettings.ToList();

        this.TransactionTimeout = settings.TransactionTimeout;

        if (cachedMappingSettings.SelectMany(z => new[] { z.Database, z.AuditDatabase }).Where(z => null != z).Distinct().Count() == 1)
        {
            throw new ArgumentException("All mapping settings has equal database with schema. Utilities has domain object with same names");
        }

        try
        {
            this.cfg = new Configuration();

            this.RegisteredTypes = cachedMappingSettings.ToHashSet(ms => ms.PersistentDomainObjectBaseType);

            foreach (var initializer in cachedMappingSettings.Select(ms => ms.Initializer).Concat(initializers))
            {
                initializer.Initialize(this.cfg);
            }

            this.Configuration.SessionFactory().ParsingLinqThrough<VisitedQueryProvider>();

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
