using CommonFramework;

using Framework.Configuration.SubscriptionModeling;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <inheritdoc />
public class SubscriptionMetadataFinder(IServiceProxyFactory serviceProxyFactory, IEnumerable<SubscriptionMetadataFinderAssemblyInfo> assemblyInfoList)
    : ISubscriptionMetadataFinder
{
    public IEnumerable<ISubscriptionMetadata> Find()
    {
        return from assemblyInfo in assemblyInfoList

               from type in assemblyInfo.Assembly.GetTypes()

               where !type.IsAbstract && typeof(ISubscriptionMetadata).IsAssignableFrom(type)

               select serviceProxyFactory.Create<ISubscriptionMetadata>(type);
    }
}
