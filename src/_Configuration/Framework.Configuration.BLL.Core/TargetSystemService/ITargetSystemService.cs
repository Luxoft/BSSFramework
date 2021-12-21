using System;
using System.Collections.Generic;

using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Notification;
using Framework.Persistent;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL
{
    public interface ITargetSystemService :
        ITypeResolverContainer<DomainType>,
        ITargetSystemElement<TargetSystem>,
        IVisualIdentityObject
    {
        ISubscriptionSystemService SubscriptionService { get; }

        object TargetSystemContext { get; }

        Type TargetSystemContextType { get; }

        ITypeResolver<string> TypeResolverS { get; }

        bool IsAssignable(Type domainType);

//        void ExecuteRegularJob(RegularJob job);

        void ExecuteBLLContextLambda(ILambdaObject lambdaObject);

        void ValidateRegularJob(RegularJob job);
    }

    public interface ITargetSystemService<out TBLLContext> : ITargetSystemService
    {
        new TBLLContext TargetSystemContext { get; }
    }

    public interface IPersistentTargetSystemService : ITargetSystemService, IPersistentDomainObjectBaseTypeContainer, IAttachmentSecurityProviderSource
    {
        new IRevisionSubscriptionSystemService SubscriptionService { get; }

        bool HasAttachments { get; }


        void TryRemoveAttachments(Array domainObjects);


        void TryDenormalizeHasAttachmentFlag(AttachmentContainer container, bool value);

        IEnumerable<Guid> GetNotExistsObjects(DomainType domainType, IEnumerable<Guid> idents);


        IRevisionSubscriptionSystemService GetSubscriptionService(IMessageSender<MessageTemplateNotification> subscriptionSender);

        void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId);
    }

    public interface ITargetSystemService<out TBLLContext, in TPersistentDomainObjectBase> : ITargetSystemService<TBLLContext>, IPersistentTargetSystemService
        where TPersistentDomainObjectBase : class
    {
        void TryRemoveAttachments<TDomainObject>(IEnumerable<TDomainObject> domainObjects)
            where TDomainObject : class, TPersistentDomainObjectBase;
    }
}
