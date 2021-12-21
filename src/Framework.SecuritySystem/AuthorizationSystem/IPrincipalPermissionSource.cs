using System;
using System.Collections.Generic;

namespace Framework.SecuritySystem
{
    public interface IPrincipalPermissionSource<TIdent>
    {
        public List<Dictionary<Type, List<TIdent>>> GetPermissions();
    }
}
