using Framework.Restriction;

namespace Framework.DomainDriven
{
    /// <summary>
    /// Общий интерфейс базовой модели для изменения единичного объекта
    /// </summary>
    /// <typeparam name="TDomainObject">Тип изменяемого объекта</typeparam>
    [DirectMode(DirectMode.In | DirectMode.Out)]
    public interface IDomainObjectChangeModel<out TDomainObject>
    {
        [Required]
        TDomainObject ChangingObject { get; }
    }
}
