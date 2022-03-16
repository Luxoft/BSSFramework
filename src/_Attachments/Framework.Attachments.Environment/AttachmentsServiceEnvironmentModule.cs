using System;
using System.Linq;
using System.Collections.Generic;

using Framework.Attachments.BLL;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.IAD;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.ExpressionParsers;
using Framework.Persistent;
using Framework.Validation;

using JetBrains.Annotations;

namespace Framework.Attachments.Environment;

public class AttachmentsServiceEnvironmentModule<TMainServiceEnvironment, TBLLContextContainer, TBLLContext, TPersistentDomainObjectBase> : IServiceEnvironmentModule<TBLLContextContainer>, IServiceEnvironment<IAttachmentsBLLContext>
        where TMainServiceEnvironment : class, IRootServiceEnvironment<TBLLContext, TBLLContextContainer>
        where TBLLContextContainer : ServiceEnvironmentBase.ServiceEnvironmentBLLContextContainer, IBLLContextContainer<IAttachmentsBLLContext>, IAttachmentsBLLContextContainer
        where TPersistentDomainObjectBase : class, IIdentityObject<Guid>
        where TBLLContext : IDefaultBLLContext<TPersistentDomainObjectBase, Guid>
{
    private readonly TMainServiceEnvironment mainServiceEnvironment;

    public AttachmentsServiceEnvironmentModule([NotNull] TMainServiceEnvironment mainServiceEnvironment)
    {
        this.mainServiceEnvironment = mainServiceEnvironment ?? throw new ArgumentNullException(nameof(mainServiceEnvironment));

        this.DefaultAttachmentsValidatorCompileCache =

                this.SessionFactory
                    .AvailableValues
                    .ToValidation()
                    .ToBLLContextValidationExtendedData<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Guid>()
                    .Pipe(extendedValidationData => new AttachmentsValidationMap(extendedValidationData))
                    .ToCompileCache();
    }

    public ValidatorCompileCache DefaultAttachmentsValidatorCompileCache { get; }

    public IFetchService<Framework.Attachments.Domain.PersistentDomainObjectBase, FetchBuildRule> AttachmentsFetchService { get; } =

        new AttachmentsMainFetchService().WithCompress().WithCache().WithLock().Add(FetchService<Framework.Attachments.Domain.PersistentDomainObjectBase>.OData);



    public IDBSessionFactory SessionFactory => this.mainServiceEnvironment.SessionFactory;

    public IServiceProvider RootServiceProvider => this.mainServiceEnvironment.RootServiceProvider;

    public bool IsDebugMode => this.mainServiceEnvironment.IsDebugMode;


    public virtual IEnumerable<IDALListener> GetBeforeTransactionCompletedListeners(TBLLContextContainer container)
    {
        foreach (var listener in this.GetAttachmentCleanerDALListeners(container))
        {
            yield return listener;
        }
    }

    private IEnumerable<IDALListener> GetAttachmentCleanerDALListeners(TBLLContextContainer container)
    {
        return from targetSystemService in container.Attachments.GetPersistentTargetSystemServices()

               where targetSystemService.HasAttachments

               select new AttachmentCleanerDALListener(targetSystemService);
    }

    public IContextEvaluator<IAttachmentsBLLContext> GetContextEvaluator(IServiceProvider currentScopedServiceProvider = null)
    {
        return currentScopedServiceProvider == null
                       ? new RootContextEvaluator<IAttachmentsBLLContext>(this, this.RootServiceProvider)
                       : new ScopeContextEvaluator<IAttachmentsBLLContext>(this, currentScopedServiceProvider);
    }

    public IBLLContextContainer<IAttachmentsBLLContext> GetBLLContextContainer(
            IServiceProvider serviceProvider,
            IDBSession session,
            string currentPrincipalName = null)
    {
        return this.mainServiceEnvironment.GetBLLContextContainer(serviceProvider, session, currentPrincipalName);
    }
}
