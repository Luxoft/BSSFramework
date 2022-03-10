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
    }

    public interface ITargetSystemService<out TBLLContext> : ITargetSystemService
    {
        new TBLLContext TargetSystemContext { get; }
    }

    public interface IPersistentTargetSystemService : ITargetSystemService, IPersistentDomainObjectBaseTypeContainer
    {
        new IRevisionSubscriptionSystemService SubscriptionService { get; }


        IRevisionSubscriptionSystemService GetSubscriptionService(IMessageSender<MessageTemplateNotification> subscriptionSender);

        void ForceEvent(DomainTypeEventOperation operation, long? revision, Guid domainObjectId);
    }
}
