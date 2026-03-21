namespace Framework.BLL.Domain.Persistent.IdentityObject;

public interface IAuditObject
{
    DateTime? CreateDate { get; }

    string? CreatedBy { get; }


    DateTime? ModifyDate { get; }

    string? ModifiedBy { get; }
}
