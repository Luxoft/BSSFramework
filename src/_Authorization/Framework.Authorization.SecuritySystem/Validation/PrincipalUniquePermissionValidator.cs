using FluentValidation;
using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.ExternalSource;
using Framework.Core;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PrincipalUniquePermissionValidator : AbstractValidator<Principal>
{
    private readonly IAuthorizationExternalSource externalSource;

    public const string Key = "UniquePermission";

    public PrincipalUniquePermissionValidator(IAuthorizationExternalSource externalSource)
    {
        var duplicatesVar = "Duplicates";

        var comparer = new EqualityComparerImpl<Permission>(this.IsDuplicate);

        this.externalSource = externalSource;

        this.RuleForEach(principal => principal.Permissions.GroupBy(permission => permission, comparer))
            .Must(
                (principal, g, context) =>
                {
                    context.MessageFormatter.AppendArgument(
                        duplicatesVar,
                        $"Principal \"{principal}\" has duplicate permissions ({this.GetFormattedPermission(g.Key)})");

                    return g.Count() == 1;
                })
            .WithMessage(principal => $"Principal {principal.Name} has duplicate permissions {{{duplicatesVar}}}");
    }

    protected virtual bool IsDuplicate(Permission permission, Permission otherPermission)
    {
        return permission.Role == otherPermission.Role
               && permission.Period.IsIntersected(otherPermission.Period)
               && permission.GetOrderedSecurityContextIdents().SequenceEqual(otherPermission.GetOrderedSecurityContextIdents());
    }

    protected virtual string GetFormattedPermission(Permission permission, string separator = " | ")
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        return this.GetPermissionVisualParts(permission).Join(separator);
    }

    private IEnumerable<string> GetPermissionVisualParts(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        yield return $"Role: {permission.Role}";

        yield return $"Period: {permission.Period}";

        foreach (var securityContextTypeGroup in permission.Restrictions.GroupBy(fi => fi.SecurityContextType, fi => fi.SecurityContextId))
        {
            var securityEntities = this.externalSource.GetTyped(securityContextTypeGroup.Key).GetSecurityEntitiesByIdents(securityContextTypeGroup);

            yield return $"{securityContextTypeGroup.Key.Name.ToPluralize()}: {securityEntities.Join(", ")}";
        }
    }
}
