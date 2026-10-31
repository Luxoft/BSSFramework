namespace Framework.Database.Mapping;

/// <summary>
/// Indicates that the mapping for this object will be generated separately from the base class, but with its fields (requires the table name to match).
/// As an independent object, i.e. the hierarchy is not observed.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class InlineBaseTypeMappingAttribute : Attribute;
