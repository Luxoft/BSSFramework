namespace SampleSystem.Domain.Projections;

public partial class BusinessUnitProgramClass
{
    public override string VirtualValue => this.Id.ToString();

    public override string VirtualName => this.Name;
}
