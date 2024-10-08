﻿using Framework.SecuritySystem;

namespace Framework.DomainDriven.ApplicationCore.Security;

public static class SecuritySystemExtensions
{
    public static bool IsSecurityAdministrator(this ISecuritySystem securitySystem) =>
        securitySystem.HasAccess(ApplicationSecurityRule.SecurityAdministrator);
}
