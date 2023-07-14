using Framework.Core.Services;
using Framework.DomainDriven.NHibernate.Audit;

namespace Automation.ServiceEnvironment.Services;

public interface IIntegrationTestUserAuthenticationService : IDefaultUserAuthenticationService, IAuditRevisionUserAuthenticationService, IUserAuthenticationService
{
    public void SetUserName(string customUserName);

    public void Reset();

    public Task<T> WithImpersonateAsync<T>(string customUserName, Func<Task<T>> func);
}
