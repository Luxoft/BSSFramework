using Anch.Core;
using Anch.SecuritySystem.Services;

using Framework.Application;
using Framework.Authorization.Domain;
using Framework.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Framework.Authorization.BLL;

public partial class PrincipalBLL
{
    public override void Save(Principal principal)
    {
        if (principal is null) throw new ArgumentNullException(nameof(principal));

        if (this.Context.CurrentPrincipalSource.CurrentUser.RunAs is not null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        base.Save(principal);
    }

    protected override void Validate(Principal domainObject, OperationContextBase operationContext)
    {
        this.DefaultCancellationTokenSource.RunSync(async ct => await this.Context.PrincipalValidator.ValidateAsync(domainObject.ToPrincipalData(), ct));

        base.Validate(domainObject, operationContext);
    }

    public override void Remove(Principal domainObject)
    {
        this.DefaultCancellationTokenSource.RunSync(ct => this.Context.ServiceProvider.GetRequiredService<IPrincipalDomainService<Principal>>()
                                                              .RemoveAsync(domainObject, false, ct));

        base.Remove(domainObject);
    }
}
