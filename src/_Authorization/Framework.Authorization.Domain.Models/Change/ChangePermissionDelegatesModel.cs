using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.Authorization.Domain
{
    public class ChangePermissionDelegatesModel : DomainObjectBase
    {
        public ChangePermissionDelegatesModel()
        {
            this.Items = new List<DelegateToItemModel>();
        }

        [Framework.Restriction.Required]
        public Permission DelegateFromPermission { get; set; }

        [Framework.Restriction.Required]
        public IList<DelegateToItemModel> Items { get; set; }

        public void Merge(UpdatePermissionDelegatesModel updatePermissionDelegatesModel)
        {
            if (updatePermissionDelegatesModel == null) throw new ArgumentNullException(nameof(updatePermissionDelegatesModel));

            if (this.DelegateFromPermission != updatePermissionDelegatesModel.DelegateFromPermission)
            {
                throw new System.ArgumentException("Invalida delegated from principal", nameof(updatePermissionDelegatesModel));
            }

            this.Items.AddRange(updatePermissionDelegatesModel.AddItems.Where(tryAddItem => this.Items.All(item => item.Permission != tryAddItem.Permission)));

            this.Items.RemoveBy(item => updatePermissionDelegatesModel.RemoveItems.Contains(item.Permission));
        }
    }
}