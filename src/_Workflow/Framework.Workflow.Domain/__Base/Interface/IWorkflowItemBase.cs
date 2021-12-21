using Framework.Persistent;

namespace Framework.Workflow.Domain
{
    /// <summary>
    /// Интерфейс для воркфлоу объектов
    /// </summary>
    public interface IWorkflowItemBase : IDefaultAuditPersistentDomainObjectBase, IVisualIdentityObject, IDescriptionObject
    {

    }
}