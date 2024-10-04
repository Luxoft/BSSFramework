using Framework.Core;
using Framework.DomainDriven.Audit;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.NHibernate;

public class NHibSessionSettings(
    IServiceProvider serviceProvider,
    TimeProvider timeProvider)
    : INHibSessionSetup
{
    private readonly Lazy<ICurrentUser> lazyCurrentUser = LazyHelper.Create(serviceProvider.GetRequiredService<ICurrentUser>);

    public DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(this.lazyCurrentUser.Value, timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(this.lazyCurrentUser.Value, timeProvider);
}
