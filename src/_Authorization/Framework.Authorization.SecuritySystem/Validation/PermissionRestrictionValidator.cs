using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PermissionRestrictionValidator : AbstractValidator<Permission>
{
    public const string Key = "InvalidRestrictionTypes";


    private readonly ISecurityContextInfoService securityContextInfoService;

    public PermissionRestrictionValidator(
        ISecurityContextInfoService securityContextInfoService,
        ISecurityRoleSource securityRoleSource)
    {
        this.securityContextInfoService = securityContextInfoService;

        this.RuleFor(permission => permission.Role)
            .Must((permission, role, context) =>
                  {
                      var securityRole = securityRoleSource.GetSecurityRole(role.Id);

                      var allowedSecurityContexts = securityRole.Information.Restriction.SecurityContexts;

                      if (allowedSecurityContexts != null)
                      {
                          var usedTypes = permission.Restrictions.Select(pr => this.GetSecurityContextInfo(pr.SecurityContextType).Type);

                          var invalidTypes = usedTypes.Except(allowedSecurityContexts).ToList();

                          if (invalidTypes.Any())
                          {
                              context.MessageFormatter.AppendArgument(Key, invalidTypes.Join(", ", t => t.Name));
                              return false;
                          }
                      }

                      return true;
                  })
            .WithMessage($"Invalid permission restriction types: {{{Key}}}");
    }

    private ISecurityContextInfo GetSecurityContextInfo(SecurityContextType securityContextType)
    {
        return this.securityContextInfoService.GetSecurityContextInfo(securityContextType.Name);
    }
}
