using CommonFramework;

using Framework.Authorization.Domain;
using Framework.Core;

using HierarchicalExpand;

using SecuritySystem;
using SecuritySystem.ExternalSystem.Management;
using SecuritySystem.ExternalSystem.SecurityContextStorage;
using SecuritySystem.GeneralPermission.Validation.Permission;
using SecuritySystem.Validation;

namespace Framework.Authorization.Environment.Validation;

public class PermissionDelegateValidator(
    IHierarchicalInfoSource hierarchicalInfoSource,
    TimeProvider timeProvider,
    ISecurityContextStorage securityEntitySource,
    ISecurityContextInfoSource securityContextInfoSource,
    ISecurityRoleSource securityRoleSource)
    : IPermissionValidator<Permission, PermissionRestriction>
{
    public async Task ValidateAsync(PermissionData<Permission, PermissionRestriction> permissionData, CancellationToken cancellationToken)
    {
        var permission = permissionData.Permission;

        var delegatedFrom = permission.DelegatedFrom;

        if (delegatedFrom != null)
        {
            if (delegatedFrom.Principal == permission.Principal)
            {
                throw new SecuritySystemValidationException("Permission cannot be delegated to the original user");
            }

            try
            {
                this.Validate(permission, ValidatePermissionDelegateMode.All);
            }
            catch (Exception ex)
            {
                throw new SecuritySystemValidationException(ex.Message);
            }
        }
    }

    private void Validate(Permission permission, ValidatePermissionDelegateMode mode)
    {
        if (permission.DelegatedFrom != null)
        {
            this.ValidatePermissionDelegatedFrom(permission, permission.DelegatedFrom, mode);
        }

        this.ValidatePermissionDelegatedTo(permission, mode);
    }

    private void ValidatePermissionDelegatedTo(Permission permission, ValidatePermissionDelegateMode mode)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        foreach (var subPermission in permission.DelegatedTo)
        {
            this.ValidatePermissionDelegatedFrom(subPermission, permission, mode);
        }
    }

    private void ValidatePermissionDelegatedFrom(Permission permission, Permission delegatedFrom, ValidatePermissionDelegateMode mode)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));
        if (delegatedFrom == null) throw new ArgumentNullException(nameof(delegatedFrom));

        if (mode.HasFlag(ValidatePermissionDelegateMode.Role))
        {
            if (!this.IsCorrectRoleSubset(permission, delegatedFrom))
            {
                throw new SecuritySystemValidationException(
                    $"Invalid delegated permission role. Selected role \"{permission.Role}\" not subset of \"{delegatedFrom.Role}\"");
            }
        }

        if (mode.HasFlag(ValidatePermissionDelegateMode.Period))
        {
            if (!this.IsCorrectPeriodSubset(permission, delegatedFrom))
            {
                throw new SecuritySystemValidationException(
                    $"Invalid delegated permission period. Selected period \"{permission.Period}\" not subset of \"{delegatedFrom.Period}\"");
            }
        }

        if (mode.HasFlag(ValidatePermissionDelegateMode.SecurityObjects) && permission.Period.Contains(timeProvider.GetToday()))
        {
            var invalidEntityGroups = this.GetInvalidDelegatedPermissionSecurities(permission, delegatedFrom).ToList();

            if (invalidEntityGroups.Any())
            {
                throw new SecuritySystemValidationException(
                    string.Format(
                        "Can't delegate permission from {0} to {1}, because {0} have no access to objects ({2})",
                        delegatedFrom.Principal.Name,
                        permission.Principal.Name,
                        invalidEntityGroups.Join(
                            " | ",
                            g => $"{g.Key.Name}: {g.Value.Join(", ", s => s.Name)}")));
            }
        }
    }

    private bool IsCorrectRoleSubset(Permission subPermission, Permission delegatedFrom)
    {
        if (subPermission == null) throw new ArgumentNullException(nameof(subPermission));
        if (delegatedFrom == null) throw new ArgumentNullException(nameof(delegatedFrom));

        return securityRoleSource
                   .GetSecurityRole(delegatedFrom.Role.Id)
                   .GetAllElements(role => role.Information.Children.Select(subRole => securityRoleSource.GetSecurityRole(subRole)))
                   .Contains(securityRoleSource.GetSecurityRole(subPermission.Role.Id));
    }

    private bool IsCorrectPeriodSubset(Permission subPermission, Permission delegatedFrom)
    {
        if (subPermission == null) throw new ArgumentNullException(nameof(subPermission));
        if (delegatedFrom == null) throw new ArgumentNullException(nameof(delegatedFrom));

        return subPermission.Period.IsEmpty || delegatedFrom.Period.Contains(subPermission.Period);
    }

    private Dictionary<SecurityContextType, IEnumerable<SecurityContextData<Guid>>> GetInvalidDelegatedPermissionSecurities(
        Permission subPermission,
        Permission delegatedFrom)
    {
        if (subPermission == null) throw new ArgumentNullException(nameof(subPermission));
        if (delegatedFrom == null) throw new ArgumentNullException(nameof(delegatedFrom));

        var allowedEntitiesRequest = from filterItem in delegatedFrom.Restrictions

                                     group filterItem.SecurityContextId by filterItem.SecurityContextType;

        var allowedEntitiesDict = allowedEntitiesRequest.ToDictionary(g => g.Key, g => g.ToList());

        var requiredEntitiesRequest = (from filterItem in subPermission.Restrictions

                                       group filterItem.SecurityContextId by filterItem.SecurityContextType).ToArray();

        var invalidRequest1 = from requiredGroup in requiredEntitiesRequest

                              let securityContextTypeInfo = securityContextInfoSource.GetSecurityContextInfo(requiredGroup.Key.Id)

                              let allSecurityContexts = securityEntitySource
                                                            .GetTyped(securityContextTypeInfo.Type)
                                                            .Pipe(v => (ITypedSecurityContextStorage<Guid>)v)
                                                            .GetSecurityContexts()

                              let securityContextType = requiredGroup.Key

                              let preAllowedEntities = allowedEntitiesDict.GetValueOrDefault(securityContextType).Maybe(v => v.Distinct())

                              where preAllowedEntities != null // доступны все

                              let allowedEntities = this.IsExpandable(securityContextType)
                                                        ? preAllowedEntities.Distinct()
                                                                            .GetAllElements(allowedEntityId =>
                                                                                                allSecurityContexts.Where(v => v.ParentId == allowedEntityId)
                                                                                                                   .Select(v => v.Id))
                                                                            .Distinct()
                                                                            .ToList()

                                                        : preAllowedEntities.Distinct().ToList()

                              from securityContextId in requiredGroup

                              let securityObject = allSecurityContexts.SingleOrDefault(v => v.Id == securityContextId)

                              where securityObject != null // Протухшая безопасность

                              let hasAccess = allowedEntities.Contains(securityContextId)

                              where !hasAccess

                              group securityObject by securityContextType

                              into g

                              let key = g.Key

                              let value = (IEnumerable<SecurityContextData<Guid>>)g

                              select (key, value);

        var invalidRequest2 = from securityContextType in allowedEntitiesDict.Keys

                              join requiredGroup in requiredEntitiesRequest on securityContextType equals requiredGroup.Key into g

                              where !g.Any()

                              let key = securityContextType

                              let value = (IEnumerable<SecurityContextData<Guid>>)
                                  [new SecurityContextData<Guid>(Guid.Empty, "[Not Selected Element]", Guid.Empty)]

                              select (key, value);

        return invalidRequest1.Concat(invalidRequest2).ToDictionary();
    }

    private SecurityContextInfo GetSecurityContextInfo(SecurityContextType securityContextType)
    {
        return securityContextInfoSource.GetSecurityContextInfo(securityContextType.Id);
    }

    private bool IsExpandable(SecurityContextType securityContextType)
    {
        return hierarchicalInfoSource.IsHierarchical(this.GetSecurityContextInfo(securityContextType).Type);
    }
}
