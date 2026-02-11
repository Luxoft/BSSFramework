using Framework.Authorization.BLL._Validation;
using Framework.Authorization.Domain;
using Framework.Exceptions;

using Microsoft.Extensions.DependencyInjection;

using SecuritySystem.Services;

namespace Framework.Authorization.BLL;

public partial class PrincipalBLL
{
    public override void Save(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        if (this.Context.CurrentPrincipalSource.CurrentUser.RunAs != null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        base.Save(principal);
    }

    protected override void Validate(Principal domainObject, AuthorizationOperationContext operationContext)
    {
        this.Context.PrincipalValidator.ValidateAsync(domainObject.ToPrincipalData(), CancellationToken.None).GetAwaiter().GetResult();

        base.Validate(domainObject, operationContext);
    }

    public override void Remove(Principal domainObject)
    {
        this.Context.ServiceProvider.GetRequiredService<IPrincipalDomainService<Principal>>().RemoveAsync(domainObject).GetAwaiter().GetResult();

        base.Remove(domainObject);
    }
}
