namespace Framework.Database.EntityFramework.Audit;

public sealed record AuditPropertyMetadata(string Name, Type PropertyType, bool IsKey);