using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.SecuritySystem.Rules.Builders;
using Framework.DomainDriven.BLL.Tracking;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.QueryLanguage;
using Framework.SecuritySystem;
using Framework.Validation;
using Framework.Workflow.Domain;
using Framework.Workflow.Domain.Definition;

using JetBrains.Annotations;

namespace Framework.Workflow.BLL
{
    public partial class WorkflowBLLContext
    {
        private readonly Lazy<IWorkflowSecurityService> _lazySecurityService;

        private readonly Lazy<IWorkflowBLLFactoryContainer> _lazyLogics;

        private readonly Lazy<Dictionary<TargetSystem, ITargetSystemService>> _lazyTargetSystemServiceCache;

        private readonly Func<string, IWorkflowBLLContext> _impersonateFunc;

        private readonly IDictionaryCache<StateBase, StateBase> _stateBaseCache;

        private readonly IDictionaryCache<Event, Event> _eventCache;

        private readonly IDictionaryCache<Type, DomainType> _domainTypeCache;

        public WorkflowBLLContext(
            IServiceProvider serviceProvider,
            [NotNull]IDALFactory<PersistentDomainObjectBase, Guid> dalFactory,
            [NotNull]BLLOperationEventListenerContainer<DomainObjectBase> operationListeners,
            [NotNull]BLLSourceEventListenerContainer<PersistentDomainObjectBase> sourceListeners,
            [NotNull]IObjectStateService objectStateService,
            [NotNull]IAccessDeniedExceptionService<PersistentDomainObjectBase> accessDeniedExceptionService,
            [NotNull]IStandartExpressionBuilder standartExpressionBuilder,
            [NotNull]IValidator validator,
            [NotNull]IHierarchicalObjectExpanderFactory<Guid> hierarchicalObjectExpanderFactory,
            [NotNull]IFetchService<PersistentDomainObjectBase, FetchBuildRule> fetchService,
            [NotNull]IDateTimeService dateTimeService,
            [NotNull]ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> securityExpressionBuilderFactory,
            [NotNull]IConfigurationBLLContext configuration,
            [NotNull]IAuthorizationBLLContext authorization,
            [NotNull]Func<IWorkflowSecurityService> getSecurityService,
            [NotNull]Func<IWorkflowBLLFactoryContainer> getLogics,
            [NotNull]IExpressionParserFactory expressionParsers,
            [NotNull]IAnonymousTypeBuilder<TypeMap<ParameterizedTypeMapMember>> anonymousTypeBuilder,
            [NotNull]IEnumerable<ITargetSystemService> targetSystemServices,
            [NotNull]Func<string, IWorkflowBLLContext> impersonateFunc,
            [NotNull]IValidator anonymousObjectValidator)

            : base(serviceProvider, dalFactory, operationListeners, sourceListeners, objectStateService, accessDeniedExceptionService, standartExpressionBuilder, validator, hierarchicalObjectExpanderFactory, fetchService, dateTimeService)
        {
            if (securityExpressionBuilderFactory == null) throw new ArgumentNullException(nameof(securityExpressionBuilderFactory));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (authorization == null) throw new ArgumentNullException(nameof(authorization));

            if (getSecurityService == null) throw new ArgumentNullException(nameof(getSecurityService));
            if (getLogics == null) throw new ArgumentNullException(nameof(getLogics));

            if (expressionParsers == null) throw new ArgumentNullException(nameof(expressionParsers));
            if (anonymousTypeBuilder == null) throw new ArgumentNullException(nameof(anonymousTypeBuilder));

            if (impersonateFunc == null) throw new ArgumentNullException(nameof(impersonateFunc));

            this.SecurityExpressionBuilderFactory = securityExpressionBuilderFactory;

            this._lazyLogics = getLogics.ToLazy();
            this._lazySecurityService = getSecurityService.ToLazy();

            this.Configuration = configuration;
            this.Authorization = authorization;

            this.ExpressionParsers = expressionParsers;

            this.AnonymousTypeBuilder = anonymousTypeBuilder;
            this._lazyTargetSystemServiceCache = LazyHelper.Create(() => targetSystemServices.ToDictionary(s => s.TargetSystem, s => s));


            this._impersonateFunc = impersonateFunc;

            this._stateBaseCache = new WorkflowNestedCache<StateBase>(this).WithLock();
            this._eventCache = new WorkflowNestedCache<Event>(this).WithLock();
            this._domainTypeCache = new DictionaryCache<Type, DomainType>(domainObjectType => this.Logics.DomainType.GetByType(domainObjectType)).WithLock();

            this.AnonymousObjectValidator = anonymousObjectValidator;
        }


