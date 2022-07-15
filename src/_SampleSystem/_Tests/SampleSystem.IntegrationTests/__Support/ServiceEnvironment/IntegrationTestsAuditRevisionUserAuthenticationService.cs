using Framework.DomainDriven.NHibernate.Audit;

namespace SampleSystem.IntegrationTests.__Support.ServiceEnvironment;

public class IntegrationTestsAuditRevisionUserAuthenticationService : IAuditRevisionUserAuthenticationService
{
    public string GetUserName() => $"{System.Environment.UserDomainName}\\{System.Environment.UserName}";
}
