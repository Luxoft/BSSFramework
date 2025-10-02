using CommonFramework;

using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Core;
using SecuritySystem;
using SecuritySystem.ExternalSystem.SecurityContextStorage;
using SecuritySystem.Services;

namespace Framework.Authorization.SecuritySystemImpl.Validation;

public class PermissionRestrictionValidator : AbstractValidator<PermissionRestriction>
{
    private readonly ISecurityContextInfoSource securityContextInfoSource;

    private readonly ISecurityContextSource securityContextSource;

    private readonly IIdentityInfoSource identityInfoSource;

    public PermissionRestrictionValidator(
        ISecurityContextInfoSource securityContextInfoSource,
        ISecurityRoleSource securityRoleSource,
        ISecurityContextStorage securityEntitySource,
        ISecurityContextSource securityContextSource,
        IIdentityInfoSource identityInfoSource)
    {
        this.securityContextInfoSource = securityContextInfoSource;
        this.securityContextSource = securityContextSource;
        this.identityInfoSource = identityInfoSource;

        this.RuleFor(permissionRestriction => permissionRestriction.SecurityContextType)
            .Must((permissionRestriction, securityContextType) =>
                  {
                      var securityRole = securityRoleSource.GetSecurityRole(permissionRestriction.Permission.Role.Id);

                      var securityContextInfo = this.GetSecurityContextInfo(securityContextType);

                      var allowedSecurityContexts = securityRole.Information.Restriction.SecurityContextTypes;

                      return allowedSecurityContexts == null || allowedSecurityContexts.Contains(securityContextInfo.Type);
                  })
            .WithMessage(permissionRestriction => $"Invalid SecurityContextType: {permissionRestriction.SecurityContextType.Name}.");

        this.RuleFor(permissionRestriction => permissionRestriction.SecurityContextId)
            .Must((permissionRestriction, securityContextId) =>
                  {
                      var securityContextTypeInfo =
                          securityContextInfoSource.GetSecurityContextInfo(permissionRestriction.SecurityContextType.Id);

                      var authorizationTypedExternalSource =
                          (ITypedSecurityContextStorage<Guid>)securityEntitySource.GetTyped(securityContextTypeInfo.Type);

                      return authorizationTypedExternalSource.IsExists(securityContextId);
                  })
            .WithMessage(permissionRestriction =>
                             $"{permissionRestriction.SecurityContextType.Name} with id '{permissionRestriction.SecurityContextId}' not exists.");

        this.RuleFor(permissionRestriction => permissionRestriction.SecurityContextType)
            .Must((permissionRestriction, securityContextType) =>
                  {
                      var securityRole = securityRoleSource.GetSecurityRole(permissionRestriction.Permission.Role.Id);

                      var securityContextInfo = this.GetSecurityContextInfo(securityContextType);

                      var securityContextRestriction =
                          securityRole.Information.Restriction.SecurityContextRestrictions?.SingleOrDefault(r => r.SecurityContextType
                              == securityContextInfo.Type);

                      var restrictionFilterInfo = securityContextRestriction?.RawFilter;

                      return restrictionFilterInfo == null
                             || this.IsAllowed(permissionRestriction.SecurityContextId, restrictionFilterInfo);
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
        var identityInfo = this.identityInfoSource.GetIdentityInfo<TSecurityContext, Guid>();

        return this.securityContextSource.GetQueryable(restrictionFilterInfo).Select(identityInfo.IdPath).Contains(securityContextId);
    }
}
