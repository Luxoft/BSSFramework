﻿using Framework.SecuritySystem;

namespace Framework.Authorization;

public static class AuthorizationSecurityOperation
{
    public static SecurityOperation PrincipalOpenModule { get; } = new(nameof(PrincipalOpenModule)) { Description = "Can open Principal module" };

    public static SecurityOperation PrincipalView { get; } = new(nameof(PrincipalView)) { Description = "Can view Principal" };

    public static SecurityOperation PrincipalEdit { get; } = new(nameof(PrincipalEdit)) { Description = "Can edit Principal" };


    public static SecurityOperation BusinessRoleOpenModule { get; } = new(nameof(BusinessRoleOpenModule)) { Description = "Can open BusinessRole module" };

    public static SecurityOperation BusinessRoleView { get; } = new(nameof(BusinessRoleView)) { Description = "Can view BusinessRole" };

    public static SecurityOperation BusinessRoleEdit { get; } = new(nameof(BusinessRoleEdit)) { Description = "Can edit BusinessRole" };


    public static SecurityOperation AuthorizationImpersonate { get; } = new(nameof(AuthorizationImpersonate)) { Description = "Can authorization impersonate" };
}
