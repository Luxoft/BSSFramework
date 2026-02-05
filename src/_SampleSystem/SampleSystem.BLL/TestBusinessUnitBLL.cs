using Framework.OData;
using Framework.Persistent;

using GenericQueryable.Fetching;

using SampleSystem.Domain.Models.Filters;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL;

public partial class TestBusinessUnitBLL
{
    public SelectOperationResult<HierarchicalNode<TestBusinessUnit, Guid>> GetTreeByOData(
            SelectOperation<TestBusinessUnit> selectOperation,
            HierarchicalBusinessUnitFilterModel filter,
            FetchRule<TestBusinessUnit> fetchs)
    {
        return this.GetTreeByOData(selectOperation, fetchs);
    }
}
