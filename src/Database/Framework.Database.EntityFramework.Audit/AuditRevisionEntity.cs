namespace Framework.Database.EntityFramework.Audit;

public class AuditRevisionEntity
{
    public long Id { get; set; }

    public string Author { get; set; } = null!;

    public DateTime RevisionDate { get; set; }
}
