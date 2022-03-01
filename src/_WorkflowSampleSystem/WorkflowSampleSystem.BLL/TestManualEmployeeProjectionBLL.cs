using System.Collections.Generic;

using Framework.DomainDriven;

using WorkflowSampleSystem.Domain.Models.Filters;
using WorkflowSampleSystem.Domain.Projections;

namespace WorkflowSampleSystem.BLL
{
    public partial class TestLegacyEmployeeBLL
    {
        public List<TestLegacyEmployee> GetListBy(EmployeeFilterModel filter, IFetchContainer<TestLegacyEmployee> fetchs) => throw new System.NotImplementedException();
    }
}
