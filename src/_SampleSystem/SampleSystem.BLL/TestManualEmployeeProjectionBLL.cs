using GenericQueryable.Fetching;

using SampleSystem.Domain.Models.Filters;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL;

public partial class TestLegacyEmployeeBLL
{
    public List<TestLegacyEmployee> GetListBy(EmployeeFilterModel filter, FetchRule<TestLegacyEmployee> fetchs) => throw new NotImplementedException();
}
