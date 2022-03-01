using System;

using Framework.DomainDriven;
using Framework.OData;
using Framework.Persistent;

using WorkflowSampleSystem.Domain.Models.Filters;
using WorkflowSampleSystem.Domain.Projections;

namespace WorkflowSampleSystem.BLL
{
    public partial class TestBusinessUnitBLL
    {
        public SelectOperationResult<HierarchicalNode<TestBusinessUnit, Guid>> GetTreeByOData(
            SelectOperation<TestBusinessUnit> selectOperation,
            HierarchicalBusinessUnitFilterModel filter,
            IFetchContainer<TestBusinessUnit> fetchs)
        {
            return this.GetTreeByOData(selectOperation, fetchs);
        }
    }
}
