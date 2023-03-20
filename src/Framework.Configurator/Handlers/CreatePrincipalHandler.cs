﻿using System.Threading;
using System.Threading.Tasks;

using Framework.Authorization.BLL.Core.Context;
using Framework.Authorization.Domain;
using Framework.Configurator.Interfaces;
using Framework.SecuritySystem;

using Microsoft.AspNetCore.Http;

namespace Framework.Configurator.Handlers;

public record CreatePrincipalHandler(
        IAuthorizationRepositoryFactory<Principal> AuthorizationRepositoryFactory,
        IConfiguratorIntegrationEvents? ConfiguratorIntegrationEvents = null) : BaseWriteHandler, ICreatePrincipalHandler
{
    public async Task Execute(HttpContext context, CancellationToken cancellationToken)
    {
        var name = await this.ParseRequestBodyAsync<string>(context);
        var principal = new Principal { Name = name };

        await this.AuthorizationRepositoryFactory
                  .Create(BLLSecurityMode.Edit)
                  .SaveAsync(principal, cancellationToken);

        if (this.ConfiguratorIntegrationEvents != null)
        {
            await this.ConfiguratorIntegrationEvents.PrincipalCreatedAsync(principal, cancellationToken);
        }
    }
}