using Framework.Authorization.Domain;
using Framework.Validation;

namespace Framework.Authorization.BLL;

public partial class AuthorizationValidator
{
    protected override ValidationResult GetPermissionValidationResult(Permission permission, AuthorizationOperationContext operationContext, IValidationState ownerState)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var baseResult = base.GetPermissionValidationResult(permission, operationContext, ownerState);

        return baseResult
               + (operationContext.HasFlag(AuthorizationOperationContext.SavePrincipal) ? ValidationResult.Success : this.GetUniqueValidationResult(permission)); // Если идёт сохрание принципала целиком, то проверка уникальности пермиссий производится выше и тут избыточна
    }

    /// <summary>
    /// Проверка уникальности пермишшиона в содержащем его принципале
    /// </summary>
    /// <param name="permission">Проверяемый пермишшион</param>
    /// <returns></returns>
    protected virtual ValidationResult GetUniqueValidationResult(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var otherPermissions = permission.Principal.Permissions.Except(new[] { permission });

        var duplicates = otherPermissions.Where(otherPermission => otherPermission.IsDuplicate(permission));

        return ValidationResult.FromCondition(!duplicates.Any(), () => $"Principal \"{permission.Principal}\" has duplicate permissions ({this.Context.GetFormattedPermission(permission)})");
    }
}
