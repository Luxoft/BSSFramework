using Framework.Events;
using Framework.Persistent;

namespace Framework.DomainDriven.ServiceModel.IAD;

public static class TargetSystemHelper
{
    public static readonly string AuthorizationName = typeof(Framework.Authorization.Domain.PersistentDomainObjectBase).GetTargetSystemName();

    public static readonly string ConfigurationName = typeof(Framework.Configuration.Domain.PersistentDomainObjectBase).GetTargetSystemName();
}
