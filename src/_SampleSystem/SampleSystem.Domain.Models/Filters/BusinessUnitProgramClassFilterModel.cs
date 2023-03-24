using System;

namespace SampleSystem.Domain.Models.Filters;

public class BusinessUnitProgramClassFilterModel : DomainObjectBase
{
    public Guid? AncestorIdent { get; set; }

    public string FilterVirtualName { get; set; }
}
