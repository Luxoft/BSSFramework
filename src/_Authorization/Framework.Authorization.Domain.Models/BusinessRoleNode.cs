using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace Framework.Authorization.Domain;

public class BusinessRoleNode : DomainObjectBase
{
    public BusinessRoleNode(BusinessRole businessRole, BusinessRole parentRole)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        this.Id = businessRole.Id;
        this.ParentId = parentRole.TryGetId();

        this.Active = businessRole.Active;
        this.CreatedBy = businessRole.CreatedBy;
        this.CreateDate = businessRole.CreateDate;
        this.ModifyDate = businessRole.ModifyDate;
        this.ModifiedBy = businessRole.ModifiedBy;
        this.Name = businessRole.Name;
        this.Description = businessRole.Description;

        this.Children = businessRole.SubBusinessRoles;
    }

    public Guid Id { get; set; }

    public Guid ParentId { get; set; }

    public bool Active { get; private set; }

    public string CreatedBy { get; private set; }

    public DateTime? CreateDate { get; private set; }

    public DateTime? ModifyDate { get; private set; }

    public string ModifiedBy { get; private set; }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public IEnumerable<BusinessRole> Children { get; private set; }

    public string Name { get; set; }

    public string Description { get; set; }
}
