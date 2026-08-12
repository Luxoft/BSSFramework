using Framework.BLL.Domain.DirectMode;
using Framework.Restriction;

namespace Framework.BLL.Domain.Models;

/// <summary>
/// Общий интерфейс базовой модели для изменения единичного объекта
/// </summary>
/// <typeparam name="TDomainObject">Тип изменяемого объекта</typeparam>
[DirectMode(DirectMode.DirectMode.In | DirectMode.DirectMode.Out)]
public interface IDomainObjectChangeModel<out TDomainObject>
{
    [Required]
    TDomainObject ChangingObject { get; }
}
