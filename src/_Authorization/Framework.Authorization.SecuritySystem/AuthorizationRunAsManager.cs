﻿using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Core.Services;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;
using Framework.SecuritySystem.Credential;
using Framework.SecuritySystem.Services;
using Framework.SecuritySystem.UserSource;

namespace Framework.Authorization.SecuritySystem;

public class AuthorizationRunAsManager(
    IUserAuthenticationService userAuthenticationService,
    ISecuritySystemFactory securitySystemFactory,
    ICurrentPrincipalSource currentPrincipalSource,
    [DisabledSecurity] IRepository<Principal> principalRepository,
    IPrincipalResolver principalResolver,
    IEnumerable<IRunAsValidator> validators)
    : RunAsManager(userAuthenticationService, securitySystemFactory)
{
    private Principal CurrentPrincipal => currentPrincipalSource.CurrentPrincipal;

    public override User? RunAsUser =>
        this.CurrentPrincipal.RunAs.Maybe(runAsPrincipal => new User(runAsPrincipal.Id, runAsPrincipal.Name));

    protected override async Task PersistRunAs(UserCredential? userCredential, CancellationToken cancellationToken)
    {
        if (userCredential != null)
        {
            validators.Foreach(validator => validator.Validate(userCredential));
        }

        this.CurrentPrincipal.RunAs =
            userCredential == null
                ? null
                : await principalResolver.Resolve(userCredential, cancellationToken);

        await principalRepository.SaveAsync(this.CurrentPrincipal, cancellationToken);
    }
}
