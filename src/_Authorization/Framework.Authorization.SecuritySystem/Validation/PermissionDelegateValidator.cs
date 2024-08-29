using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Core;
using Framework.Persistent;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PermissionDelegateValidator : AbstractValidator<Permission>
{
    public const string Key = "PermissionDelegate";


    private readonly TimeProvider timeProvider;

    private readonly IAuthorizationExternalSource externalSource;

    private readonly ISecurityContextSource securityContextSource;

    private readonly ISecurityRoleSource securityRoleSource;

    public PermissionDelegateValidator(
        TimeProvider timeProvider,
        IAuthorizationExternalSource externalSource,
        ISecurityContextSource securityContextSource,
        ISecurityRoleSource securityRoleSource)
    {
        this.timeProvider = timeProvider;
        this.externalSource = externalSource;
        this.securityContextSource = securityContextSource;
        this.securityRoleSource = securityRoleSource;

        this.RuleFor(permission => permission.DelegatedFrom)
            .Must((permission, delegatedFrom) => delegatedFrom?.Principal != permission.Principal)
            .WithMessage("Permission cannot be delegated to the original user");

        this.RuleFor(permission => permission.DelegatedFrom)
            .Must(
                (permission, delegatedFrom, context) =>
                {
                    try
                    {
                        this.Validate(permission, ValidatePermissionDelegateMode.All);

                        return true;
                    }
                    catch (Exception ex)
                    {
                        context.MessageFormatter.AppendArgument("ExceptionMessage", ex.Message);

                        return false;
                    }
                })
            .WithMessage("{ExceptionMessage}");
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
                throw new ValidationException(
                    $"Invalid delegated permission role. Selected role \"{permission.Role}\" not subset of \"{delegatedFrom.Role}\"");
            }
        }

        if (mode.HasFlag(ValidatePermissionDelegateMode.Period))
        {
            if (!this.IsCorrectPeriodSubset(permission, delegatedFrom))
            {
                throw new ValidationException(
                    $"Invalid delegated permission period. Selected period \"{permission.Period}\" not subset of \"{delegatedFrom.Period}\"");
            }
        }

        if (mode.HasFlag(ValidatePermissionDelegateMode.SecurityObjects) && this.timeProvider.IsActivePeriod(permission))
        {
            var invalidEntityGroups = this.GetInvalidDelegatedPermissionSecurities(permission, delegatedFrom).ToList();

            if (invalidEntityGroups.Any())
            {
                throw new ValidationException(
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

        return this.securityRoleSource
                   .GetSecurityRole(delegatedFrom.Role.Id)
                   .GetAllElements(role => role.Information.Children.Select(subRole => this.securityRoleSource.GetSecurityRole(subRole)))
                   .Contains(this.securityRoleSource.GetSecurityRole(subPermission.Role.Id));
    }

    private bool IsCorrectPeriodSubset(Permission subPermission, Permission delegatedFrom)
    {
        if (subPermission == null) throw new ArgumentNullException(nameof(subPermission));
        if (delegatedFrom == null) throw new ArgumentNullException(nameof(delegatedFrom));

        return subPermission.Period.IsEmpty || delegatedFrom.Period.Contains(subPermission.Period);
    }

    private Dictionary<SecurityContextType, IEnumerable<SecurityEntity>> GetInvalidDelegatedPermissionSecurities(
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

                              let allSecurityEntities = this.externalSource.GetTyped(requiredGroup.Key).GetSecurityEntities()

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

                              select (key, value);

        var invalidRequest2 = from securityContextType in allowedEntitiesDict.Keys

                              join requeredGroup in requiredEntitiesRequest on securityContextType equals requeredGroup.Key into g

                              where !g.Any()

                              let key = securityContextType

                              let value = (IEnumerable<SecurityEntity>)new[] { new SecurityEntity { Name = "[Not Selected Element]" } }

                              select (key, value);

        return invalidRequest1.Concat(invalidRequest2).ToDictionary();
    }

    private SecurityContextInfo GetSecurityContextInfo(SecurityContextType securityContextType)
    {
        return this.securityContextSource.GetSecurityContextInfo(securityContextType.Id);
    }

    private bool IsExpandable(SecurityContextType securityContextType)
    {
        return this.GetSecurityContextInfo(securityContextType).Type.IsHierarchical();
    }
}
