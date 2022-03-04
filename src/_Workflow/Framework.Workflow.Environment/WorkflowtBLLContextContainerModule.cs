using System;
using System.Collections.Generic;

using Framework.Authorization;
using Framework.Authorization.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Security;
using Framework.Events;
using Framework.Persistent;
using Framework.Validation;
using Framework.Workflow.BLL;
using Framework.Workflow.Environment;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD;


public abstract class WorkflowBLLContextContainerModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : IBLLContextContainerModule
        where TMainServiceEnvironment : class, IRootServiceEnvironment<TBLLContext, TBLLContextContainer>
        where TBLLContextContainer : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext>.ServiceEnvironmentBLLContextContainer, IBLLContextContainer<IWorkflowBLLContext>, IWorkflowBLLContextContainer
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>, ISecurityBLLContext<IAuthorizationBLLContext, TPersistentDomainObjectBase, Guid>, IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>
        where TSecurityOperationCode : struct, Enum
{
    protected readonly WorkflowServiceEnvironmentModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase> WorkflowServiceEnvironment;

    protected readonly TMainServiceEnvironment MainServiceEnvironment;

    protected readonly TBLLContextContainer BllContextContainer;

    protected readonly BLLOperationEventListenerContainer<Framework.Workflow.Domain.DomainObjectBase>
            WorkflowOperationListeners =
                    new BLLOperationEventListenerContainer<Framework.Workflow.Domain.DomainObjectBase>();

    protected readonly BLLSourceEventListenerContainer<Framework.Workflow.Domain.PersistentDomainObjectBase>
            WorkflowSourceListeners =
                    new BLLSourceEventListenerContainer<Framework.Workflow.Domain.PersistentDomainObjectBase>();

    private readonly Lazy<IEventsSubscriptionManager<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase>> lazyWorkflowEventsSubscriptionManager;

    protected WorkflowBLLContextContainerModule(
            [NotNull] WorkflowServiceEnvironmentModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase> workflowServiceEnvironment,
            [NotNull] TMainServiceEnvironment mainServiceEnvironment,
            TBLLContextContainer bllContextContainer)
    {
        this.WorkflowServiceEnvironment = workflowServiceEnvironment ?? throw new ArgumentNullException(nameof(workflowServiceEnvironment));
        this.MainServiceEnvironment = mainServiceEnvironment ?? throw new ArgumentNullException(nameof(mainServiceEnvironment));
        this.BllContextContainer = bllContextContainer;
        this.lazyWorkflowEventsSubscriptionManager = LazyHelper.Create(this.CreateWorkflowEventsSubscriptionManager);


        this.Workflow = LazyInterfaceImplementHelper.CreateProxy(this.CreateWorkflowBLLContext);
    }

    protected virtual IWorkflowBLLContext CreateWorkflowBLLContext()
    {
        return new WorkflowBLLContext(
                                      this.BllContextContainer.ScopedServiceProvider,
                                      this.BllContextContainer.Session.GetDALFactory<Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(),
                                      this.WorkflowOperationListeners,
                                      this.WorkflowSourceListeners,
                                      this.BllContextContainer.Session.GetObjectStateService(),
                                      this.BllContextContainer.GetAccessDeniedExceptionService<Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(),
                                      this.BllContextContainer.StandartExpressionBuilder,
                                      LazyInterfaceImplementHelper.CreateProxy<IValidator>(this.CreateWorkflowValidator),
                                      this.BllContextContainer.HierarchicalObjectExpanderFactory,
                                      this.WorkflowServiceEnvironment.WorkflowFetchService,
                                      this.BllContextContainer.GetDateTimeService(),
                                      LazyInterfaceImplementHelper.CreateProxy(() => this.BllContextContainer.GetSecurityExpressionBuilderFactory<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(this.Workflow)),
                                      this.BllContextContainer.Configuration,
                                      this.BllContextContainer.Authorization,
                                      () => new WorkflowSecurityService(this.Workflow),
                                      () => new WorkflowBLLFactoryContainer(this.Workflow),
                                      this.WorkflowServiceEnvironment.WorkflowLambdaProcessorFactory,
                                      this.WorkflowServiceEnvironment.WorkflowAnonymousTypeBuilder,
                                      this.GetWorkflowTargetSystemServices(),
                                      principalName => ((TBLLContextContainer)this.BllContextContainer.Impersonate(principalName)).Workflow,
                                      this.WorkflowServiceEnvironment.WorkflowAnonymousObjectValidator);
    }

    protected IEventsSubscriptionManager<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase> WorkflowEventsSubscriptionManager => this.lazyWorkflowEventsSubscriptionManager.Value;

    /// <summary>
    /// Подписка на евенты
    /// </summary>
    public virtual void SubscribeEvents()
    {
        this.WorkflowEventsSubscriptionManager.Maybe(eventManager => eventManager.Subscribe());
    }

    public IWorkflowBLLContext Workflow { get; }

    /// <summary>
    /// Создание валидатора для воркфлоу
    /// </summary>
    /// <returns></returns>
    protected virtual WorkflowValidator CreateWorkflowValidator()
    {
        return new WorkflowValidator(this.Workflow, this.WorkflowServiceEnvironment.DefaultWorkflowValidatorCompileCache);
    }

    protected virtual IEventsSubscriptionManager<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase> CreateWorkflowEventsSubscriptionManager()
    {
        return null;
    }


    protected abstract IEnumerable<Framework.Workflow.BLL.ITargetSystemService> GetWorkflowTargetSystemServices();

    protected Framework.Configuration.BLL.ITargetSystemService GetWorkflowConfigurationTargetSystemServices()
    {
        return new Framework.Configuration.BLL.TargetSystemService<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase>(
         this.BllContextContainer.Configuration,
         this.Workflow,
         this.BllContextContainer.Configuration.Logics.TargetSystem.GetByName(WorkflowTargetSystemHelper.WorkflowName, true),
         this.GetWorkflowEventDALListeners(),
         this.MainServiceEnvironment.SubscriptionMetadataStore);
    }

    protected Framework.Workflow.BLL.ITargetSystemService GetMainWorkflowTargetSystemService(HashSet<Type> workflowSourceTypes)
    {
        return new TargetSystemService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>(
         this.Workflow,
         this.BllContextContainer.MainContext,
         this.Workflow.Logics.TargetSystem.GetObjectBy(ts => ts.IsMain, true),
         this.WorkflowServiceEnvironment.WorkflowMainSystemCompileCache,
         workflowSourceTypes);
    }

    /// <summary>
    /// Возврат DAL-подписчиков, при их вызове изменения в базе данных всё ещё доступны
    /// </summary>
    /// <returns></returns>
    public IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
    {
        var dalListeners = new[]
                           {
                                   this.GetWorkflowEventDALListeners()
                           };

        return dalListeners.SelectMany();
    }


    /// <summary>
    /// Получение воркфлоу DALListener-ов с возможностью ручных вызовов
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerable<IManualEventDALListener<Framework.Workflow.Domain.PersistentDomainObjectBase>> GetWorkflowEventDALListeners()
    {
        yield break;
    }
}
