namespace Framework.Events.Legacy;

public interface ISubscriptionManagerSetupObject
{
    ISubscriptionManagerSetupObject Add<TSubscriptionManager>()
        where TSubscriptionManager : class, IEventOperationReceiver;
}
