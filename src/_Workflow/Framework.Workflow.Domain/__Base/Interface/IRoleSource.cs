using System.Collections.Generic;

using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{

    /// <summary>
    /// Интерфейс для ролей воркфлоу
    /// </summary>
    public interface IRoleSource
    {
        IEnumerable<Role> GetUsingRoles();
    }
}