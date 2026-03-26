using System.Linq.Expressions;

using Framework.Core;

namespace Framework.Projection.Lambda;

/// <summary>
/// Метаданные проекции
/// </summary>
/// <typeparam name="TDomainObject"></typeparam>
public class Projection<TDomainObject> : IProjection
{
    private readonly IReadOnlyList<IProjectionCustomProperty> customProperties = new List<IProjectionCustomProperty>();

    private readonly IReadOnlyList<Attribute> projectionAttributes = new List<Attribute>();

    private readonly IReadOnlyList<ProjectionFilterAttribute> filterAttributes = new List<ProjectionFilterAttribute>();

    private readonly IReadOnlyList<IProjectionProperty> properties = new List<IProjectionProperty>();

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="name">Имя проекции</param>
    /// <param name="bllView">Генерация BLL и фасадных методов</param>
    public Projection(string name, bool bllView = false)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

        this.Name = name;
        this.BLLView = bllView;
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="getName">Получение имени проекции</param>
    /// <param name="bllView">Генерация BLL и фасадных методов</param>
    public Projection(Expression<Func<Projection<TDomainObject>>> getName, bool bllView = false)
            : this(getName.GetInstanceMemberName(), bllView)
    {
    }

    internal Projection(string name, bool bllView, IEnumerable<IProjectionProperty> properties, IEnumerable<ProjectionFilterAttribute> filterAttributes, IEnumerable<IProjectionCustomProperty> customProperties, IEnumerable<Attribute> attributes)
            : this(name, bllView)
    {
        this.properties = (properties ?? throw new ArgumentNullException(nameof(properties))).ToList();
        this.filterAttributes = (filterAttributes ?? throw new ArgumentNullException(nameof(filterAttributes))).ToList();
        this.customProperties = (customProperties ?? throw new ArgumentNullException(nameof(customProperties))).ToList();
        this.projectionAttributes = (attributes ?? throw new ArgumentNullException(nameof(attributes))).ToList();
    }

    /// <inheritdoc />
    public Type SourceType { get; } = typeof(TDomainObject);

    /// <inheritdoc />
    public string Name { get; }

    /// <inheritdoc />
    public bool BLLView { get; }

    /// <inheritdoc />
    public ProjectionRole Role { get; } = ProjectionRole.Default;

    /// <summary>
    /// Добавление в проекцию свойства
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="path">Путь до свойства</param>
    /// <param name="name">Имя свойства</param>
    /// <param name="ignoreSerialization">Отключение сериализации в DTO</param>
    /// <param name="propertyAttributes">Дополнительные атрибуты свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> Property<TProperty>(Expression<Func<TDomainObject, TProperty>> path, string name = null, bool ignoreSerialization = false, IEnumerable<Attribute> propertyAttributes = null)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));

