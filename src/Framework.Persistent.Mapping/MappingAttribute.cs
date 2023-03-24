using System;

namespace Framework.Persistent.Mapping;

[AttributeUsage(AttributeTargets.Property)]
public class MappingAttribute : Attribute
{
    private CascadeMode? actualCascadeMode;

    /// <summary>
    /// Имя колонки в бд
    /// </summary>
    public string ColumnName
    {
        get;
        set;
    }

    /// <summary>
    ///     Получает или устанавливает флаг, указывающий, что сущности связываются один к одному.
    /// </summary>
    /// <value>
    ///     <c>true</c> если сущности связываются один к одному; в противном случае<c>false</c>.
    /// </value>
    /// <remarks>
    ///     Внимание!!! Свойство, отмеченное как Mapping(IsOneToOne = true), материализуется всегда.
    ///     Говоря проще, lazy loading в этом случае не работает.
    ///     Однако, если всё-таки очень нужно, можно применить подход:
    ///
    ///     <code>
    ///         public virtual TEvent Event
    ///         {
    ///             get { return this.GetOneToOne(v => v.InternalEvents) ?? this.CreateDefaultEvent(); }
    ///             set
    ///             {
    ///                 if (value == null) { return; }
    ///
    ///                 var newValue = value;
    ///
    ///                 this.SetValueSafe(v => v.Event, newValue, () => this.SetOneToOne(v => v.InternalEvents, newValue));
    ///             }
    ///         }
    ///    </code>
    ///
    /// </remarks>
    public bool IsOneToOne
    {
        get;
        set;
    }

    /// <summary>
    /// Поддержка каскадности удаления
    /// </summary>
    public CascadeMode CascadeMode
    {
        get { return this.actualCascadeMode ?? CascadeMode.Auto; }
        set { this.actualCascadeMode = value; }
    }

    public CascadeMode? GetActualCascadeMode()
    {
        return this.actualCascadeMode;
    }

    // TODO gtsaplin: never used, maybe mark as Obsolete?
    public string GroupKey
    {
        get;
        set;
    }

    public bool IsUnique { get; set; }

    /// <summary>
    /// Имя из сторонней таблицы которая join-тся по id и из которой берётся данная колонка
    /// </summary>
    public string ExternalTableName { get; set; }
}
