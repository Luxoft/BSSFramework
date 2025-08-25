using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem.Validation;
using Framework.Core;
using Framework.DomainDriven.Repository;
using Framework.Exceptions;
using Framework.Persistent;

using SecuritySystem.Attributes;
using SecuritySystem.UserSource;

namespace Framework.Authorization.SecuritySystem;

public class PrincipalDomainService(
    [DisabledSecurity] IRepository<Principal> principalRepository,
    IPrincipalGeneralValidator principalGeneralValidator,
    IUserSource? userSource = null) : IPrincipalDomainService
{
    public async Task<Principal> GetOrCreateAsync(string name, CancellationToken cancellationToken)
    {
        var principal = principalRepository.GetQueryable().SingleOrDefault(p => p.Name == name);

        if (principal == null)
        {
            principal = new Principal { Name = name };

            var user = userSource?.TryGetUser(name);

            if (user == null)
            {
                await principalRepository.SaveAsync(principal, cancellationToken);
            }
            else
            {
                await principalRepository.InsertAsync(principal, user.Id, cancellationToken);
            }
        }

        return principal;
    }

    public async Task SaveAsync(Principal principal, CancellationToken cancellationToken = default)
    {
        await this.ValidateAsync(principal, cancellationToken);

        await principalRepository.SaveAsync(principal, cancellationToken);
    }

    public async Task RemoveAsync(Principal principal, bool force, CancellationToken cancellationToken)
    {
        if (force)
        {
            principal.Permissions.Foreach(p => p.DelegatedTo.Foreach(delP => delP.Principal.RemoveDetail(delP)));
        }
        else if (principal.Permissions.Any())
        {
            throw new BusinessLogicException($"Removing principal \"{principal.Name}\" must be empty");
        }


        await principalRepository.RemoveAsync(principal, cancellationToken);
    }

    public async Task ValidateAsync(Principal principal, CancellationToken cancellationToken)
    {
        await principalGeneralValidator.ValidateAndThrowAsync(principal, cancellationToken);
    }
}
