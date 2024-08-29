﻿using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationSystemFactory(IServiceProvider serviceProvider) : IAuthorizationSystemFactory
{
    public ISecuritySystem Create(bool withRunAs)
    {
        return ActivatorUtilities.CreateInstance<AuthorizationSystemBase>(serviceProvider, withRunAs);
    }
}
