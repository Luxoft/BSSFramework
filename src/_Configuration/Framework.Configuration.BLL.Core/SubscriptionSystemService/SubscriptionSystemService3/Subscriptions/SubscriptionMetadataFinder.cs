using Framework.Configuration.SubscriptionModeling;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;

/// <inheritdoc />
public class SubscriptionMetadataFinder : ISubscriptionMetadataFinder
{
    private readonly IServiceProvider rootServiceProvider;

    private readonly IEnumerable<SubscriptionMetadataFinderAssemblyInfo> assemblyInfoList;

    public SubscriptionMetadataFinder(IServiceProvider rootServiceProvider, IEnumerable<SubscriptionMetadataFinderAssemblyInfo> assemblyInfoList)
    {
        this.rootServiceProvider = rootServiceProvider;
        this.assemblyInfoList = assemblyInfoList;
    }

    public IEnumerable<ISubscriptionMetadata> Find()
    {
        return from assemblyInfo in this.assemblyInfoList

               from type in assemblyInfo.Assembly.GetTypes()

               where !type.IsAbstract && typeof(ISubscriptionMetadata).IsAssignableFrom(type)

               select (ISubscriptionMetadata)ActivatorUtilities.CreateInstance(this.rootServiceProvider, type);
    }
}
