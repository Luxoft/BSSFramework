using System;
using System.Collections.Generic;

namespace Framework.Authorization.Domain;

public interface IOptimizeRuntimePermissionService
{
    IEnumerable<Dictionary<Type, List<Guid>>> Optimize(IEnumerable<Dictionary<Type, List<Guid>>> permissions);
}
