using System;

using Framework.Persistent;

namespace Framework.Security;

/// <summary>
/// Атрибут указывающий, что безопасность используется из другого типа
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class DependencySecurityAttribute : Attribute, IPathAttribute
{
    /// <summary>
    /// Конструктор атрибута
    /// </summary>
    /// <param name="sourceType">Тип исходного объекта, в котором определена безопасность</param>
    /// <param name="path">Путь до исходного объекта, в котором определена безопасность, если пусть оставить пустым, то сравнение будет нетипизированно производиться по Id-шникам (актуально для проекций и вьюх)</param>
    public DependencySecurityAttribute(Type sourceType, string path = "")
    {
        this.SourceType = sourceType;
        this.Path = path;
    }


    /// <summary>
    /// Тип исходного объекта, в котором определена безопасность
    /// </summary>
    public Type SourceType { get; }

    /// <summary>
    /// Путь до исходного объекта, в котором определена безопасность
    /// </summary>
    public string Path { get; }

    /// <summary>
    /// Является ли наследуемое секурити нетизированным (для проекций и вьюх)
    /// </summary>
    public bool IsUntyped => this.Path == string.Empty;
}
