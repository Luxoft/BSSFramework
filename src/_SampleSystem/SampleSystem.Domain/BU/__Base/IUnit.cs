using Framework.Persistent;

namespace SampleSystem.Domain;

public interface IUnit<out TUnit> : IDefaultHierarchicalPersistentDomainObjectBase<TUnit>, IVisualIdentityObject
        where TUnit : CommonUnitBase, IUnit<TUnit>
{
    TUnit CurrentObject { get; }
}