        public ITypeResolver<string> TypeResolver { get; } = TypeSource.FromSample<PersistentDomainObjectBase>().ToDefaultTypeResolver();


        public IAnonymousTypeBuilder<TypeMap<ParameterizedTypeMapMember>> AnonymousTypeBuilder
        {
            get; private set;
        }

        public IWorkflowSecurityService SecurityService
        {
            get { return this._lazySecurityService.Value; }
        }

        public IConfigurationBLLContext Configuration { get; }


        public IEnumerable<ITargetSystemService> GetTargetSystemServices()
        {
            return this._lazyTargetSystemServiceCache.Value.Values;
        }

        public StateBase GetNestedStateBase(StateBase stateBase)
        {
            if (stateBase == null) throw new ArgumentNullException(nameof(stateBase));

            return this._stateBaseCache[stateBase];
        }

        public Event GetNestedEvent(Event @event)
        {
            if (@event == null) throw new ArgumentNullException(nameof(@event));

            return this._eventCache[@event];
        }

        public DomainType GetDomainType(Type type)
        {
            return this._domainTypeCache[type];
        }

        public IValidator AnonymousObjectValidator { get; }



        public override IWorkflowBLLFactoryContainer Logics => this._lazyLogics.Value;

        public IAuthorizationBLLContext Authorization { get; }

        public ISecurityExpressionBuilderFactory<PersistentDomainObjectBase, Guid> SecurityExpressionBuilderFactory { get; }


        public IExpressionParserFactory ExpressionParsers { get; }


        public IWorkflowBLLContext Impersonate(string principalName)
        {
            return this._impersonateFunc(principalName);
        }



        public ITargetSystemService GetTargetSystemService([NotNull] TargetSystem targetSystem)
        {
            if (targetSystem == null) throw new ArgumentNullException(nameof(targetSystem));

            return this._lazyTargetSystemServiceCache.Value[targetSystem];
        }

        public ITargetSystemService GetTargetSystemService(string targetSystemName)
        {
            return this._lazyTargetSystemServiceCache.Value.Values.Single(service => service.TargetSystem.Name.Equals(targetSystemName, StringComparison.CurrentCultureIgnoreCase),
                () => new BusinessLogicException($"Target System with name {targetSystemName} not found"),
                () => new BusinessLogicException($"To many Target Systems with name {targetSystemName}"));
        }

        public ITargetSystemService GetTargetSystemService(Type domainType, bool throwOnNotFound)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            if (throwOnNotFound)
            {
                return this._lazyTargetSystemServiceCache.Value.Values.Single(service => service.PersistentDomainObjectBaseType.IsAssignableFrom(domainType),
                    () => new BusinessLogicException($"Target System for type {domainType.Name} not found"),
                    () => new BusinessLogicException($"To many Target Systems for type {domainType.Name}"));
            }
            else
            {
                return this._lazyTargetSystemServiceCache.Value.Values.SingleOrDefault(service => service.PersistentDomainObjectBaseType.IsAssignableFrom(domainType),
                    () => new BusinessLogicException($"Target System for type {domainType.Name} not found"));
            }
        }
    }
}
