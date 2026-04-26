using Framework.BLL.Domain.Persistent;

using Anch.GenericQueryable.Fetching;

using Anch.OData.Domain;

using SampleSystem.Domain.Models.Filters;

using TestBusinessUnit = SampleSystem.Domain.Projections.TestBusinessUnit;

namespace SampleSystem.BLL;

public partial class TestBusinessUnitBLL
{
    public SelectOperationResult<HierarchicalNode<TestBusinessUnit, Guid>> GetTreeByOData(
            SelectOperation<TestBusinessUnit> selectOperation,
            HierarchicalBusinessUnitFilterModel filter,
            FetchRule<TestBusinessUnit> fetchs) =>
        this.GetTreeByOData(selectOperation, fetchs);
}
