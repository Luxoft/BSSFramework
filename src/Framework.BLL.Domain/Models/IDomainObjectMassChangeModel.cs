using System.ComponentModel.DataAnnotations;

using Framework.BLL.Domain.DirectMode;

namespace Framework.BLL.Domain.Models;

/// <summary>
/// Общий интерфейс базовой модели для изменения коллекции объектов
/// </summary>
/// <typeparam name="TDomainObject">Тип изменяемого объекта</typeparam>
[DirectMode(DirectMode.DirectMode.In | DirectMode.DirectMode.Out)]
public interface IDomainObjectMassChangeModel<out TDomainObject>
{
    [Required]
    IEnumerable<TDomainObject> ChangingObjects { get; }
}
