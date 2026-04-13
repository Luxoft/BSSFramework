namespace Framework.BLL.Domain.Persistent.Attributes;

/// <summary>
/// Раскрывает цепочку виртуальных свойств, превращает виртуальные вызовы свойств в реальные. Атрибут нужен для "уплощения" выбранного доменного объекта, - т. е. вытащить часть информации из одного или нескольких вложенных в друг друга сложных объектов, в корневой объект. Необходим для возможности построения OData запросов к виртуальным свойствам.
/// <see href="confluence/display/IADFRAME/ExpandPathAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class ExpandPathAttribute(string path) : PathAttribute(path);
