using Framework.DomainDriven;
using Framework.Restriction;

namespace SampleSystem.Domain;

/// <summary>
/// Общий тип для базовой модели для изменения коллекции объектов
/// </summary>
/// <typeparam name="TDomainObject">Тип изменяемого объекта</typeparam>
[DirectMode(DirectMode.In | DirectMode.Out)]
public abstract class DomainObjectMassChangeModel<TDomainObject> : DomainObjectBase, IDomainObjectMassChangeModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    [Required]
    public List<TDomainObject> ChangingObjects { get; set; }

    IEnumerable<TDomainObject> IDomainObjectMassChangeModel<TDomainObject>.ChangingObjects => this.ChangingObjects;
}
