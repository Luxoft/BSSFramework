using CommonFramework;

using Framework.Configuration.Domain;

namespace Framework.Configuration.BLL;

public partial class SubscriptionBLL(IConfigurationBLLContext context) : ISubscriptionBLL
{
    public bool HasActiveSubscriptions(Type type)
    {
        if (type == null) throw new ArgumentNullException(nameof(type));

        return context.GetDomainType(type, false).Maybe(domainType => this.HasActiveSubscriptions(domainType));
    }

    public bool HasActiveSubscriptions(DomainType domainType)
    {
        if (domainType == null) throw new ArgumentNullException(nameof(domainType));

        return domainType.TargetSystem.SubscriptionEnabled;
    }
}
