namespace Framework.Database.EntityFramework.EnversAudit;

public sealed record AuditRevisionInfo(long RevisionNumber, string Author, DateTime Date, AuditRevisionType RevisionType);

public sealed record AuditPropertyRevisionInfo<TProperty>(TProperty Value, AuditRevisionInfo Revision);
