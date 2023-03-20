using System;

namespace Framework.Persistent.Mapping;

/// <summary>
/// Для доменного типа, размеченного этим атрибутом, включается запись только измененных полей в БД.
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Class)]
public class SmartUpdateAttribute : Attribute
{
}
