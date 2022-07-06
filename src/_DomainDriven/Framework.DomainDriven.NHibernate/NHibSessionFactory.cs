using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Audit;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.DomainDriven.NHibernate.SqlExceptionProcessors;
using Framework.Exceptions;

using JetBrains.Annotations;

using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NHibernate.Envers.Configuration;
using NHibernate.Event;
using NHibernate.Tool.hbm2ddl;

namespace Framework.DomainDriven.NHibernate
{
    public class NHibSessionFactory : IDBSessionFactory
    {
        private static readonly TimeSpan DefaultTransactionTimeout = new TimeSpan(0, 20, 0);

        private readonly Configuration cfg;

        /// <summary> Creates new NH Session Factory
        /// </summary>
        /// <param name="connectionSettings">conntection settings</param>
        /// <param name="mappingSettings">mapping settings</param>
        public NHibSessionFactory([NotNull] NHibConnectionSettings connectionSettings, [NotNull] IEnumerable<IMappingSettings> mappingSettings, IUserAuthenticationService userAuthenticationService, IDateTimeService dateTimeService)
            : this(connectionSettings, mappingSettings, DefaultTransactionTimeout, userAuthenticationService, dateTimeService)
        {
        }

        /// <summary> Creates new NH Session Factory
        /// </summary>
        /// <param name="connectionSettings">connectino settings</param>
        /// <param name="mappingSettings">mapping settings</param>
        /// <param name="transactionTimeout">transaction timeout</param>
        public NHibSessionFactory([NotNull] NHibConnectionSettings connectionSettings, [NotNull] IEnumerable<IMappingSettings> mappingSettings, TimeSpan transactionTimeout, IUserAuthenticationService userAuthenticationService, IDateTimeService dateTimeService)
            : this(connectionSettings, mappingSettings, transactionTimeout, AvailableValuesHelper.AvailableValues, AuditPropertyPair.GetModifyAuditProperty(userAuthenticationService, dateTimeService), AuditPropertyPair.GetCreateAuditProperty(userAuthenticationService, dateTimeService), userAuthenticationService)
        {
        }

        /// <summary>
        /// Creates new NH Session Factory
        /// </summary>
        /// <param name="connectionSettings">connectino settings</param>
        /// <param name="mappingSettings">mapping settings</param>
        /// <param name="transactionTimeout">transaction timeout</param>
        /// <param name="availableValues">different data types' value limits. Goes to <see cref="AvailableValues" /></param>
        /// <param name="modifyAuditProperties">modify audit properties</param>
        /// <param name="createAuditProperties">create audit properties</param>
        /// <param name="userAuthenticationService">The user authentication service.</param>
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
        /// <exception cref="System.ArgumentException">All mapping settings has equal database with schema. Utilities, Worflow has domain object with same names</exception>
        /// <exception cref="ApplicationException">Could not initialize ServiceFactory.</exception>
        public NHibSessionFactory(
            [NotNull] NHibConnectionSettings connectionSettings,
            [NotNull] IEnumerable<IMappingSettings> mappingSettings,
            TimeSpan transactionTimeout,
            [NotNull] AvailableValues availableValues,
            [NotNull] IEnumerable<IAuditProperty> modifyAuditProperties,
            [NotNull] IEnumerable<IAuditProperty> createAuditProperties,
            [NotNull] IUserAuthenticationService userAuthenticationService)
        {
            if (connectionSettings == null)
            {
                throw new ArgumentNullException(nameof(connectionSettings));
            }

            if (mappingSettings == null)
            {
                throw new ArgumentNullException(nameof(mappingSettings));
            }

            if (availableValues == null)
            {
                throw new ArgumentNullException(nameof(availableValues));
            }

            if (modifyAuditProperties == null)
            {
                throw new ArgumentNullException(nameof(modifyAuditProperties));
            }

            if (createAuditProperties == null)
            {
                throw new ArgumentNullException(nameof(createAuditProperties));
            }

            this.AvailableValues = availableValues;
            this.TransactionTimeout = transactionTimeout;

            var cachedMappingSettings = mappingSettings.ToList();

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

                this.cfg.InitializeAudit(cachedMappingSettings, userAuthenticationService);

#pragma warning disable 0618 // Obsolete
                if (connectionSettings.UseEventListenerInsteadOfInterceptorForAudit)
                {
                    var modifyAuditEventListener = new ModifyAuditEventListener(modifyAuditProperties);
                    var createAuditEventListener = new CreateAuditEventListener(createAuditProperties);
#pragma warning restore 0618

                    this.cfg.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[] { modifyAuditEventListener };
                    this.cfg.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[] { modifyAuditEventListener, createAuditEventListener };
                }
                else
                {
                    this.cfg.SetInterceptor(new AuditInterceptor(createAuditProperties, modifyAuditProperties));
                }

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

        /// <summary> Different data types' value limits
        /// </summary>
        public AvailableValues AvailableValues { get; }

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
        public bool EnableTransactionScope
        {
            get;
            set;
        } = true;

        protected internal void OnSessionFlushed([NotNull] SessionFlushedEventArgs e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            this.SessionFlushed.Maybe(handler => handler(this, e));
        }

        protected internal virtual void OnSessionAfterTransactionCompleted([NotNull] SessionFlushedEventArgs e)
        {
            if (e == null) { throw new ArgumentNullException(nameof(e)); }

            this.SessionAfterTransactionCompleted?.Invoke(this, e);
        }

        protected internal virtual void OnSessionBeforeTransactionCompleted([NotNull] SessionFlushedEventArgs e)
        {
            if (e == null) { throw new ArgumentNullException(nameof(e)); }

            this.SessionBeforeTransactionCompleted?.Invoke(this, e);
        }

        /// <summary> Creates DB session
        /// </summary>
        /// <param name="openMode">session's mode</param>
        /// <returns>New DB session</returns>
        public IDBSession Create(DBSessionMode openMode)
        {
            if (DBSessionMode.Read == openMode)
            {
                return new ReadOnlyNHibSession(this);
            }

            return new WriteNHibSession(this);
        }

        /// <summary> Session flushed event
        /// </summary>
        public event EventHandler<SessionFlushedEventArgs> SessionFlushed;

        /// <summary> Transaction completed event
        /// </summary>
        [Obsolete("Use SessionAfterTransactionCompleted", true)]
        public event EventHandler<SessionFlushedEventArgs> SessionTransactionCompleted;

        public event EventHandler<SessionFlushedEventArgs> SessionBeforeTransactionCompleted;

        public event EventHandler<SessionFlushedEventArgs> SessionAfterTransactionCompleted;

        /// <inheritdoc />
        public void Dispose()
        {
            AuditConfiguration.Remove(this.cfg);
            this.InternalSessionFactory.Close();
        }
    }
}
