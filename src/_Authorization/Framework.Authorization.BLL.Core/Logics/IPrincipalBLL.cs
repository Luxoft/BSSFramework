using System;
using System.Collections.Generic;

using Framework.Authorization.Domain;

namespace Framework.Authorization.BLL
{
    public partial interface IPrincipalBLL
    {
        void Remove(Principal principal, bool force);

        Principal GetByNameOrCreate(string name, bool autoSave = false);

        Principal GetCurrent(bool autoSave = false);

        IEnumerable<Principal> GetAvailablePrincipals<TSecurityOperationCode>(params TSecurityOperationCode[] securityOperationCodes)
            where TSecurityOperationCode : struct, Enum;
    }
}
