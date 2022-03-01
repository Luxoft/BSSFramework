using System;
using System.Collections.Generic;

using Framework.Authorization;
using Framework.Authorization.BLL;
using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Events;
using Framework.Validation;
using Framework.Workflow.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class WorkflowtBLLContextContainerModule

        //IBLLContextContainer<IWorkflowBLLContext>
{

    protected readonly BLLOperationEventListenerContainer<Framework.Workflow.Domain.DomainObjectBase>
            WorkflowOperationListeners =
                    new BLLOperationEventListenerContainer<Framework.Workflow.Domain.DomainObjectBase>();

    protected readonly BLLSourceEventListenerContainer<Framework.Workflow.Domain.PersistentDomainObjectBase>
            WorkflowSourceListeners =
                    new BLLSourceEventListenerContainer<Framework.Workflow.Domain.PersistentDomainObjectBase>();

    private readonly Lazy<IEventsSubscriptionManager<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase>> lazyWorkflowEventsSubscriptionManager;


    public WorkflowtBLLContextContainerModule()
    {
        this.lazyWorkflowEventsSubscriptionManager = LazyHelper.Create(this.CreateWorkflowEventsSubscriptionManager);


        this.Workflow = LazyInterfaceImplementHelper.CreateProxy(this.CreateWorkflowBLLContext);
    }

    protected virtual IWorkflowBLLContext CreateWorkflowBLLContext()
    {
        return new WorkflowBLLContext(
                                      this.ScopedServiceProvider,
                                      this.Session.GetDALFactory<Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(),
                                      this.WorkflowOperationListeners,
                                      this.WorkflowSourceListeners,
                                      this.Session.GetObjectStateService(),
                                      this.GetAccessDeniedExceptionService<Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(),
                                      this.StandartExpressionBuilder,
                                      LazyInterfaceImplementHelper.CreateProxy<IValidator>(this.CreateWorkflowValidator),
                                      this.HierarchicalObjectExpanderFactory,
                                      this.ServiceEnvironment.workflowFetchService,
                                      this.GetDateTimeService(),
                                      LazyInterfaceImplementHelper.CreateProxy(() => this.GetSecurityExpressionBuilderFactory<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(this.Workflow)),
                                      this.Configuration,
                                      this.Authorization,
                                      () => new WorkflowSecurityService(this.Workflow),
                                      () => new WorkflowBLLFactoryContainer(this.Workflow),
                                      this.ServiceEnvironment._workflowLambdaProcessorFactory,
                                      this.ServiceEnvironment.workflowAnonymousTypeBuilder,
                                      this.GetWorkflowTargetSystemServices(this.ServiceEnvironment.SubscriptionMetadataStore),
                                      principalName => this.Impersonate(principalName).Workflow,
                                      this.ServiceEnvironment.workflowAnonymousObjectValidator);
    }

    protected IEventsSubscriptionManager<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase> WorkflowEventsSubscriptionManager => this.lazyWorkflowEventsSubscriptionManager.Value;

    /// <summary>
    /// Подписка на евенты
    /// </summary>
    protected internal virtual void SubscribeEvents()
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
        return new WorkflowValidator(this.Workflow, this.ServiceEnvironment.DefaultWorkflowValidatorCompileCache);
    }

    protected virtual IEventsSubscriptionManager<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase> CreateWorkflowEventsSubscriptionManager()
    {
        return null;
    }


    protected abstract IEnumerable<Framework.Workflow.BLL.ITargetSystemService> GetWorkflowTargetSystemServices(
            SubscriptionMetadataStore subscriptionMetadataStore);

    protected Framework.Configuration.BLL.ITargetSystemService GetWorkflowConfigurationTargetSystemServices()
    {
        return new Framework.Configuration.BLL.TargetSystemService<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase>(
         this.Configuration,
         this.Workflow,
         this.Configuration.Logics.TargetSystem.GetByName(TargetSystemHelper.WorkflowName, true),
         this.GetWorkflowEventDALListeners(),
         this.ServiceEnvironment.SubscriptionMetadataStore);
    }

    protected Framework.Workflow.BLL.ITargetSystemService GetAuthorizationWorkflowTargetSystemService()
    {
        return new Framework.Workflow.BLL.TargetSystemService<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, AuthorizationSecurityOperationCode>(
         this.Workflow,
         this.Authorization,
         this.Workflow.Logics.TargetSystem.GetByName(TargetSystemHelper.AuthorizationName, true),
         this.ServiceEnvironment.workflowAuthorizationSystemCompileCache,
         new[] { typeof(Framework.Authorization.Domain.Permission) });
    }

    protected Framework.Workflow.BLL.ITargetSystemService GetMainWorkflowTargetSystemService(HashSet<Type> workflowSourceTypes)
    {
        return new TargetSystemService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>(
         this.Workflow,
         this.MainContext,
         this.Workflow.Logics.TargetSystem.GetObjectBy(ts => ts.IsMain, true),
         this.serviceEnvironment._workflowMainSystemCompileCache,
         workflowSourceTypes);
    }

    protected override IEnumerable<Framework.Workflow.BLL.ITargetSystemService> GetWorkflowTargetSystemServices(
            SubscriptionMetadataStore subscriptionMetadataStore)
    {
        yield return this.GetMainWorkflowTargetSystemService(new HashSet<Type>(new[] { typeof(Domain.Location) }));
        yield return this.GetAuthorizationWorkflowTargetSystemService();
    }

    /// <summary>
    /// Возврат DAL-подписчиков, при их вызове изменения в базе данных всё ещё доступны
    /// </summary>
    /// <returns></returns>
    protected override IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners()
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


    IWorkflowBLLContext IBLLContextContainer<IWorkflowBLLContext>.Context
    {
        get { return this.Workflow; }
    }
}
