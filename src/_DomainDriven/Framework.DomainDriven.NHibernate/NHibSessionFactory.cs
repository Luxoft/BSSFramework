using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Framework.Core;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.NHibernate.SqlExceptionProcessors;
using Framework.Exceptions;

using JetBrains.Annotations;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Envers.Configuration;
using NHibernate.Tool.hbm2ddl;

namespace Framework.DomainDriven.NHibernate
{
    public class NHibSessionConfiguration : IDisposable
    {
        private static readonly TimeSpan DefaultTransactionTimeout = new TimeSpan(0, 20, 0);

        private readonly Configuration cfg;

        /// <summary> Creates new NH Session Factory
        /// </summary>
        /// <param name="connectionSettings">connection settings</param>
        /// <param name="mappingSettings">mapping settings</param>
        public NHibSessionConfiguration([NotNull] NHibConnectionSettings connectionSettings, [NotNull] IEnumerable<IMappingSettings> mappingSettings, IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService)
            : this(connectionSettings, mappingSettings, auditRevisionUserAuthenticationService, DefaultTransactionTimeout)
        {
        }

        /// <summary>
        /// Creates new NH Session Factory
        /// </summary>
        /// <param name="connectionSettings">connection settings</param>
        /// <param name="mappingSettings">mapping settings</param>
        /// <param name="transactionTimeout">transaction timeout</param>
        /// <exception cref="ArgumentNullException">
        /// connectionSettings
        /// or
        /// mappingSettings
        /// or
        /// availableValues
        /// or
        /// modifyAuditProperties
        /// or
        /// createAuditProperties
        /// </exception>
        /// <exception cref="System.ArgumentException">All mapping settings has equal database with schema. Utilities, Workflow has domain object with same names</exception>
        /// <exception cref="ApplicationException">Could not initialize ServiceFactory.</exception>
        public NHibSessionConfiguration(
            [NotNull] NHibConnectionSettings connectionSettings,
            [NotNull] IEnumerable<IMappingSettings> mappingSettings,
            IAuditRevisionUserAuthenticationService auditRevisionUserAuthenticationService,
            TimeSpan transactionTimeout)
        {
            this.ConnectionSettings = connectionSettings ?? throw new ArgumentNullException(nameof(connectionSettings));

            var cachedMappingSettings = (mappingSettings ?? throw new ArgumentNullException(nameof(mappingSettings))).ToList();

            this.TransactionTimeout = transactionTimeout;

            if (cachedMappingSettings.SelectMany(z => new[] { z.Database, z.AuditDatabase }).Where(z => null != z).Distinct().Count() == 1)
            {
                throw new System.ArgumentException("All mapping settings has equal database with schema. Utilities has domain object with same names");
            }

            try
            {
                this.cfg = new Configuration();

                this.RegisteredTypes = cachedMappingSettings.ToHashSet(ms => ms.PersistentDomainObjectBaseType);

                foreach (var ms in cachedMappingSettings)
                {
                    ms.InitMapping(this.cfg);
                }

                this.cfg.InitializeAudit(cachedMappingSettings, auditRevisionUserAuthenticationService);

                connectionSettings.Init(this.cfg);

                SchemaMetadataUpdater.QuoteTableAndColumns(this.cfg, Dialect.GetDialect(this.cfg.Properties));

                this.InternalSessionFactory = this.cfg.BuildSessionFactory();

                this.ExceptionProcessor = new SqlExceptionProcessorInterceptor(this.InternalSessionFactory, this.cfg);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Could not initialize ServiceFactory.", ex);
            }
        }

        [NotNull]
        public NHibConnectionSettings ConnectionSettings { get; }

        internal TimeSpan TransactionTimeout { get; }

        internal ISessionFactory InternalSessionFactory { get; }

        internal HashSet<Type> RegisteredTypes { get; }

        internal IExceptionProcessor ExceptionProcessor { get; }

        public Configuration Configuration => this.cfg;

        /// <summary>
        /// Process transaction created in Write session
        /// </summary>
        public virtual void ProcessTransaction(IDbTransaction dbTransaction)
        {
            // Do nothing
        }

        /// <summary>
        /// Backward compatibility for TransactionScope
        /// </summary>
        public bool EnableTransactionScope { get; set; } = true;

        /// <inheritdoc />
        public void Dispose()
        {
            AuditConfiguration.Remove(this.cfg);
            this.InternalSessionFactory.Close();
        }
    }
}
