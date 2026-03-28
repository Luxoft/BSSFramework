namespace Framework.OData.Tests.Unit.DomainModel;

public class Employee : Base
{
    private Department department;
    private NameEng nameEng;

    public Department Department
    {
        get { return this.department; }
        set { this.department = value; }
    }

    public int VirtualProperty => 123;

    public int NonVirtualProperty { get; set; }

    public NameEng NameEng
    {
        get { return this.nameEng; }
    }

    public Location Location
    {
        get { return this.Department.Location; }
    }
}
