namespace Framework.Database.Domain;

[AttributeUsage(AttributeTargets.Property)]
public class NotAuditedPropertyAttribute : Attribute
{
}
