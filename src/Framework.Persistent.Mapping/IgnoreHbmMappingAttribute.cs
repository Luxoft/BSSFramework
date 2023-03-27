namespace Framework.Persistent.Mapping;

/// <summary>
/// Do not generate only HBM files!
/// <see cref="IgnoreMappingAttribute"/>
/// <see cref="NotPersistentClassAttribute"/>
/// <see cref="NotPersistentFieldAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreHbmMappingAttribute : Attribute
{
}
