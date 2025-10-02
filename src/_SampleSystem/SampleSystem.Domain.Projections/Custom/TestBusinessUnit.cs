using CommonFramework;

namespace SampleSystem.Domain.Projections;

public partial class TestBusinessUnit
{
    public override string CalcProp
    {
        get => this.Name + this.Name;
        set => throw new NotImplementedException();
    }

    public override string[][] CalcMatrix => new string[][]
                                             {
                                                     new string[] { "A", "B" },
                                                     new string[] { "B", "C" }
                                             };

    public override TestBusinessUnitType CalcProjectionProp => null;

    public override string HerBusinessUnit_Full => this.Her.GetAllElements(h => h.Parent).Join(",", bu => bu.Name);

    public override string Employees => this.BusinessUnitEmployeeRoles.OrderBy(link => link.Employee.NameEngFirstName).Join(",", link => link.Employee.NameEngFirstName);
}
