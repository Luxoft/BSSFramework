namespace Framework.Relations;

/// <summary>
/// Master object marker. Applied when there are 2 or more fields that can act as a Master reference.
/// <see href="confluence/display/IADFRAME/IsMasterAttribute"/>
/// </summary>
[AttributeUsage(AttributeTargets.Property)]
public class IsMasterAttribute : Attribute;
