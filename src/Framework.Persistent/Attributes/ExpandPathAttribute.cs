using System;

using Framework.Core;

namespace Framework.Persistent;

/// <summary>
/// Раскрывает цепочку виртуальных свойств, превращает виртуальные вызовы свойств в реальные. Атрибут нужен для "уплощения" выбранного доменного объекта, - т. е. вытащить часть информации из одного или нескольких вложенных в друг друга сложных объектов, в корневой объект. Необходим для возможности построения OData запросов к виртуальным свойствам.
/// <see href="confluence/display/IADFRAME/ExpandPathAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ExpandPathAttribute : PathAttribute
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="path">Определяет путь до нужного вложенного объекта или свойства.</param>
    public ExpandPathAttribute(PropertyPath path)
            : base(path)
    {
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="path">Определяет путь до нужного вложенного объекта или свойства.</param>
    public ExpandPathAttribute(string path)
            : base(path)
    {
    }
}
