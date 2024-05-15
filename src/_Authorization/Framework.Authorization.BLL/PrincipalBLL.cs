﻿using Framework.Authorization.Domain;
using Framework.Exceptions;

namespace Framework.Authorization.BLL;

public partial class PrincipalBLL
{
    public override void Save(Principal principal)
    {
        if (principal == null) throw new ArgumentNullException(nameof(principal));

        if (this.Context.CurrentPrincipal.RunAs != null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        base.Save(principal);
    }

    protected override void Validate(Principal domainObject, AuthorizationOperationContext operationContext)
    {
        this.Context.PrincipalValidator.Validate(domainObject);

        base.Validate(domainObject, operationContext);
    }
}
