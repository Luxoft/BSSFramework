using SampleSystem.Domain.BU;

namespace SampleSystem.Domain.Models.Create._Base;

public class TestUnpersistentObject : DomainObjectBase
{
    public BusinessUnit BusinessUnit { get; set; }

    public string Value1 { get; set; }

    public int Value2 { get; set; }
}
