using Framework.Projection;
using Framework.Projection.Lambda;
using Framework.Projection.Lambda.ProjectionSource;
using SampleSystem.Domain.Employee;
using SampleSystem.Domain.Models.Filters;

namespace SampleSystem.CodeGenerate.Configurations._ProjectionSources;

public class LegacySampleSystemProjectionSource : ProjectionSource
{
    public LegacySampleSystemProjectionSource() =>
        this.TestLegacyEmployee = new Projection<Employee>(() => this.TestLegacyEmployee, true)
                                  .Property(employee => employee.Login)
                                  .Property(employee => employee.Role.Name)
                                  .Property(employee => employee.Role.Id)

                                  .Filter<EmployeeFilterModel>(ProjectionFilterTargets.Collection);

    public Projection<Employee> TestLegacyEmployee { get; }
}
