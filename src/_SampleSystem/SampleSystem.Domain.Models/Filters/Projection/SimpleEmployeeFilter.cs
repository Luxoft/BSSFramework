using System;
using System.Linq.Expressions;

namespace SampleSystem.Domain;

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
