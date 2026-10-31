namespace Framework.Database.Mapping;

/// <summary>
/// For a domain type marked with this attribute, writing only changed fields to the DB is enabled.
/// </summary>
/// <seealso cref="System.Attribute" />
[AttributeUsage(AttributeTargets.Class)]
public class SmartUpdateAttribute : Attribute;
