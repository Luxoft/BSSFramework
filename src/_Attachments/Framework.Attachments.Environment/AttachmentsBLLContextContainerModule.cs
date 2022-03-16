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
using Framework.Attachments.BLL;
using Framework.Attachments.Environment;

using JetBrains.Annotations;

namespace Framework.DomainDriven.ServiceModel.IAD;


public abstract class AttachmentsBLLContextContainerModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> : IBLLContextContainerModule
        where TMainServiceEnvironment : class, IRootServiceEnvironment<TBLLContext, TBLLContextContainer>
        where TBLLContextContainer : ServiceEnvironmentBase<TBLLContextContainer, TBLLContext>.ServiceEnvironmentBLLContextContainer, IBLLContextContainer<IAttachmentsBLLContext>, IAttachmentsBLLContextContainer
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TBLLContext : class, ITypeResolverContainer<string>, ISecurityServiceContainer<IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode>>, ISecurityBLLContext<IAuthorizationBLLContext, TPersistentDomainObjectBase, Guid>, IAccessDeniedExceptionServiceContainer<TPersistentDomainObjectBase>
        where TSecurityOperationCode : struct, Enum
{
    protected readonly AttachmentsServiceEnvironmentModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase> AttachmentsServiceEnvironment;

    protected readonly TMainServiceEnvironment MainServiceEnvironment;

    protected readonly TBLLContextContainer BllContextContainer;

    protected readonly BLLOperationEventListenerContainer<Framework.Attachments.Domain.DomainObjectBase>
            AttachmentsOperationListeners =
                    new BLLOperationEventListenerContainer<Framework.Attachments.Domain.DomainObjectBase>();

    protected readonly BLLSourceEventListenerContainer<Framework.Attachments.Domain.PersistentDomainObjectBase>
            AttachmentsSourceListeners =
                    new BLLSourceEventListenerContainer<Framework.Attachments.Domain.PersistentDomainObjectBase>();

    private readonly Lazy<IEventsSubscriptionManager<IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase>> lazyAttachmentsEventsSubscriptionManager;

    protected AttachmentsBLLContextContainerModule(
            [NotNull] AttachmentsServiceEnvironmentModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase> attachmentsServiceEnvironment,
            [NotNull] TMainServiceEnvironment mainServiceEnvironment,
            TBLLContextContainer bllContextContainer)
    {
        this.AttachmentsServiceEnvironment = attachmentsServiceEnvironment ?? throw new ArgumentNullException(nameof(attachmentsServiceEnvironment));
        this.MainServiceEnvironment = mainServiceEnvironment ?? throw new ArgumentNullException(nameof(mainServiceEnvironment));
        this.BllContextContainer = bllContextContainer;
        this.lazyAttachmentsEventsSubscriptionManager = LazyHelper.Create(this.CreateAttachmentsEventsSubscriptionManager);


        this.Attachments = LazyInterfaceImplementHelper.CreateProxy(this.CreateAttachmentsBLLContext);
    }

    protected virtual IAttachmentsBLLContext CreateAttachmentsBLLContext()
    {
        return new AttachmentsBLLContext(
                                      this.BllContextContainer.ScopedServiceProvider,
                                      this.BllContextContainer.Session.GetDALFactory<Framework.Attachments.Domain.PersistentDomainObjectBase, Guid>(),
                                      this.AttachmentsOperationListeners,
                                      this.AttachmentsSourceListeners,
                                      this.BllContextContainer.Session.GetObjectStateService(),
                                      this.BllContextContainer.GetAccessDeniedExceptionService<Framework.Attachments.Domain.PersistentDomainObjectBase, Guid>(),
                                      this.BllContextContainer.StandartExpressionBuilder,
                                      LazyInterfaceImplementHelper.CreateProxy<IValidator>(this.CreateAttachmentsValidator),
                                      this.BllContextContainer.HierarchicalObjectExpanderFactory,
                                      this.AttachmentsServiceEnvironment.AttachmentsFetchService,
                                      this.BllContextContainer.GetDateTimeService(),
                                      LazyInterfaceImplementHelper.CreateProxy(() => this.BllContextContainer.GetSecurityExpressionBuilderFactory<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Guid>(this.Attachments)),

                                      LazyInterfaceImplementHelper.CreateProxy<IAttachmentsSecurityService>(() => new AttachmentsSecurityService(this.Attachments)),
                                      LazyInterfaceImplementHelper.CreateProxy<IAttachmentsBLLFactoryContainer>(() => new AttachmentsBLLFactoryContainer(this.Attachments)),
                                      this.BllContextContainer.Authorization,
                                      this.GetAttachmentsTargetSystemServices());
    }

    protected IEventsSubscriptionManager<IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase> AttachmentsEventsSubscriptionManager => this.lazyAttachmentsEventsSubscriptionManager.Value;

    /// <summary>
    /// Подписка на евенты
    /// </summary>
    public virtual void SubscribeEvents()
    {
        this.AttachmentsEventsSubscriptionManager.Maybe(eventManager => eventManager.Subscribe());
    }

    public IAttachmentsBLLContext Attachments { get; }

    /// <summary>
    /// Создание валидатора для воркфлоу
    /// </summary>
    /// <returns></returns>
    protected virtual AttachmentsValidator CreateAttachmentsValidator()
    {
        return new AttachmentsValidator(this.Attachments, this.AttachmentsServiceEnvironment.DefaultAttachmentsValidatorCompileCache);
    }

    protected virtual IEventsSubscriptionManager<IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase> CreateAttachmentsEventsSubscriptionManager()
    {
        return null;
    }


    protected abstract IEnumerable<Framework.Attachments.BLL.ITargetSystemService> GetAttachmentsTargetSystemServices();


    protected Framework.Attachments.BLL.ITargetSystemService GetMainAttachmentsTargetSystemService()
    {
        return new TargetSystemService<TBLLContext, TPersistentDomainObjectBase>(
            this.Attachments,
            this.BllContextContainer.MainContext,
            this.Attachments.Logics.TargetSystem.GetObjectBy(ts => ts.IsMain, true));
    }
}
