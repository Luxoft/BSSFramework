﻿using FluentValidation;

using Framework.Authorization.Domain;
using Framework.Core;
using Framework.Exceptions;
using Framework.HierarchicalExpand;
using Framework.Persistent;
using Framework.Validation;

namespace Framework.Authorization.BLL;

public partial class PermissionBLL
{
    public override void Save(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        if (this.Context.CurrentPrincipal.RunAs != null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        base.Save(permission);
    }

    protected override void Validate(Permission permission, AuthorizationOperationContext operationContext)
    {
        this.Context.PrincipalValidator.ValidateAndThrow(permission.Principal);

        base.Validate(permission, operationContext);
    }

    public void ChangeDelegatePermissions(ChangePermissionDelegatesModel changePermissionDelegatesModel)
    {
        if (changePermissionDelegatesModel == null) throw new ArgumentNullException(nameof(changePermissionDelegatesModel));

        this.Context.Validator.Validate(changePermissionDelegatesModel);

        var delegatedFromPermission = changePermissionDelegatesModel.DelegateFromPermission;

        var currentDelegatedToPermissions = delegatedFromPermission.DelegatedTo.ToList();

        var expectedDelegatedToPermissions = changePermissionDelegatesModel.Items.ToList(item => item.Permission);

        var removingItems = currentDelegatedToPermissions.GetMergeResult(expectedDelegatedToPermissions).RemovingItems;

        delegatedFromPermission.RemoveDetails(removingItems);

        this.Save(delegatedFromPermission);
    }

    public void UpdateDelegatePermissions(UpdatePermissionDelegatesModel updatePermissionDelegatesModel)
    {
        if (updatePermissionDelegatesModel == null) throw new ArgumentNullException(nameof(updatePermissionDelegatesModel));

        this.Context.Validator.Validate(updatePermissionDelegatesModel);

        var changePermissionDelegatesModel = new ChangePermissionDelegatesModel
                                             {
                                                     DelegateFromPermission = updatePermissionDelegatesModel.DelegateFromPermission,

                                                     Items = updatePermissionDelegatesModel.DelegateFromPermission.DelegatedTo.ToList(subPerm =>

                                                             new DelegateToItemModel { Permission = subPerm, Principal = subPerm.Principal })
                                             };

        changePermissionDelegatesModel.Merge(updatePermissionDelegatesModel);

        this.ChangeDelegatePermissions(changePermissionDelegatesModel);
    }

    public void WithdrawDelegation(Permission permission)
    {
        if (permission == null) throw new ArgumentNullException(nameof(permission));

        var newPeriod = permission.Period.StartDate.ToPeriod(this.Context.TimeProvider.GetToday().SubtractDay());

        permission.GetAllChildren().Foreach(p => p.Period = newPeriod);

        this.Save(permission);
    }
}
