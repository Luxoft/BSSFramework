using CommonFramework;

using Framework.Subscriptions.Metadata;

namespace Framework.Subscriptions.Subscriptions;

/// <inheritdoc />
public class SubscriptionMetadataFinder(IServiceProxyFactory serviceProxyFactory, IEnumerable<SubscriptionMetadataFinderAssemblyInfo> assemblyInfoList)
    : ISubscriptionMetadataFinder
{
    public IEnumerable<ISubscription> Find() =>
        from assemblyInfo in assemblyInfoList

        from type in assemblyInfo.Assembly.GetTypes()

        where !type.IsAbstract && typeof(ISubscription).IsAssignableFrom(type)

        select serviceProxyFactory.Create<ISubscription>(type);
}
