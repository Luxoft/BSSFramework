using Framework.Core;
using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public interface ISubscriptionSystemService
{
    IList<ITryResult<Subscription>> ProcessChangedObjectUntyped(object? prev, object? next, Type type);

    SubscriptionRecipientInfo GetRecipientsUntyped(Type type, object? prev, object? next, string subscriptionCode);
}

public interface ISubscriptionSystemService<in TPersistentDomainObjectBase> : ISubscriptionSystemService
{
    SubscriptionRecipientInfo GetRecipients<TDomainObject>(TDomainObject prev, TDomainObject next, string subscriptionCode)
            where TDomainObject : TPersistentDomainObjectBase;
}
