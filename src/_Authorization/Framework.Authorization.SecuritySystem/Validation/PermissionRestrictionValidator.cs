﻿using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.SecuritySystem;
using Framework.SecuritySystem.ExternalSystem.SecurityContextStorage;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PermissionRestrictionValidator : AbstractValidator<PermissionRestriction>
{
    private readonly ISecurityContextInfoSource securityContextInfoSource;

    private readonly ISecurityContextSource securityContextSource;

    public PermissionRestrictionValidator(
        ISecurityContextInfoSource securityContextInfoSource,
        ISecurityRoleSource securityRoleSource,
        ISecurityContextStorage securityEntitySource,
        ISecurityContextSource securityContextSource)
    {
        this.securityContextInfoSource = securityContextInfoSource;
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
            .WithMessage(
                permissionRestriction =>
                    $"{permissionRestriction.SecurityContextType.Name} with id '{permissionRestriction.SecurityContextId}' not exists.");

        this.RuleFor(permissionRestriction => permissionRestriction.SecurityContextType)
            .Must(
                (permissionRestriction, securityContextType) =>
                {
                    var securityRole = securityRoleSource.GetSecurityRole(permissionRestriction.Permission.Role.Id);

                    var securityContextInfo = this.GetSecurityContextInfo(securityContextType);

                    var securityContextRestriction =
                        securityRole.Information.Restriction.SecurityContextRestrictions?.SingleOrDefault(
                            r => r.SecurityContextType == securityContextInfo.Type);

                    var restrictionFilterInfo = securityContextRestriction?.RawFilter;

                    return restrictionFilterInfo == null || this.IsAllowed(permissionRestriction.SecurityContextId, restrictionFilterInfo);
                })
            .WithMessage(permissionRestriction => $"SecurityContext: '{permissionRestriction.SecurityContextId}' denied by filter.");
    }

    private SecurityContextInfo GetSecurityContextInfo(SecurityContextType securityContextType)
    {
        return this.securityContextInfoSource.GetSecurityContextInfo(securityContextType.Id);
    }

    private bool IsAllowed(Guid securityContextId, SecurityContextRestrictionFilterInfo restrictionFilterInfo)
    {
        return new Func<Guid, SecurityContextRestrictionFilterInfo<ISecurityContext>, bool>(this.IsAllowed)
               .CreateGenericMethod(restrictionFilterInfo.SecurityContextType)
               .Invoke<bool>(this, securityContextId, restrictionFilterInfo);
    }

    private bool IsAllowed<TSecurityContext>(
        Guid securityContextId,
        SecurityContextRestrictionFilterInfo<TSecurityContext> restrictionFilterInfo)
        where TSecurityContext : class, ISecurityContext
    {
        return this.securityContextSource.GetQueryable(restrictionFilterInfo)
                   .Any(securityContext => securityContext.Id == securityContextId);
    }
}
