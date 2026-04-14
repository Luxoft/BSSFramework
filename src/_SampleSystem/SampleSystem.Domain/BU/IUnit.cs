using Framework.Application.Domain;

namespace SampleSystem.Domain.BU;

public interface IUnit<out TUnit> : IVisualIdentityObject
        where TUnit : CommonUnitBase, IUnit<TUnit>
{
    TUnit CurrentObject { get; }
}
