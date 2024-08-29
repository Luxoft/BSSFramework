using FluentValidation;
using Framework.Authorization.Domain;
using Framework.Authorization.SecuritySystem;
using Framework.Exceptions;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.BLL;

public partial class PrincipalBLL
{
    public override void Save(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        if (this.Context.CurrentPrincipalSource.CurrentPrincipal.RunAs != null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        base.Save(principal);
    }

    protected override void Validate(Principal domainObject, AuthorizationOperationContext operationContext)
    {
        this.Context.PrincipalValidator.ValidateAndThrow(domainObject);

        base.Validate(domainObject, operationContext);
    }

    public override void Remove(Principal domainObject)
    {
        this.Context.ServiceProvider.GetRequiredService<IPrincipalDomainService>().RemoveAsync(domainObject).GetAwaiter().GetResult();

        base.Remove(domainObject);
    }
}
