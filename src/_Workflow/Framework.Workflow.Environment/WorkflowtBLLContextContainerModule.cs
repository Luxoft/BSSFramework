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

//IBLLContextContainer<IWorkflowBLLContext>
{
    private readonly WorkflowServiceEnvironmentModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase> workflowServiceEnvironment;

    private readonly TMainServiceEnvironment mainServiceEnvironment;

    private readonly TBLLContextContainer bllContextContainer;

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
        this.workflowServiceEnvironment = workflowServiceEnvironment ?? throw new ArgumentNullException(nameof(workflowServiceEnvironment));
        this.mainServiceEnvironment = mainServiceEnvironment ?? throw new ArgumentNullException(nameof(mainServiceEnvironment));
        this.bllContextContainer = bllContextContainer;
        this.lazyWorkflowEventsSubscriptionManager = LazyHelper.Create(this.CreateWorkflowEventsSubscriptionManager);


        this.Workflow = LazyInterfaceImplementHelper.CreateProxy(this.CreateWorkflowBLLContext);
    }

    protected virtual IWorkflowBLLContext CreateWorkflowBLLContext()
    {
        return new WorkflowBLLContext(
                                      this.bllContextContainer.ScopedServiceProvider,
                                      this.bllContextContainer.Session.GetDALFactory<Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(),
                                      this.WorkflowOperationListeners,
                                      this.WorkflowSourceListeners,
                                      this.bllContextContainer.Session.GetObjectStateService(),
                                      this.bllContextContainer.GetAccessDeniedExceptionService<Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(),
                                      this.bllContextContainer.StandartExpressionBuilder,
                                      LazyInterfaceImplementHelper.CreateProxy<IValidator>(this.CreateWorkflowValidator),
                                      this.bllContextContainer.HierarchicalObjectExpanderFactory,
                                      this.workflowServiceEnvironment.WorkflowFetchService,
                                      this.bllContextContainer.GetDateTimeService(),
                                      LazyInterfaceImplementHelper.CreateProxy(() => this.bllContextContainer.GetSecurityExpressionBuilderFactory<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase, Guid>(this.Workflow)),
                                      this.bllContextContainer.Configuration,
                                      this.bllContextContainer.Authorization,
                                      () => new WorkflowSecurityService(this.Workflow),
                                      () => new WorkflowBLLFactoryContainer(this.Workflow),
                                      this.workflowServiceEnvironment.WorkflowLambdaProcessorFactory,
                                      this.workflowServiceEnvironment.WorkflowAnonymousTypeBuilder,
                                      this.GetWorkflowTargetSystemServices(),
                                      principalName => ((TBLLContextContainer)this.bllContextContainer.Impersonate(principalName)).Workflow,
                                      this.workflowServiceEnvironment.WorkflowAnonymousObjectValidator);
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
        return new WorkflowValidator(this.Workflow, this.workflowServiceEnvironment.DefaultWorkflowValidatorCompileCache);
    }

    protected virtual IEventsSubscriptionManager<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase> CreateWorkflowEventsSubscriptionManager()
    {
        return null;
    }


    protected abstract IEnumerable<Framework.Workflow.BLL.ITargetSystemService> GetWorkflowTargetSystemServices();

    protected Framework.Configuration.BLL.ITargetSystemService GetWorkflowConfigurationTargetSystemServices()
    {
        return new Framework.Configuration.BLL.TargetSystemService<IWorkflowBLLContext, Framework.Workflow.Domain.PersistentDomainObjectBase>(
         this.bllContextContainer.Configuration,
         this.Workflow,
         this.bllContextContainer.Configuration.Logics.TargetSystem.GetByName(WorkflowTargetSystemHelper.WorkflowName, true),
         this.GetWorkflowEventDALListeners(),
         this.mainServiceEnvironment.SubscriptionMetadataStore);
    }

    protected Framework.Workflow.BLL.ITargetSystemService GetAuthorizationWorkflowTargetSystemService()
    {
        return new Framework.Workflow.BLL.TargetSystemService<IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, AuthorizationSecurityOperationCode>(
         this.Workflow,
         this.bllContextContainer.Authorization,
         this.Workflow.Logics.TargetSystem.GetByName(TargetSystemHelper.AuthorizationName, true),
         this.workflowServiceEnvironment.WorkflowAuthorizationSystemCompileCache,
         new[] { typeof(Framework.Authorization.Domain.Permission) });
    }

    protected Framework.Workflow.BLL.ITargetSystemService GetMainWorkflowTargetSystemService(HashSet<Type> workflowSourceTypes)
    {
        return new TargetSystemService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>(
         this.Workflow,
         this.bllContextContainer.MainContext,
         this.Workflow.Logics.TargetSystem.GetObjectBy(ts => ts.IsMain, true),
         this.workflowServiceEnvironment.WorkflowMainSystemCompileCache,
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