        return this.AddProperty(new ProjectionSingleProperty<TDomainObject, TProperty>(path, name, null, ignoreSerialization, propertyAttributes ?? new Attribute[0]));
    }

    /// <summary>
    /// Добавление в проекцию свойства
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="path">Путь до свойства</param>
    /// <param name="getPropProjection">Проекция на тип свойства</param>
    /// <param name="name">Имя свойства</param>
    /// <param name="ignoreSerialization">Отключение сериализации в DTO</param>
    /// <param name="propertyAttributes">Дополнительные атрибуты свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> Property<TProperty>(Expression<Func<TDomainObject, TProperty>> path, Func<Projection<TProperty>> getPropProjection, string name = null, bool ignoreSerialization = false, IEnumerable<Attribute> propertyAttributes = null)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));
        if (getPropProjection == null) throw new ArgumentNullException(nameof(getPropProjection));

        return this.AddProperty(new ProjectionSingleProperty<TDomainObject, TProperty>(path, name, getPropProjection, ignoreSerialization, propertyAttributes ?? new Attribute[0]));
    }

    /// <summary>
    /// Добавление в проекцию коллекционного свойства
    /// </summary>
    /// <typeparam name="TProperty"></typeparam>
    /// <param name="path">Путь до свойства</param>
    /// <param name="getPropProjection">Проекция на тип свойства</param>
    /// <param name="name">Имя свойства</param>
    /// <param name="ignoreSerialization">Отключение сериализации в DTO</param>
    /// <param name="propertyAttributes">Дополнительные атрибуты свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> Property<TProperty>(Expression<Func<TDomainObject, IEnumerable<TProperty>>> path, Func<Projection<TProperty>> getPropProjection, string name = null, bool ignoreSerialization = false, IEnumerable<Attribute> propertyAttributes = null)
    {
        if (path == null) throw new ArgumentNullException(nameof(path));
        if (getPropProjection == null) throw new ArgumentNullException(nameof(getPropProjection));

        return this.AddProperty(new ProjectionManyProperty<TDomainObject, TProperty>(path, name, getPropProjection, ignoreSerialization, propertyAttributes ?? new Attribute[0]));
    }

    internal Projection<TDomainObject> AddProperty(IProjectionProperty newProp)
    {
        if (newProp == null) throw new ArgumentNullException(nameof(newProp));

        return new Projection<TDomainObject>(this.Name, this.BLLView, this.properties.Concat(new[] { newProp }), this.filterAttributes, this.customProperties, this.projectionAttributes);
    }

    /// <summary>
    /// Добавление в проекцию расчётного свойства
    /// </summary>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <param name="name">Имя свойства</param>
    /// <param name="getPropProjection">Проекция на тип свойства</param>
    /// <param name="fetchs">Прокачиваемые свойства</param>
    /// <param name="propertyAttributes">Дополнительные атрибуты свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomProperty<TProperty>(string name, Func<Projection<TProperty>> getPropProjection = null, string[] fetchs = null, IEnumerable<Attribute> propertyAttributes = null)
    {
        if (name == null) { throw new ArgumentNullException(nameof(name)); }

        return this.CustomProperty(name, false, getPropProjection, fetchs, propertyAttributes);
    }

    /// <summary>
    /// Добавление в проекцию расчётного свойства
    /// </summary>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <param name="name">Имя свойства</param>
    /// <param name="writable">Доступна запись в свойство</param>
    /// <param name="getPropProjection">Проекция на тип свойства</param>
    /// <param name="fetchs">Прокачиваемые свойства</param>
    /// <param name="propertyAttributes">Дополнительные атрибуты свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomProperty<TProperty>(string name, bool writable, Func<Projection<TProperty>> getPropProjection = null, string[] fetchs = null, IEnumerable<Attribute> propertyAttributes = null)
    {
        if (name == null) { throw new ArgumentNullException(nameof(name)); }

        return new Projection<TDomainObject>(
                                             this.Name,
                                             this.BLLView,
                                             this.properties,
                                             this.filterAttributes,
                                             this.customProperties.Concat(new[] { new ProjectionCustomProperty<TProperty>(name, writable, getPropProjection, null, fetchs, propertyAttributes) }),
                                             this.projectionAttributes);
    }


    /// <summary>
    /// Добавление в проекцию расчётного свойства
    /// </summary>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <param name="path">Путь к свойству</param>
    /// <param name="getPropProjection">Проекция на тип свойства</param>
    /// <param name="fetchs">Прокачиваемые свойства</param>
    /// <param name="propertyAttributes">Дополнительные атрибуты свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomProperty<TProperty>(Expression<Func<TDomainObject, TProperty>> path, Func<Projection<TProperty>> getPropProjection = null, string[] fetchs = null, IEnumerable<Attribute> propertyAttributes = null)
    {
        if (path == null) { throw new ArgumentNullException(nameof(path)); }

        return this.CustomProperty(
                                   path.ToPath().Replace(".", string.Empty),
                                   getPropProjection,
                                   fetchs,
                                   propertyAttributes);
    }

    /// <summary>
    /// Добавление в проекцию расчётного свойства
    /// </summary>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <param name="path">Путь к свойству</param>
    /// <param name="writable">Доступна запись в свойство</param>
    /// <param name="getPropProjection">Проекция на тип свойства</param>
    /// <param name="fetchs">Прокачиваемые свойства</param>
    /// <param name="propertyAttributes">Дополнительные атрибуты свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomProperty<TProperty>(Expression<Func<TDomainObject, TProperty>> path, bool writable, Func<Projection<TProperty>> getPropProjection = null, string[] fetchs = null, IEnumerable<Attribute> propertyAttributes = null)
    {
        if (path == null) { throw new ArgumentNullException(nameof(path)); }

        return this.CustomProperty(
                                   path.ToPath().Replace(".", string.Empty),
                                   writable,
                                   getPropProjection,
                                   fetchs,
                                   propertyAttributes);
    }

    /// <summary>
    /// Добавление в проекцию коллекционного расчётного свойства
    /// </summary>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <param name="name">Имя свойства</param>
    /// <param name="writable">Доступна запись в свойство</param>
    /// <param name="getPropProjection">Проекция на тип свойства</param>
    /// <param name="collectionType">Тип коллекции у проекции</param>
    /// <param name="fetchs">Прокачиваемые свойства</param>
    /// <param name="propertyAttributes">Дополнительные атрибуты свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomManyProperty<TProperty>(string name, bool writable, Func<Projection<TProperty>> getPropProjection, Type collectionType, string[] fetchs = null, IEnumerable<Attribute> propertyAttributes = null)
    {
        if (name == null) { throw new ArgumentNullException(nameof(name)); }

        return new Projection<TDomainObject>(
                                             this.Name,
                                             this.BLLView,
                                             this.properties,
                                             this.filterAttributes,
                                             this.customProperties.Concat(new[] { new ProjectionCustomProperty<TProperty>(name, writable, getPropProjection, collectionType, fetchs, propertyAttributes) }),
                                             this.projectionAttributes);
    }

    /// <summary>
    /// Добавление в проекцию коллекционного расчётного свойства
    /// </summary>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <param name="name">Имя свойства</param>
    /// <param name="writable">Доступна запись в свойство</param>
    /// <param name="getPropProjection">Проекция на тип свойства</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomManyProperty<TProperty>(string name, bool writable, Func<Projection<TProperty>> getPropProjection)
    {
        if (name == null) { throw new ArgumentNullException(nameof(name)); }

        return this.CustomManyProperty<TProperty>(name, writable, getPropProjection, typeof(IEnumerable<>));
    }

    /// <summary>
    /// Добавление в проекцию коллекционного расчётного свойства
    /// </summary>
    /// <typeparam name="TProperty">Тип свойства</typeparam>
    /// <param name="name">Имя свойства</param>
    /// <param name="writable">Доступна запись в свойство</param>
    /// <returns></returns>
    public Projection<TDomainObject> CustomManyProperty<TProperty>(string name, bool writable)
    {
        if (name == null) { throw new ArgumentNullException(nameof(name)); }

        return this.CustomManyProperty<TProperty>(name, writable, null);
    }

    /// <summary>
    /// Копия проекции с новым именем
    /// </summary>
    /// <param name="name">Имя проекции</param>
    /// <param name="bllView">Генерация BLL и фасадных методов</param>
    /// <returns></returns>
    public Projection<TDomainObject> OverrideHeader(string name, bool? bllView = null)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        return new Projection<TDomainObject>(
                                             name,
                                             bllView ?? this.BLLView,
                                             this.properties,
                                             this.filterAttributes,
                                             this.customProperties,
                                             this.projectionAttributes);
    }

    /// <summary>
    /// Копия проекции с новым именем
    /// </summary>
    /// <param name="getName">Получение имени проекции</param>
    /// <param name="bllView">Генерация BLL и фасадных методов</param>
    /// <returns></returns>
    public Projection<TDomainObject> OverrideHeader(Expression<Func<Projection<TDomainObject>>> getName, bool? bllView = null)
    {
        return this.OverrideHeader(getName.GetInstanceMemberName(), bllView);
    }


    /// <summary>
    /// Добавление всех свойств из другой проекции, кроме заголовков
    /// </summary>
    /// <param name="propertiesSource"></param>
    /// <returns></returns>
    public Projection<TDomainObject> AddSource(Projection<TDomainObject> propertiesSource)
    {
        return new Projection<TDomainObject>(
                                             this.Name,
                                             this.BLLView,
                                             this.properties.Concat(propertiesSource.properties),
                                             this.filterAttributes.Concat(propertiesSource.filterAttributes),
                                             this.customProperties.Concat(propertiesSource.customProperties),
                                             this.projectionAttributes.Concat(propertiesSource.projectionAttributes));
    }

    /// <summary>
    /// Добавление фильтра, по которому выгружаюстя проекционные объекты
    /// </summary>
    /// <param name="filterType">Тип фильтра</param>
    /// <param name="target">Применимость фильтра</param>
    /// <returns></returns>
    public Projection<TDomainObject> Filter(Type filterType, ProjectionFilterTargets target = ProjectionFilterTargets.OData)
    {
        if (filterType == null) throw new ArgumentNullException(nameof(filterType));

        return new Projection<TDomainObject>(this.Name, this.BLLView, this.properties, this.filterAttributes.Concat(new[] { new ProjectionFilterAttribute(filterType, target) }), this.customProperties, this.projectionAttributes);
    }

    /// <summary>
    /// Добавление фильтра, по которому выгружаюстя проекционные объекты
    /// </summary>
    /// <typeparam name="TFilter">Тип фильтра</typeparam>
    /// <param name="target">Применимость фильтра</param>
    /// <returns></returns>
    public Projection<TDomainObject> Filter<TFilter>(ProjectionFilterTargets target = ProjectionFilterTargets.OData)
    {
        return this.Filter(typeof(TFilter), target);
    }

    /// <summary>
    /// Добавление пользовательского атрибута
    /// </summary>
    /// <param name="attribute">Пользовательский атрибут</param>
    /// <returns></returns>
    public Projection<TDomainObject> Attribute(Attribute attribute)
    {
        if (attribute == null) { throw new ArgumentNullException(nameof(attribute)); }

        return new Projection<TDomainObject>(this.Name, this.BLLView, this.properties, this.filterAttributes, this.customProperties, this.projectionAttributes.Concat(new[] { attribute }));
    }


    IReadOnlyList<IProjectionProperty> IProjection.Properties => this.properties;

    IReadOnlyList<IProjectionCustomProperty> IProjection.CustomProperties => this.customProperties;

    IReadOnlyList<Attribute> IProjectionAttributeProvider.Attributes => this.projectionAttributes;

    IReadOnlyList<ProjectionFilterAttribute> IProjection.FilterAttributes => this.filterAttributes;

    bool IProjection.IgnoreIdSerialization { get; } = false;
}
