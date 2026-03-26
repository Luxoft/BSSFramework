namespace Framework.Database.Domain;

/// <summary>
/// Do not generate HBM files and DB Table
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class IgnoreMappingAttribute : Attribute
{
}
