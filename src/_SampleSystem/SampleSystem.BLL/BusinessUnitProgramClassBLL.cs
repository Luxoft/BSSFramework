using System.Linq.Expressions;

using Framework.OData;

using GenericQueryable.Fetching;

using SampleSystem.Domain;
using SampleSystem.Domain.Models.Filters;
using SampleSystem.Domain.Projections;

namespace SampleSystem.BLL;

public partial class BusinessUnitProgramClassBLL
{
    public SelectOperationResult<BusinessUnitProgramClass> GetObjectsByOData(
            SelectOperation<BusinessUnitProgramClass> selectOperation,
            BusinessUnitProgramClassFilterModel filter,
            FetchRule<BusinessUnitProgramClass> fetchs)
    {
        var nextSelectOperation = selectOperation.AddFilter(this.GetFilter(filter)).AddFilter(this.GetVirtualFilter(filter));

        var result = this.GetObjectsByOData(nextSelectOperation, fetchs);

        return result;

    }

    private Expression<Func<BusinessUnitProgramClass, bool>> GetVirtualFilter(BusinessUnitProgramClassFilterModel filter)
    {
        if (string.IsNullOrEmpty(filter.FilterVirtualName))
        {
            return z => true;
        }

        return z => z.VirtualName.Contains(filter.FilterVirtualName);
    }

    private Expression<Func<BusinessUnitProgramClass, bool>> GetFilter(BusinessUnitProgramClassFilterModel filter)
    {
        if (null == filter.AncestorIdent)
        {
            return z => true;
        }

        var childQueryable = this.Context.Logics.Default.Create<BusinessUnitAncestorLink>()
                                 .GetUnsecureQueryable()
                                 .Where(z => z.Ancestor.Id == filter.AncestorIdent)
                                 .Select(z => z.Child.Id);

        return z => childQueryable.Contains(z.Id);
    }
}
