﻿using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.DomainDriven.VirtualPermission;

public class VirtualPermissionSystemFactory<TPrincipal, TPermission> : IPermissionSystemFactory
    where TPrincipal : IIdentityObject<Guid>
    where TPermission : IIdentityObject<Guid>
{
    private readonly IServiceProvider serviceProvider;

    private readonly VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo;

    public VirtualPermissionSystemFactory(
        IServiceProvider serviceProvider,
        ISecurityRoleSource securityRoleSource,
        VirtualPermissionBindingInfo<TPrincipal, TPermission> bindingInfo)
    {
        this.serviceProvider = serviceProvider;
        this.bindingInfo = bindingInfo;

        this.bindingInfo.Validate(securityRoleSource);
    }

    public IPermissionSystem Create(SecurityRuleCredential securityRuleCredential) =>
        ActivatorUtilities.CreateInstance<VirtualPermissionSystem<TPrincipal, TPermission>>(
            this.serviceProvider,
            this.bindingInfo,
            securityRuleCredential);
}