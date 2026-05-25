using SampleSystem.Domain.BU;

namespace SampleSystem.Domain.Models.Filters.Projection;

public class TestEmployeeFilter : DomainObjectBase
{
    public bool TestValue { get; set; }

    public SampleStruct SampleStruct { get; set; }


    public BusinessUnit BusinessUnit
    {
        get;
        set;
    }
}
