using Framework.SecuritySystem;

namespace Framework.Authorization;

public static class AuthorizationSecurityOperation
{
    public static DisabledSecurityOperation Disabled { get; } = SecurityOperation.Disabled;


    public static SecurityOperation<Guid> PrincipalOpenModule { get; } = new(nameof(PrincipalOpenModule), new Guid("5e72e6a4-33bb-4f0f-8928-0c2d44429cc6")) { Description = "Can open Principal module", IsClient = true };

    public static SecurityOperation<Guid> PrincipalView { get; } = new(nameof(PrincipalView), new Guid("5031a272-b730-4e65-9d56-50b3e0441c4f")) { Description = "Can view Principal" };

    public static SecurityOperation<Guid> PrincipalEdit { get; } = new(nameof(PrincipalEdit), new Guid("3dc58d7b-85a0-43e1-8e54-e23f9a360e7b")) { Description = "Can edit Principal" };


    public static SecurityOperation<Guid> BusinessRoleOpenModule { get; } = new(nameof(BusinessRoleOpenModule), new Guid("76477c2a-c0ef-4cc3-be15-1087aac9ae6e")) { Description = "Can open BusinessRole module", IsClient = true };

    public static SecurityOperation<Guid> BusinessRoleView { get; } = new(nameof(BusinessRoleView), new Guid("641cb98e-f47d-4f4a-9183-d969ba1434d4")) { Description = "Can view BusinessRole" };

    public static SecurityOperation<Guid> BusinessRoleEdit { get; } = new(nameof(BusinessRoleEdit), new Guid("44f519ae-acd8-487b-94ba-bbd897df687a")) { Description = "Can edit BusinessRole" };


    public static SecurityOperation<Guid> OperationOpenModule { get; } = new(nameof(OperationOpenModule), new Guid("e50e7f5a-f042-4239-ba57-1d34c084f42f")) { Description = "Can open Operation module", IsClient = true };

    public static SecurityOperation<Guid> OperationView { get; } = new(nameof(OperationView), new Guid("9b0f5a86-5ecc-44db-b325-8f2fcd7c2e46")) { Description = "Can view Operation" };

    public static SecurityOperation<Guid> OperationEdit { get; } = new(nameof(OperationEdit), new Guid("7d148dad-d4f7-45e3-b087-2125abcd8a58")) { Description = "Can edit Operation" };


    public static SecurityOperation<Guid> AuthorizationImpersonate { get; } = new(nameof(AuthorizationImpersonate), new Guid("e48d7030-fc38-4416-8c7f-f08764d884e3")) { Description = "Can authorization impersonate" };
}
