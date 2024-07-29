using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;
using Framework.Persistent.Mapping;
using Framework.Restriction;

namespace SampleSystem.Domain;

[BLLViewRole]
public class EmployeePhoto : AuditPersistentDomainObjectBase, IDetail<Employee>, ITypeObject<EmployeePhotoType>
{
    public const int DEFAULT_PHOTO_MAX_SIZE = 1024 * 1024;

    private readonly Employee employee;

    private string contentType;
    private byte[] data = new byte[0];

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

    public virtual Employee Employee
    {
        get { return this.employee; }
    }

    [Framework.Restriction.Required]
    public virtual string ContentType
    {
        get { return this.contentType.TrimNull(); }
        protected internal set { this.contentType = value.TrimNull(); }
    }

    public virtual bool IsDefault
    {
        get { return this.Type == EmployeePhotoType.Default; }
    }

    [UniqueElement]
    public virtual EmployeePhotoType Type
    {
        get { return this.type; }
        protected internal set { this.type = value; }
    }

    [Mapping(ColumnName = "Photo")]
    [MaxLength(DEFAULT_PHOTO_MAX_SIZE)]
    [Framework.Restriction.Required]
    public virtual byte[] Data
    {
        get { return this.data; }
        set { this.data = value; }
    }

    Employee IDetail<Employee>.Master
    {
        get { return this.Employee; }
    }
}
