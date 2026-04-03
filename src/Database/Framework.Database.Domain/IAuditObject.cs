namespace Framework.Database;

public interface IAuditObject
{
    DateTime? CreateDate { get; }

    string? CreatedBy { get; }


    DateTime? ModifyDate { get; }

    string? ModifiedBy { get; }
}
