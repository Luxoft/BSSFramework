using Framework.DomainDriven;
using Framework.Restriction;

namespace SampleSystem.Domain;

/// <summary>
/// Базовая произвольная модель для изменений объектов (пример, для которого будет расширена генерация)
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
[DirectMode(DirectMode.In)]
public abstract class DomainObjectComplexChangeModel<TDomainObject> : DomainObjectBase
        where TDomainObject : PersistentDomainObjectBase
{
    [Required]
    public TDomainObject PrimaryChangingObject { get; set; }

    [Required]
    public List<TDomainObject> SecondaryChangingObjects { get; set; }
}
