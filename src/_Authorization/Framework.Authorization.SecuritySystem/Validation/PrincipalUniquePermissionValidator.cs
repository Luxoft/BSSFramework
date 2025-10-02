using CommonFramework;

using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Core;

using SecuritySystem;
using SecuritySystem.ExternalSystem.SecurityContextStorage;

namespace Framework.Authorization.SecuritySystemImpl.Validation;

public class PrincipalUniquePermissionValidator : AbstractValidator<Principal>, IPrincipalUniquePermissionValidator
{
    private readonly ISecurityContextStorage securityEntitySource;

    private readonly ISecurityContextInfoSource securityContextInfoSource;

    public PrincipalUniquePermissionValidator(ISecurityContextStorage securityEntitySource, ISecurityContextInfoSource securityContextInfoSource)
    {
        var duplicatesVar = "Duplicates";

        this.securityEntitySource = securityEntitySource;
        this.securityContextInfoSource = securityContextInfoSource;

        this.RuleFor(principal => principal.Permissions)
            .Must((_, permissions, context) =>
                  {
                      var duplicates = this.GetDuplicates(permissions).ToList();

                      context.MessageFormatter.AppendArgument(
                          duplicatesVar,
                          duplicates.Join(",", g => $"({this.GetFormattedPermission(g.Key)})"));

                      return !duplicates.Any();
                  })
            .WithMessage(principal => $"Principal \"{principal.Name}\" has duplicate permissions: {{{duplicatesVar}}}");
    }

    protected virtual IEnumerable<IGrouping<Permission, Permission>> GetDuplicates(IEnumerable<Permission> permissions)
    {
        var comparer = new EqualityComparerImpl<Permission>(this.IsDuplicate);

        return permissions.GroupBy(permission => permission, comparer).Where(g => g.Count() > 1);
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
            var securityContextInfo = this.securityContextInfoSource.GetSecurityContextInfo(securityContextTypeGroup.Key.Id);

            var securityEntities = this.securityEntitySource
                                       .GetTyped(securityContextInfo.Type)
                                       .Pipe(v => (ITypedSecurityContextStorage<Guid>)v)
                                       .GetSecurityContextsByIdents(securityContextTypeGroup);

            yield return $"{securityContextTypeGroup.Key.Name.ToPluralize()}: {securityEntities.Select(v => v.Name).Join(", ")}";
        }
    }
}
