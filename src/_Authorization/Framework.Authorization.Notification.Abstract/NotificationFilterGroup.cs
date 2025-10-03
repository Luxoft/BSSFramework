using System.Collections.ObjectModel;

using CommonFramework;

using Framework.Persistent;

namespace Framework.Authorization.Notification;

public class NotificationFilterGroup
{
    public NotificationFilterGroup(Type securityContextType, IEnumerable<Guid> idents, NotificationExpandType expandType)
    {
        if (securityContextType == null) throw new ArgumentNullException(nameof(securityContextType));
        if (idents == null) throw new ArgumentNullException(nameof(idents));

        this.SecurityContextType = securityContextType;
        this.Idents = idents.ToReadOnlyCollection();
        this.ExpandType = expandType;
    }


    public Type SecurityContextType { get; private set; }

    public ReadOnlyCollection<Guid> Idents { get; private set; }

    public NotificationExpandType ExpandType { get; private set; }

    public static NotificationFilterGroup Create<TDomainObject>(IEnumerable<TDomainObject> domainObjects, NotificationExpandType expandType)
        where TDomainObject : IIdentityObject<Guid>
    {
        return new NotificationFilterGroup(
            typeof(TDomainObject),
            domainObjects.FromMaybe(() => new ArgumentNullException(nameof(domainObjects))).Select(domainObject => domainObject.Id),
            expandType);
    }
}
