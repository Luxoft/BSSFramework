using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PermissionRestrictionValidator : AbstractValidator<PermissionRestriction>
{
    private readonly ISecurityContextInfoService securityContextInfoService;

    public PermissionRestrictionValidator(
        ISecurityContextInfoService securityContextInfoService,
        ISecurityRoleSource securityRoleSource,
        IAuthorizationExternalSource authorizationExternalSource)
    {
        this.securityContextInfoService = securityContextInfoService;

        this.RuleFor(permissionRestriction => permissionRestriction.SecurityContextType)
            .Must(
                (permissionRestriction, securityContextType) =>
                {
                    var securityRole = securityRoleSource.GetSecurityRole(permissionRestriction.Permission.Role.Id);

                    var securityContextInfo = this.GetSecurityContextInfo(securityContextType);

                    var allowedSecurityContexts = securityRole.Information.Restriction.SecurityContexts;

                    return allowedSecurityContexts == null || allowedSecurityContexts.Contains(securityContextInfo.Type);
                })
            .WithMessage(permissionRestriction => $"Invalid SecurityContextType: {permissionRestriction.SecurityContextType.Name}");

        this.RuleFor(permissionRestriction => permissionRestriction.SecurityContextId)
            .Must(
                (permissionRestriction, securityContextId) =>
                {
                    var securityContextInfo = this.GetSecurityContextInfo(permissionRestriction.SecurityContextType);

                    var authorizationTypedExternalSource = authorizationExternalSource.GetTyped(permissionRestriction.SecurityContextType);

                    return authorizationTypedExternalSource.GetSecurityEntities() allowedSecurityContexts == null || allowedSecurityContexts.Contains(securityContextInfo.Type);
                })
            .WithMessage(permissionRestriction => $"{permissionRestriction.SecurityContextType.Name} with id '{permissionRestriction.SecurityContextType.Name}' not exists");
    }

    private ISecurityContextInfo GetSecurityContextInfo(SecurityContextType securityContextType)
    {
        return this.securityContextInfoService.GetSecurityContextInfo(securityContextType.Name);
    }
}
