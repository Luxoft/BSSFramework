using Framework.Core.Services;
using Framework.DomainDriven.NHibernate.Audit;

namespace Automation.ServiceEnvironment.Services;

public interface IIntegrationTestUserAuthenticationService : IDefaultUserAuthenticationService, IAuditRevisionUserAuthenticationService, IUserAuthenticationService
{
    void SetUserName(string customUserName);

    void Reset();

    Task<T> WithImpersonateAsync<T>(string customUserName, Func<Task<T>> func);
}
