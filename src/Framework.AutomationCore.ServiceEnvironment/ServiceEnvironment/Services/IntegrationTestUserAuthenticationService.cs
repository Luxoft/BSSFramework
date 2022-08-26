using System;
using System.Threading.Tasks;
using Framework.Core.Services;
using Framework.DomainDriven.NHibernate.Audit;

namespace Automation.ServiceEnvironment.Services;

public class IntegrationTestUserAuthenticationService : IDefaultUserAuthenticationService, IAuditRevisionUserAuthenticationService
{
    private readonly string integrationTestUserName;

    public IntegrationTestUserAuthenticationService(string integrationTestUserName = "IntegrationTestRootUser")
    {
        this.integrationTestUserName = integrationTestUserName;
    }

    public string CustomUserName { get; internal set; }

    public string GetUserName()
    {
        return this.CustomUserName ?? this.integrationTestUserName;
    }

    public async Task<T> WithImpersonateAsync<T>(string customUserName, Func<Task<T>> func)
    {
        var prev = this.CustomUserName;

        this.CustomUserName = customUserName;

        try
        {
            return await func();
        }
        finally
        {
            this.CustomUserName = prev;
        }
    }
}
