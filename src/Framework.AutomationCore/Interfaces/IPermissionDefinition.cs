using System;
using System.Collections.Generic;

namespace Automation.Utils
{
    public interface IPermissionDefinition
    {
        IEnumerable<Tuple<string, Guid>> GetEntities();

        string GetRoleName();
    }
}
