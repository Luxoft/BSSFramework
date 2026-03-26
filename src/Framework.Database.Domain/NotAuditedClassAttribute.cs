namespace Framework.Database.Domain;

[AttributeUsage(AttributeTargets.Class)]
public class NotAuditedClassAttribute : Attribute
{
}
