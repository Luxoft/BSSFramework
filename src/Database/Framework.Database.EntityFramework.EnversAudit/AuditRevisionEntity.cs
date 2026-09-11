namespace Framework.Database.EntityFramework.EnversAudit;

public class AuditRevisionEntity
{
    public long Id { get; set; }

    public string Author { get; set; } = null!;

    public DateTime RevisionDate { get; set; }
}
