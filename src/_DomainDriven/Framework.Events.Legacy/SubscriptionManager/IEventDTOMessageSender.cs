using Framework.Core;

namespace Framework.Events;

public interface IEventDTOMessageSender<in TPersistentDomainObjectBase> : IMessageSender<
    IDomainOperationSerializeData<TPersistentDomainObjectBase>>
    where TPersistentDomainObjectBase : class
{
}
