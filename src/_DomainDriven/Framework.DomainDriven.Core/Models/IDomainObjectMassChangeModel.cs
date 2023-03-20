using System.Collections.Generic;

using Framework.Restriction;

namespace Framework.DomainDriven;

/// <summary>
/// Общий интерфейс базовой модели для изменения коллекции объектов
/// </summary>
/// <typeparam name="TDomainObject">Тип изменяемого объекта</typeparam>
[DirectMode(DirectMode.In | DirectMode.Out)]
public interface IDomainObjectMassChangeModel<out TDomainObject>
{
    [Required]
    IEnumerable<TDomainObject> ChangingObjects { get; }
}
