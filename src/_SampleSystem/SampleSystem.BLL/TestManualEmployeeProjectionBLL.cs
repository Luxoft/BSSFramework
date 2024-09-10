using Framework.DomainDriven;

using SampleSystem.Domain.Models.Filters;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL;

public partial class TestLegacyEmployeeBLL
{
    public List<TestLegacyEmployee> GetListBy(EmployeeFilterModel filter, IFetchContainer<TestLegacyEmployee> fetchs) => throw new NotImplementedException();
}
