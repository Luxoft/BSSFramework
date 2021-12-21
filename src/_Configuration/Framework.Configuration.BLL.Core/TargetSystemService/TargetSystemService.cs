using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Framework.Configuration.BLL.SubscriptionSystemService3;
using Framework.Configuration.BLL.SubscriptionSystemService3.Services;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Configuration.Domain;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Events;
using Framework.Exceptions;
using Framework.Notification;
using Framework.Persistent;
using Framework.SecuritySystem;

using JetBrains.Annotations;

using Serilog;

namespace Framework.Configuration.BLL
{
    public class TargetSystemService<TBLLContext, TPersistentDomainObjectBase> : TargetSystemService<TBLLContext>, ITargetSystemService<TBLLContext, TPersistentDomainObjectBase>, IPersistentTargetSystemService

        where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase>>, IDefaultBLLContext<TPersistentDomainObjectBase, Guid>, IBLLOperationEventContext<TPersistentDomainObjectBase>
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
    {
        private readonly IEnumerable<IManualEventDALListener<TPersistentDomainObjectBase>> eventDalListeners;

        private readonly IRevisionSubscriptionSystemService subscriptionService;

        private readonly Lazy<bool> lazyHasAttachments;


        /// <summary>
        /// Создаёт экземпляр класса <see cref="TargetSystemService{TBLLContext, TPersistentDomainObjectBase}" />.
        /// </summary>
        /// <param name="context">Контекст конфигурации.</param>
        /// <param name="targetSystemContext">Контекст целевой системы.</param>
        /// <param name="targetSystem">Целевая система.</param>
        /// <param name="subscriptionMetadataStore">Хранилище описаний подписок.</param>
        /// <param name="eventDalListeners">DAL-подписчики для пробразсывания евентов</param>
        public TargetSystemService(
            IConfigurationBLLContext context,
            TBLLContext targetSystemContext,
            TargetSystem targetSystem,
            IEnumerable<IManualEventDALListener<TPersistentDomainObjectBase>> eventDalListeners,
            SubscriptionMetadataStore subscriptionMetadataStore = null)
            : base(context, targetSystemContext, targetSystem, targetSystemContext.FromMaybe(() => new ArgumentNullException(nameof(targetSystemContext))).TypeResolver)
        {
            this.eventDalListeners = (eventDalListeners ?? throw new ArgumentNullException(nameof(eventDalListeners)));

            this.subscriptionService = this.GetSubscriptionService(
                Log.ForContext(this.GetType()),
                context.SubscriptionSender,
                subscriptionMetadataStore);

            this.lazyHasAttachments = LazyHelper.Create(() => this.Context.Logics.Attachment.GetUnsecureQueryable().Any(a => a.Container.DomainType.TargetSystem == this.TargetSystem));
        }


        public Type PersistentDomainObjectBaseType => typeof(TPersistentDomainObjectBase);

        public bool HasAttachments => this.lazyHasAttachments.Value;

        public override ISubscriptionSystemService SubscriptionService => this.subscriptionService;


        public void TryRemoveAttachments<TDomainObject>([NotNull] IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var idents = domainObjects.ToList(obj => obj.Id);

            if (!idents.Any())
            {
                return;
            }

            var domainType = this.Context.GetDomainType(typeof(TDomainObject), false);

            if (domainType == null)
            {
                return;
            }

            const int maxSqlParametersCount = 2000;
            var idsParts = idents.Split(maxSqlParametersCount);

            var logic = this.Context.Logics.AttachmentContainer;

            foreach (var ids in idsParts)
            {
                var containers = logic.GetUnsecureQueryable()
                    .Where(ac => ac.DomainType == domainType && ids.Contains(ac.ObjectId)).ToList();

                logic.Remove(containers);
            }
        }

        public IRevisionSubscriptionSystemService GetSubscriptionService(IMessageSender<MessageTemplateNotification> subscriptionSender)
        {
            return this.subscriptionService;
        }

        public void ForceEvent([NotNull] DomainTypeEventOperation operation, long? revision, Guid domainObjectId)
        {
            if (operation == null) throw new ArgumentNullException(nameof(operation));
            if (domainObjectId.IsDefault()) throw new ArgumentOutOfRangeException(nameof(domainObjectId));

            var domainType = this.TypeResolver.Resolve(operation.DomainType);

            var operationType = domainType.GetEventOperationType(true);

            new Action<string, long?, Guid>(this.ForceEvent<TPersistentDomainObjectBase, TypeCode>)
           .CreateGenericMethod(domainType, operationType)
           .Invoke(this, new object[] { operation.Name, revision, domainObjectId });
        }

        private void ForceEvent<TDomainObject, TOperation>(string operationName, long? revision, Guid domainObjectId)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TOperation : struct, Enum
        {
            var bll = this.TargetSystemContext.Logics.Default.Create<TDomainObject>();

            var domainObject = revision == null ? bll.GetById(domainObjectId, true)
                                                : bll.GetObjectByRevision(domainObjectId, revision.Value);

            var operation = EnumHelper.Parse<TOperation>(operationName);

            var listener = this.TargetSystemContext.OperationListeners.GetEventListener<TDomainObject, TOperation>();

            listener.ForceEvent(domainObject, operation);

            operation.ToOperationMaybe<TOperation, EventOperation>().Match(
                eventOperation =>
                    this.eventDalListeners.Foreach(dalListener => dalListener.GetForceEventContainer<TDomainObject>().ForceEvent(domainObject, eventOperation)));
        }

        public override bool IsAssignable(Type domainType)
        {
            return typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainType);
        }

