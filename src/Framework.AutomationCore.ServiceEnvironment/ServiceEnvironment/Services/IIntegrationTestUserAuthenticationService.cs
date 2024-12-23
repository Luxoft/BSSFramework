﻿using Framework.DomainDriven.Auth;
using Framework.DomainDriven.NHibernate.Audit;
using Framework.SecuritySystem.Credential;

namespace Automation.ServiceEnvironment.Services;

public interface IIntegrationTestUserAuthenticationService : IDefaultUserAuthenticationService, IAuditRevisionUserAuthenticationService
{
    void SetUser(UserCredential? customUserCredential);

    void Reset();

    Task<T> WithImpersonateAsync<T>(UserCredential customUserCredential, Func<Task<T>> func);
}
