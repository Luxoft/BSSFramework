using Framework.Authorization.Domain;
using Framework.DomainDriven;
using Framework.Validation;

namespace Framework.Authorization.BLL;

public partial class PermissionBLL
{
    public List<Permission> GetListBy(PermissionDirectFilterModel filter, FetchRule<Permission> fetchs)
    {
        if (filter == null) throw new ArgumentNullException(nameof(filter));

        this.Context.Validator.Validate(filter);

        var innerFilter = new PermissionDirectInternalFilterModel(this.Context, filter);

        return this.GetListBy(innerFilter, fetchs);
    }
}
