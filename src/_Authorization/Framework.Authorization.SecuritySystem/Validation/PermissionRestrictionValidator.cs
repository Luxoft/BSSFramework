using FluentValidation;

using Framework.Authorization.Domain;
using Framework.DomainDriven.ApplicationCore.ExternalSource;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PermissionRestrictionValidator : AbstractValidator<PermissionRestriction>
{
    private readonly ISecurityContextSource securityContextSource;

    public PermissionRestrictionValidator(
        ISecurityContextSource securityContextSource,
        ISecurityRoleSource securityRoleSource,
        ISecurityEntitySource securityEntitySource)
    {
        this.securityContextSource = securityContextSource;

        this.RuleFor(permissionRestriction => permissionRestriction.SecurityContextType)
            .Must(
                (permissionRestriction, securityContextType) =>
                {
                    var securityRole = securityRoleSource.GetSecurityRole(permissionRestriction.Permission.Role.Id);

                    var securityContextInfo = this.GetSecurityContextInfo(securityContextType);

                    var allowedSecurityContexts = securityRole.Information.Restriction.SecurityContextTypes;

                    return allowedSecurityContexts == null || allowedSecurityContexts.Contains(securityContextInfo.Type);
                })
            .WithMessage(permissionRestriction => $"Invalid SecurityContextType: {permissionRestriction.SecurityContextType.Name}.");

        this.RuleFor(permissionRestriction => permissionRestriction.SecurityContextId)
            .Must(
                (permissionRestriction, securityContextId) =>
                {
                    var authorizationTypedExternalSource = securityEntitySource.GetTyped(permissionRestriction.SecurityContextType.Id);

                    return authorizationTypedExternalSource.IsExists(securityContextId);
                })
            .WithMessage(permissionRestriction => $"{permissionRestriction.SecurityContextType.Name} with id '{permissionRestriction.SecurityContextId}' not exists.");
    }

    private SecurityContextInfo GetSecurityContextInfo(SecurityContextType securityContextType)
    {
        return this.securityContextSource.GetSecurityContextInfo(securityContextType.Id);
    }
}
