using Framework.DomainDriven.BLL.Security.Lock;
using Framework.Workflow.Domain.Definition;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// Константа для идентификации объекта, на котором можно сделать пессимистическую блокировку
    /// </summary>
    public enum NamedLockOperation
    {
        [GlobalLock(typeof(TargetSystem))]
        UpdateTargetSystem,
    }
}