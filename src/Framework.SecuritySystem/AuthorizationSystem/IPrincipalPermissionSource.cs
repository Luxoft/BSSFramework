using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.SecuritySystem;

public interface IPrincipalPermissionSource<TIdent>
{
    List<Dictionary<Type, List<TIdent>>> GetPermissions();

    IQueryable<IPermission<TIdent>> GetPermissionQuery<TSecurityOperationCode>(ContextSecurityOperation<TSecurityOperationCode> securityOperation)
            where TSecurityOperationCode : struct, Enum;

}
