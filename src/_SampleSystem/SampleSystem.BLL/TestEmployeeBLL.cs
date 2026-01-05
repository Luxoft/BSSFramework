using Framework.DomainDriven;
using Framework.OData;

using SampleSystem.Domain;
using SampleSystem.Domain.Models.Filters;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL;

public partial class TestEmployeeBLL
{
    /// <summary>
    /// Gets the object by.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="fetchs">The fetchs.</param>
    /// <returns>TestEmployee.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public TestEmployee GetObjectBy(SingleEmployeeFilterModel filter, FetchRule<TestEmployee> fetchs) => throw new NotImplementedException();

    public List<TestEmployee> GetListBy(TestEmployeeFilter filter, FetchRule<TestEmployee> fetchs) => throw new NotImplementedException();

    /// <summary>
    /// Gets the objects by.
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="fetchs">The fetchs.</param>
    /// <returns>IList&lt;TestEmployee&gt;.</returns>
    /// <exception cref="NotImplementedException"></exception>
    public List<TestEmployee> GetListBy(EmployeeFilterModel filter, FetchRule<TestEmployee> fetchs)
        => throw new NotImplementedException();

    public SelectOperationResult<TestEmployee> GetObjectsByOData(
            SelectOperation<TestEmployee> selectOperation,
            TestEmployeeFilter filter,
            FetchRule<TestEmployee> fetchs)
    {
        var nextSelectOperation = selectOperation.AddFilter(te => te.CoreBusinessUnit.Id == filter.BusinessUnit.Id);

        var isVirtual = nextSelectOperation.IsVirtual;

        return this.GetObjectsByOData(nextSelectOperation, fetchs);
    }
}
