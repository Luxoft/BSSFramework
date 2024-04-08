using Framework.SecuritySystem;

namespace Framework.Authorization;

public static class AuthorizationSecurityOperation
{
    public static SecurityRule PrincipalOpenModule { get; } = new(nameof(PrincipalOpenModule)) { Description = "Can open Principal module" };

    public static SecurityRule PrincipalView { get; } = new(nameof(PrincipalView)) { Description = "Can view Principal" };

    public static SecurityRule PrincipalEdit { get; } = new(nameof(PrincipalEdit)) { Description = "Can edit Principal" };


    public static SecurityRule BusinessRoleOpenModule { get; } = new(nameof(BusinessRoleOpenModule)) { Description = "Can open BusinessRole module" };

    public static SecurityRule BusinessRoleView { get; } = new(nameof(BusinessRoleView)) { Description = "Can view BusinessRole" };

    public static SecurityRule BusinessRoleEdit { get; } = new(nameof(BusinessRoleEdit)) { Description = "Can edit BusinessRole" };


    public static SecurityRule OperationOpenModule { get; } = new(nameof(OperationOpenModule)) { Description = "Can open Operation module" };

    public static SecurityRule OperationView { get; } = new(nameof(OperationView)) { Description = "Can view Operation" };

    public static SecurityRule OperationEdit { get; } = new(nameof(OperationEdit)) { Description = "Can edit Operation" };


    public static SecurityRule AuthorizationImpersonate { get; } = new(nameof(AuthorizationImpersonate)) { Description = "Can authorization impersonate" };
}
