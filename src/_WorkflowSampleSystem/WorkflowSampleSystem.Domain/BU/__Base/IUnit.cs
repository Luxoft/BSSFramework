using Framework.Persistent;

namespace WorkflowSampleSystem.Domain
{
    public interface IUnit<out TUnit> : IDefaultHierarchicalPersistentDomainObjectBase<TUnit>, IVisualIdentityObject
        where TUnit : CommonUnitBase, IUnit<TUnit>
    {
        TUnit CurrentObject { get; }
    }
}