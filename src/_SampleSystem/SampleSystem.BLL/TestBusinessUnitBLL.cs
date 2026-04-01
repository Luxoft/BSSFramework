using Framework.BLL.Domain.Persistent;

using GenericQueryable.Fetching;

using OData.Domain;

using SampleSystem.Domain.Models.Filters;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL;

public partial class TestBusinessUnitBLL
{
    public SelectOperationResult<HierarchicalNode<TestBusinessUnit, Guid>> GetTreeByOData(
            SelectOperation<TestBusinessUnit> selectOperation,
            HierarchicalBusinessUnitFilterModel filter,
            FetchRule<TestBusinessUnit> fetchs) =>
        this.GetTreeByOData(selectOperation, fetchs);
}
