using System;

using Framework.Projection;
using Framework.Projection.Lambda;

using WorkflowSampleSystem.Domain;
using WorkflowSampleSystem.Domain.Models.Filters;

namespace WorkflowSampleSystem.CodeGenerate
{
    public class LegacyWorkflowSampleSystemProjectionSource : ProjectionSource
    {
        public LegacyWorkflowSampleSystemProjectionSource()
        {
            this.TestLegacyEmployee = new Projection<Employee>(() => this.TestLegacyEmployee, true)
                .Property(employee => employee.Login)
                .Property(employee => employee.Role.Name)
                .Property(employee => employee.Role.Id)

                .Filter<EmployeeFilterModel>(ProjectionFilterTargets.Collection);
        }

        public Projection<Employee> TestLegacyEmployee { get; }
    }
}
