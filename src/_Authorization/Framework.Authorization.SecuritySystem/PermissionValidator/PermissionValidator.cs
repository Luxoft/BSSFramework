using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;
using Framework.Validation;

namespace Framework.Authorization.SecuritySystem;

public class PermissionValidator(
    TimeProvider timeProvider,
    IAuthorizationExternalSource externalSource,
    ISecurityContextInfoService securityContextInfoService,
    ISecurityRoleSource securityRoleSource)
    : IPermissionValidator
{
    public void Validate(Permission permission)
    {
        this.ValidateRestriction(permission);

        this.ValidateDelegation(permission);
    }

    public void ValidateRestriction(Permission permission)
    {
        var securityRole = securityRoleSource.GetSecurityRole(permission.Role.Id);

        var allowedSecurityContexts = securityRole.Information.Restriction.SecurityContexts;

        if (allowedSecurityContexts != null)
        {
            var usedTypes = permission.Restrictions.Select(pr => this.GetSecurityContextInfo(pr.SecurityContextType).Type);

            var invalidTypes = usedTypes.Except(allowedSecurityContexts).ToList();

            if (invalidTypes.Any())
            {
                throw new ValidationException($"Invalid permission restriction types: {invalidTypes.Join(", ", t => t.Name)}");
            }
        }
    }

    private void ValidateDelegation(Permission permission, ValidatePermissionDelegateMode mode = ValidatePermissionDelegateMode.All)
    {
        if (permission.IsDelegatedFrom)
        {
            this.ValidatePermissionDelegatedFrom(permission, mode);
        }

        this.ValidatePermissionDelegatedTo(permission, mode);
    }

    private void ValidatePermissionDelegatedTo(Permission permission, ValidatePermissionDelegateMode mode)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        foreach (var subPermission in permission.DelegatedTo)
        {
            this.ValidatePermissionDelegatedFrom(subPermission, mode);
        }
    }

    private void ValidatePermissionDelegatedFrom(Permission permission, ValidatePermissionDelegateMode mode)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        if (permission.DelegatedFrom == null)
        {
            throw new System.ArgumentException("is not delegated permission");
        }

        if (mode.HasFlag(ValidatePermissionDelegateMode.Period))
        {
            if (!this.IsCorrectPeriodSubset(permission))
            {
                throw new ValidationException(
                    $"Invalid delegated permission period. Selected period \"{permission.Period}\" not subset of \"{permission.DelegatedFrom.Period}\"");
            }
        }

        if (mode.HasFlag(ValidatePermissionDelegateMode.SecurityObjects) && timeProvider.IsActivePeriod(permission))
        {
            var invalidEntityGroups = this.GetInvalidDelegatedPermissionSecurities(permission).ToList();

            if (invalidEntityGroups.Any())
            {
                throw new ValidationException(
                    string.Format(
                        "Can't delegate permission from {0} to {1}, because {0} have no access to objects ({2})",
                        permission.DelegatedFromPrincipal.Name,
                        permission.Principal.Name,
                        invalidEntityGroups.Join(
                            " | ",
                            g => $"{g.Key.Name}: {g.Value.Join(", ", s => s.Name)}")));
            }
        }
    }

    private bool IsCorrectPeriodSubset(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var delegatedFromPermission = permission.DelegatedFrom.FromMaybe(() => new ValidationException("Permission not delegated"));

        return this.IsCorrectPeriodSubset(permission, delegatedFromPermission);
    }

    private bool IsCorrectPeriodSubset(Permission subPermission, Permission parentPermission)
    {
        if (subPermission == null) throw new ArgumentNullException(nameof(subPermission));
        if (parentPermission == null) throw new ArgumentNullException(nameof(parentPermission));

        return subPermission.Period.IsEmpty || parentPermission.Period.Contains(subPermission.Period);
    }

    private Dictionary<SecurityContextType, IEnumerable<SecurityEntity>> GetInvalidDelegatedPermissionSecurities(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var delegatedFromPermission = permission.DelegatedFrom.FromMaybe(() => new ValidationException("Permission not delegated"));

        return this.GetInvalidDelegatedPermissionSecurities(permission, delegatedFromPermission);
    }

    private Dictionary<SecurityContextType, IEnumerable<SecurityEntity>> GetInvalidDelegatedPermissionSecurities(
        Permission subPermission,
        Permission parentPermission)
    {
        if (subPermission == null) throw new ArgumentNullException(nameof(subPermission));
        if (parentPermission == null) throw new ArgumentNullException(nameof(parentPermission));

        var allowedEntitiesRequest = from filterItem in parentPermission.Restrictions

                                     group filterItem.SecurityContextId by filterItem.SecurityContextType;

        var allowedEntitiesDict = allowedEntitiesRequest.ToDictionary(g => g.Key, g => g.ToList());

        var requiredEntitiesRequest = (from filterItem in subPermission.Restrictions

                                       group filterItem.SecurityContextId by filterItem.SecurityContextType).ToArray();

        var invalidRequest1 = from requiredGroup in requiredEntitiesRequest

                              let allSecurityEntities = externalSource.GetTyped(requiredGroup.Key).GetSecurityEntities()

                              let securityContextType = requiredGroup.Key

                              let preAllowedEntities = allowedEntitiesDict.GetValueOrDefault(securityContextType).Maybe(v => v.Distinct())

                              where preAllowedEntities != null // доступны все

                              let allowedEntities = this.IsExpandable(securityContextType)
                                                        ? preAllowedEntities.Distinct()
                                                                            .GetAllElements(
                                                                                allowedEntityId =>
                                                                                    allSecurityEntities.Where(
                                                                                            v => v.ParentId == allowedEntityId)
                                                                                        .Select(v => v.Id))
                                                                            .Distinct()
                                                                            .ToList()

                                                        : preAllowedEntities.Distinct().ToList()

                              from securityContextId in requiredGroup

                              let securityObject = allSecurityEntities.SingleOrDefault(v => v.Id == securityContextId)

                              where securityObject != null // Протухшая безопасность

                              let hasAccess = allowedEntities.Contains(securityContextId)

                              where !hasAccess

                              group securityObject by securityContextType
                              into g

                              let key = g.Key

                              let value = (IEnumerable<SecurityEntity>)g

                              select key.ToKeyValuePair(value);

        var invalidRequest2 = from securityContextType in allowedEntitiesDict.Keys

                              join requeredGroup in requiredEntitiesRequest on securityContextType equals requeredGroup.Key into g

                              where !g.Any()

                              let key = securityContextType

                              let value = (IEnumerable<SecurityEntity>)new[] { new SecurityEntity { Name = "[Not Selected Element]" } }

                              select key.ToKeyValuePair(value);

        return invalidRequest1.Concat(invalidRequest2).ToDictionary();
    }

    private ISecurityContextInfo GetSecurityContextInfo(SecurityContextType securityContextType)
    {
        return securityContextInfoService.GetSecurityContextInfo(securityContextType.Name);
    }

    private bool IsExpandable(SecurityContextType securityContextType)
    {
        return this.GetSecurityContextInfo(securityContextType).Type.IsHierarchical();
    }
}
