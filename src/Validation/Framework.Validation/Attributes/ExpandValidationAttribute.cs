namespace Framework.Validation.Attributes;

/// <summary>
/// Attribute indicating that validation will expand class properties
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class ExpandValidationAttribute : Attribute;
