using CommonFramework;

using Framework.Core;

namespace Framework.Authorization.Domain;

public class ChangePermissionDelegatesModel : DomainObjectBase
{
    public ChangePermissionDelegatesModel()
    {
        this.Items = new List<DelegateToItemModel>();
    }

    [Restriction.Required]
    public Permission DelegateFromPermission { get; set; }

    [Restriction.Required]
    public IList<DelegateToItemModel> Items { get; set; }

    public void Merge(UpdatePermissionDelegatesModel updatePermissionDelegatesModel)
    {
        if (updatePermissionDelegatesModel == null) throw new ArgumentNullException(nameof(updatePermissionDelegatesModel));

        if (this.DelegateFromPermission != updatePermissionDelegatesModel.DelegateFromPermission)
        {
            throw new ArgumentException("Invalida delegated from principal", nameof(updatePermissionDelegatesModel));
        }

        this.Items.AddRange(updatePermissionDelegatesModel.AddItems.Where(tryAddItem => this.Items.All(item => item.Permission != tryAddItem.Permission)));

        this.Items.RemoveBy(item => updatePermissionDelegatesModel.RemoveItems.Contains(item.Permission));
    }
}
