using FluentValidation;

using Framework.Authorization.Domain;
using Framework.DomainDriven.Repository;
using Framework.SecuritySystem;

using Microsoft.Extensions.DependencyInjection;

using NHibernate.Linq;

namespace Framework.Authorization.SecuritySystem.Validation;

public class PrincipalNameValidator : AbstractValidator<Principal>
{
    public const string Key = "PrincipalName";

    public PrincipalNameValidator([FromKeyedServices(nameof(SecurityRule.Disabled))] IRepository<Principal> principalRepository)
    {
        this.RuleFor(p => p.Name)
            .MustAsync(
                async (principal, name, cancellationToken) =>
                {
                    return !principal.Active
                           || await principalRepository.GetQueryable()
                                                       .AllAsync(
                                                           otherPrincipal => otherPrincipal == principal
                                                                             || !otherPrincipal.Active
                                                                             || otherPrincipal.Name != name,
                                                           cancellationToken);
                })
            .WithMessage(principal => $"Active principal with name '{principal.Name}' already exists.");
    }
}
