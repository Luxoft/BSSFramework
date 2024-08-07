namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

public interface ISubscriptionInitializer
{
    Task Initialize(CancellationToken cancellationToken);
}
