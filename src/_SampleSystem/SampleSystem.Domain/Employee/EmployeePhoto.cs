using Framework.BLL.Domain.ServiceRole;
using Framework.Core;
using Framework.Database.Mapping;
using Framework.Relations;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
public class EmployeePhoto : AuditPersistentDomainObjectBase, IDetail<Employee>
{
    public const int DEFAULT_PHOTO_MAX_SIZE = 1024 * 1024;

    private readonly Employee employee;

    private string contentType;
    private byte[] data = [];

    private EmployeePhotoType type;

    public EmployeePhoto(Employee employee)
    {
        if (employee == null)
        {
            throw new ArgumentNullException(nameof(employee));
        }

        this.employee = employee;
        this.employee.AddDetail(this);
    }

    protected EmployeePhoto()
    {
    }

    public virtual Employee Employee => this.employee;

    [Required]
    public virtual string ContentType
    {
        get => this.contentType.TrimNull();
        protected internal set => this.contentType = value.TrimNull();
    }

    public virtual bool IsDefault => this.Type == EmployeePhotoType.Default;

    [UniqueElement]
    public virtual EmployeePhotoType Type
    {
        get => this.type;
        protected internal set => this.type = value;
    }

    [Mapping(ColumnName = "Photo")]
    [MaxLength(DEFAULT_PHOTO_MAX_SIZE)]
    [Required]
    public virtual byte[] Data
    {
        get => this.data;
        set => this.data = value;
    }

    Employee IDetail<Employee>.Master => this.Employee;
}
