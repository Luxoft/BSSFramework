using System.Collections.Generic;

namespace Framework.SecuritySystem
{
    public interface IPermission<out TIdent>
    {
        IEnumerable<IPermissionFilterItem<TIdent>> FilterItems { get; }

        //IEnumerable<IDenormalizedPermissionItem<TIdent>> DenormalizedItems { get; }
    }
}
