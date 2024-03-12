using Framework.Core;

namespace Framework.Events.Legacy;

public interface IEventDTOMessageSender<in TPersistentDomainObjectBase> : IMessageSender<
    IDomainOperationSerializeData<TPersistentDomainObjectBase>>
    where TPersistentDomainObjectBase : class
{
}
