namespace SampleSystem.Domain.Models.Filters;

public class GuidBasedFilterModel : DomainObjectBase
{
    public Guid? EmployeeIdent { get; set; }

    public string SearchFilter { get; set; }

    public List<Guid> EmployeeLocations { get; set; }

    public List<Guid> BusinessUnits { get; set; }
}
