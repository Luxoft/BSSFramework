using Framework.Core;

namespace Framework.BLL.Events.SubscriptionManager;

public interface IEventDTOMessageSender<in TPersistentDomainObjectBase> : IMessageSender<
    IDomainOperationSerializeData<TPersistentDomainObjectBase>>
    where TPersistentDomainObjectBase : class;