        public void TryDenormalizeHasAttachmentFlag([NotNull] AttachmentContainer container, bool value)
        {
            if (container == null) throw new ArgumentNullException(nameof(container));

            new DenormalizeHasAttachmentFlagProcessor(this.TargetSystemContext, container.ObjectId, value).Process(container.DomainType.Name);
        }

        public IEnumerable<Guid> GetNotExistsObjects(DomainType domainType, IEnumerable<Guid> idents)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));
            if (idents == null) throw new ArgumentNullException(nameof(idents));

            var domainObjectType = this.TypeResolver.Resolve(domainType, true);

            var method = new Func<IEnumerable<Guid>, IEnumerable<Guid>>(this.GetNotExistsObjects<TPersistentDomainObjectBase>)
                        .CreateGenericMethod(domainObjectType);


            return (IEnumerable<Guid>)method.Invoke(this, new object[] { idents });
        }


        private IRevisionSubscriptionSystemService GetSubscriptionService(
            ILogger logger,
            IMessageSender<MessageTemplateNotification> subscriptionSender,
            SubscriptionMetadataStore subscriptionMetadataStore)
        {
            if (!this.IsNewSubscriptionServiceRequired())
            {
                return new DBSubscription.RevisionSubscriptionService<TBLLContext, TPersistentDomainObjectBase>(
                    this.Context,
                    this,
                    logger,
                    subscriptionSender);
            }

            if (subscriptionMetadataStore == null)
            {
                throw new InvalidOperationException("SubscriptionMetadataStore instance can not be null for use new subscription services.");
            }

            var subscriptionServicesFactory = new SubscriptionServicesFactory<TBLLContext, TPersistentDomainObjectBase>(
                this.Context,
                this.TargetSystemContext.Logics.Default,
                this.TargetSystemContext,
                subscriptionMetadataStore);

            return new RevisionSubscriptionSystemService<TBLLContext, TPersistentDomainObjectBase>(subscriptionServicesFactory);
        }

        private IEnumerable<Guid> GetNotExistsObjects<TDomainObject>(IEnumerable<Guid> idents)
            where TDomainObject : class, TPersistentDomainObjectBase
        {
            if (idents == null) throw new ArgumentNullException(nameof(idents));

            var cachedIdents = idents.ToList();

            return this.TargetSystemContext.Logics.Default
                                                  .Create<TDomainObject>()
                                                  .GetUnsecureQueryable()
                                                  .Where(obj => !cachedIdents.Contains(obj.Id))
                                                  .Select(obj => obj.Id);
        }


        public ISecurityProvider<TDomainObject> GetAttachmentSecurityProvider<TDomainObject>(Expression<Func<TDomainObject, AttachmentContainer>> containerPath, DomainType mainDomainType, BLLSecurityMode securityMode)
            where TDomainObject : PersistentDomainObjectBase
        {
            if (containerPath == null) throw new ArgumentNullException(nameof(containerPath));
            if (mainDomainType == null) throw new ArgumentNullException(nameof(mainDomainType));

            return new AttachmentSecurityService<TBLLContext, TPersistentDomainObjectBase>(this.Context, this.TargetSystemContext)
                  .GetAttachmentSecurityProvider(containerPath, mainDomainType, securityMode);
        }


        void IPersistentTargetSystemService.TryRemoveAttachments(Array domainObjects)
        {
            if (domainObjects == null) throw new ArgumentNullException(nameof(domainObjects));

            var domainObjectType = domainObjects.GetElementType();

            if (!typeof(TPersistentDomainObjectBase).IsAssignableFrom(domainObjectType))
            {
                throw new BusinessLogicException("Domain Type {0} must be derived from {1}", domainObjectType.Name, typeof(TPersistentDomainObjectBase).Name);
            }

            var method = new Action<TPersistentDomainObjectBase[]>(this.TryRemoveAttachments)
                .Method
                .GetGenericMethodDefinition()
                .MakeGenericMethod(domainObjectType);

            method.Invoke(this, new object[] { domainObjects });
        }

        IRevisionSubscriptionSystemService IPersistentTargetSystemService.SubscriptionService
        {
            get { return this.subscriptionService; }
        }

        private class DenormalizeHasAttachmentFlagProcessor : TypeResolverDomainObjectProcessor<TBLLContext, TPersistentDomainObjectBase>
        {
            private readonly Guid _objectId;
            private readonly bool _value;


            public DenormalizeHasAttachmentFlagProcessor(TBLLContext context, Guid objectId, bool value)
                : base(context)
            {
                this._objectId = objectId;
                this._value = value;
            }


            protected override void Process<TDomainObject>()
            {
                var bll = this.Context.Logics.Default.Create<TDomainObject>();

                var domainObject = bll.GetById(this._objectId);

                if (domainObject is IAttachmentContainerHeader)
                {
                    (domainObject as IAttachmentContainerHeader).HasAttachments = this._value;

                    bll.Save(domainObject);
                }
            }
        }
    }



    public abstract class TargetSystemService<TBLLContext> : BLLContextContainer<IConfigurationBLLContext>, ITargetSystemService<TBLLContext>

        where TBLLContext : class
    {
        protected TargetSystemService(
            IConfigurationBLLContext context,
            TBLLContext targetSystemContext,
            TargetSystem targetSystem,
            ITypeResolver<string> typeResolver)
            : base(context)
        {
            this.TargetSystemContext = targetSystemContext ?? throw new ArgumentNullException(nameof(targetSystemContext));
            this.TargetSystem = targetSystem ?? throw new ArgumentNullException(nameof(targetSystem));

            this.TypeResolverS = typeResolver ?? throw new ArgumentNullException(nameof(typeResolver));
            this.TypeResolver = typeResolver.OverrideInput((DomainType domainType) => domainType.FullTypeName).WithCache().WithLock();
        }



        public string Name
        {
            get { return this.TargetSystem.Name; }
        }

        public TBLLContext TargetSystemContext { get; private set; }

        public TargetSystem TargetSystem { get; private set; }

        public ITypeResolver<string> TypeResolverS { get; private set; }

        public ITypeResolver<DomainType> TypeResolver { get; private set; }

        public abstract ISubscriptionSystemService SubscriptionService { get; }


        public void ExecuteRegularJob(RegularJob job)
        {
            var del = this.Context.ExpressionParsers.GetRegularJobFunctionExpressionParser<TBLLContext>().GetDelegate(job);

            del(this.TargetSystemContext);
        }

        public void ExecuteBLLContextLambda(ILambdaObject lambdaObject)
        {
            var del = this.Context.ExpressionParsers.GetRegularJobFunctionExpressionParser<TBLLContext>().GetDelegate(lambdaObject.Value);

            del(this.TargetSystemContext);
        }

        public void ValidateRegularJob(RegularJob job)
        {
            this.Context.ExpressionParsers.GetRegularJobFunctionExpressionParser<TBLLContext>().Validate(job);
        }

        public abstract bool IsAssignable(Type domainType);


        object ITargetSystemService.TargetSystemContext
        {
            get { return this.TargetSystemContext; }
        }

        Type ITargetSystemService.TargetSystemContextType
        {
            get { return typeof(TBLLContext); }
        }

        /// <summary>
        /// Required flag for 'code first' subscriptions
        /// </summary>
        protected virtual bool IsNewSubscriptionServiceRequired()
        {
            const string key = "iad.framework.useNewSubscriptionRevisionService";
            var setting = ConfigurationManagerHelper.GetAppSettings(key, false);
            bool useNewService;

            if (!bool.TryParse(setting, out useNewService))
            {
                useNewService = false;
            }

            return useNewService;
        }
    }
}
