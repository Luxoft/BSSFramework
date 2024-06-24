using Framework.Core.Services;
using Framework.DomainDriven.Audit;



namespace Framework.DomainDriven.NHibernate;

public class NHibSessionSettings : INHibSessionSetup
{
    
    private readonly IUserAuthenticationService userAuthenticationService;

    
    private readonly TimeProvider timeProvider;

    public NHibSessionSettings(
            IUserAuthenticationService userAuthenticationService,
            TimeProvider timeProvider)
    {
        this.userAuthenticationService = userAuthenticationService ?? throw new ArgumentNullException(nameof(userAuthenticationService));
        this.timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
    }

    public DBSessionMode DefaultSessionMode { get; } = DBSessionMode.Write;

    public AuditPropertyPair GetCreateAuditProperty() => AuditPropertyPair.GetCreateAuditProperty(this.userAuthenticationService, this.timeProvider);

    public AuditPropertyPair GetModifyAuditProperty() => AuditPropertyPair.GetModifyAuditProperty(this.userAuthenticationService, this.timeProvider);
}
