using System.Dynamic;

namespace Framework.DomainDriven.SerializeMetadata.Tests.Unit;

public class TestDomainObjProperty3
{
    public int Age { get; set; }
    public string Name { get; set; }

    public TestDomainObjProperty4 Property4 { get; set; }
}

public class TestDomainObjProperty4
{
    public string Age
    {
        get;
        set;
    }
    public string Name
    {
        get;
        set;
    }

}

public class TestDomainObjProperty2
{
    public TestDomainObjProperty4 Property4 { get; set; }

    public TestDomainObjProperty3 Property3 { get; set; }

    public string Name
    {
        get;
        set;
    }

    public int Age
    {
        get;
        set;
    }
}
