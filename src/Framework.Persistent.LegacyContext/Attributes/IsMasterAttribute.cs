namespace Framework.Persistent;

/// <summary>
/// Маркер Master-объекта. Применяется, когда есть 2 или больше поля, которые могут выступать Master-ссылкой.
/// <see href="confluence/display/IADFRAME/IsMasterAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class IsMasterAttribute : Attribute
{
}
