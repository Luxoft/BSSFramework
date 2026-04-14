using Framework.Application.Domain;

namespace SampleSystem.Domain.BU.__Base;

public interface IUnit<out TUnit> : IVisualIdentityObject
        where TUnit : CommonUnitBase, IUnit<TUnit>
{
    TUnit CurrentObject { get; }
}
