using System.Collections.ObjectModel;

using CommonFramework;

using Framework.Persistent;

namespace Framework.Authorization.Notification;

public class NotificationFilterGroup(Type securityContextType, IEnumerable<Guid> idents, NotificationExpandType expandType)
{
    public Type SecurityContextType { get; private set; } = securityContextType;

    public ReadOnlyCollection<Guid> Idents { get; private set; } = idents.ToReadOnlyCollection();

    public NotificationExpandType ExpandType { get; private set; } = expandType;

    public static NotificationFilterGroup Create<TDomainObject>(IEnumerable<TDomainObject> domainObjects, NotificationExpandType expandType)
        where TDomainObject : IIdentityObject<Guid>
    {
        return new NotificationFilterGroup(
            typeof(TDomainObject),
            domainObjects.FromMaybe(() => new ArgumentNullException(nameof(domainObjects))).Select(domainObject => domainObject.Id),
            expandType);
    }
}
