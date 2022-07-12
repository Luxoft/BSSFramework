using System;

using Framework.Configuration.BLL.SubscriptionSystemService3.Subscriptions;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.ServiceModel.Service;

namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IRootServiceEnvironment<out TBLLContext, out TBLLContextContainer> : IServiceEnvironment
{
    IServiceProvider RootServiceProvider { get; }

    SubscriptionMetadataStore SubscriptionMetadataStore { get; }

    TBLLContextContainer GetBLLContextContainer(IServiceProvider serviceProvider, IDBSession session, string currentPrincipalName = null);
}
