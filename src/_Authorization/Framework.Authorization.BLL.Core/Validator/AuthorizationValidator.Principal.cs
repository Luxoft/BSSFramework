using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Validation;

namespace Framework.Authorization.BLL;

public partial class AuthorizationValidator
{
    protected override ValidationResult GetPrincipalValidationResult(Principal principal, AuthorizationOperationContext operationContext, IValidationState ownerState)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        var realOperationContext = operationContext.HasFlag(AuthorizationOperationContext.Save) ? operationContext | AuthorizationOperationContext.SavePrincipal : operationContext;

        return base.GetPrincipalValidationResult(principal, realOperationContext, ownerState)
               + (realOperationContext.HasFlag(AuthorizationOperationContext.SavePrincipal) ? this.GetUniquePermissionsValidationResult(principal) : ValidationResult.Success)
               + this.GetActivePrincipalValidationResult(principal);
    }

    /// <summary>
    /// Проверка уникальности пермишшионов в принципале
    /// </summary>
    /// <param name="principal">Проверяемый принципал</param>
    /// <returns></returns>
    protected virtual ValidationResult GetUniquePermissionsValidationResult(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        return principal.Permissions.GroupBy(permission => permission, new EqualityComparerImpl<Permission>((permission, otherPermission) => permission.IsDuplicate(otherPermission)))
                        .Select(g => ValidationResult.FromCondition(g.Count() == 1, () => $"Principal \"{principal}\" has duplicate permissions ({this.Context.GetFormattedPermission(g.Key)})"))
                        .Sum();
    }

    /// <summary>
    /// Проверка активности единственного принципала с одним именем
    /// </summary>
    /// <param name="principal">Проверяемый принципал</param>
    /// <returns></returns>
    protected virtual ValidationResult GetActivePrincipalValidationResult(Principal principal)
    {
        var exists = this.Context.Logics.Principal.GetUnsecureQueryable()
                         .Any(p => p.Id != principal.Id && p.Name == principal.Name && p.Active);

        return ValidationResult.FromCondition(!exists, () => $"Active principal with name '{principal.Name}' already exists.");
    }
}
