namespace Framework.Configuration.BLL;

public partial interface IConfigurationBLLFactoryContainer
{
    ISubscriptionBLL Subscription { get; }
}
