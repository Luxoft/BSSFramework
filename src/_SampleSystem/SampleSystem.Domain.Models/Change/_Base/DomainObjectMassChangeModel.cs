using System.Collections.Generic;

using Framework.DomainDriven;

namespace SampleSystem.Domain;

/// <summary>
/// Общий тип для базовой модели для изменения коллекции объектов
/// </summary>
/// <typeparam name="TDomainObject">Тип изменяемого объекта</typeparam>
[DirectMode(DirectMode.In | DirectMode.Out)]
public abstract class DomainObjectMassChangeModel<TDomainObject> : DomainObjectBase, IDomainObjectMassChangeModel<TDomainObject>
        where TDomainObject : PersistentDomainObjectBase
{
    [Framework.Restriction.Required]
    public List<TDomainObject> ChangingObjects { get; set; }

    IEnumerable<TDomainObject> IDomainObjectMassChangeModel<TDomainObject>.ChangingObjects => this.ChangingObjects;
}
