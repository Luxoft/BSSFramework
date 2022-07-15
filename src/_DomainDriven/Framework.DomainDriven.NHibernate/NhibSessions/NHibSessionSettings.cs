using System;

using Framework.Core.Services;
using Framework.DomainDriven.Audit;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

namespace Framework.DomainDriven.NHibernate;

public class NHibSessionSettings : INHibSessionSettings
{
    [NotNull]
    private readonly IUserAuthenticationService userAuthenticationService;

    [NotNull]
    private readonly IDateTimeService dateTimeService;

    public NHibSessionSettings(
            [NotNull] IUserAuthenticationService userAuthenticationService,
            [NotNull] IDateTimeService dateTimeService)
    {
        this.userAuthenticationService = userAuthenticationService ?? throw new ArgumentNullException(nameof(userAuthenticationService));
        this.dateTimeService = dateTimeService ?? throw new ArgumentNullException(nameof(dateTimeService));
    }

    public DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(this.userAuthenticationService, this.dateTimeService);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(this.userAuthenticationService, this.dateTimeService);
}
