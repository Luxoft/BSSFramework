﻿using Framework.Authorization.Domain;
using Framework.Core;
using Framework.DomainDriven;
using Framework.DomainDriven.BLL;
using Framework.Exceptions;
using Framework.Persistent;

namespace Framework.Authorization.BLL;

public partial class BusinessRoleBLL
{
    public BusinessRole GetOrCreateAdminRole()
    {
        return this.GetByNameOrCreate(BusinessRole.AdminRoleName, true);
    }

    public BusinessRole GetAdminRole()
    {
        return this.GetByName(BusinessRole.AdminRoleName);
    }

    public BusinessRole GetByNameOrCreate(string name, bool autoSave = false)
    {
        if (name == null) throw new ArgumentNullException(nameof(name));

        return this.GetByName(name) ?? new BusinessRole { Name = name }.Self(autoSave, this.Save);
    }

    public override void Save(BusinessRole domainObject)
    {
        if (this.Context.CurrentPrincipal.RunAs != null)
        {
            throw new BusinessLogicException("RunAs mode must be disabled");
        }

        base.Save(domainObject);
    }

    private void RecalculateOperations(BusinessRole businessRole, bool withParents)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        if (withParents)
        {
            var parentRoles = this.GetUnsecureQueryable().Where(br => br.SubBusinessRoleLinks.Any(link => link.SubBusinessRole == businessRole)).ToList();

            parentRoles.Foreach(this.Save);
        }
    }

    public bool HasAdminRole()
    {
        var adminRole = this.GetAdminRole();

        return adminRole != null
               && this.HasBusinessRole(adminRole.Name);
    }

    public bool HasBusinessRole(string roleName, bool withRunAs = true)
    {
        if (roleName == null) throw new ArgumentNullException(nameof(roleName));

        return this.Context.AvailablePermissionSource.GetAvailablePermissionsQueryable(withRunAs)
                   .Any(permission => permission.Role.Name == roleName);
    }

    protected override void PreValidate(BusinessRole businessRole, AuthorizationOperationContext operationContext)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        businessRole.CheckChildCyclicReference();

        base.PreValidate(businessRole, operationContext);
    }

    protected override void PreRecalculate(BusinessRole businessRole)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        this.RecalculateOperations(businessRole, true);

        base.PreRecalculate(businessRole);
    }

    protected override void PostValidate(BusinessRole businessRole, AuthorizationOperationContext operationContext)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        this.ValidateDelegatePermissions(businessRole, true);
    }

    private void ValidateDelegatePermissions(BusinessRole businessRole, bool withParents)
    {
        var permissionBLL = this.Context.Logics.Permission;

        permissionBLL.GetListBy(permission => permission.Role == businessRole).Foreach(p => permissionBLL.ValidatePermissionDelegated(p, ValidatePermissonDelegateMode.Role));

        if (withParents)
        {
            var parentRoles = this.GetUnsecureQueryable().Where(br => br.SubBusinessRoleLinks.Any(link => link.SubBusinessRole == businessRole)).ToList();

            parentRoles.Foreach(parentRole => this.ValidateDelegatePermissions(parentRole, true));
        }
    }

    public override void Remove(BusinessRole businessRole)
    {
        if (businessRole == null) throw new ArgumentNullException(nameof(businessRole));

        if (businessRole.SubBusinessRoles.Any())
        {
            throw new BusinessLogicException($"Removing business role \"{businessRole.Name}\" must be empty");
        }

        var linksBLL = this.Context.Logics.Default.Create<SubBusinessRoleLink>();

        var removingLinks = linksBLL.GetListBy(link => link.SubBusinessRole == businessRole);

        removingLinks.Foreach(linksBLL.Remove);

        base.Remove(businessRole);
    }

    public BusinessRole Create(BusinessRoleCreateModel createModel)
    {
        if (createModel == null) throw new ArgumentNullException(nameof(createModel));

        return new BusinessRole();
    }

    public IEnumerable<BusinessRole> GetParents(ICollection<BusinessRole> businessRoles)
    {
        if (businessRoles == null) throw new ArgumentNullException(nameof(businessRoles));

        if (!businessRoles.Any())
        {
            yield break;
        }

        var allRoles = this.GetFullList(x => x.SelectMany(l => l.SubBusinessRoleLinks));

        foreach (var role in businessRoles.GetAllElements(r => allRoles.Where(x => x.SubBusinessRoles.Contains(r))).Distinct())
        {
            yield return role;
        }
    }

    public IEnumerable<BusinessRoleNode> GetNodes()
    {
        var businessRoles = this.GetFullList(rule => rule.SelectMany(role => role.SubBusinessRoleLinks));

        return from businessRole in businessRoles

               let parentBusinessRole = businessRoles.SingleOrDefault(role => role.SubBusinessRoles.Contains(businessRole), () => new BusinessLogicException(
                                                                       $"More one parent for business role \"{businessRole.Name}\""))

               select new BusinessRoleNode(businessRole, parentBusinessRole);
    }

    public BusinessRole Save(BusinessRoleNode businessRoleNode)
    {
        if (businessRoleNode == null) throw new ArgumentNullException(nameof(businessRoleNode));

        var businessRole = this.GetByIdOrCreate(businessRoleNode.Id);

        businessRole.Name = businessRoleNode.Name;
        businessRole.Description = businessRoleNode.Description;

        var newParentRole = this.GetById(businessRoleNode.ParentId, IdCheckMode.SkipEmpty);

        if (!businessRole.IsNew)
        {
            var prevNode = this.GetNodes().Single(node => node.Id == businessRole.Id);

            var prevParentRole = this.GetById(prevNode.ParentId, IdCheckMode.SkipEmpty);

            if (prevParentRole != null && prevParentRole != newParentRole)
            {
                var prevParentRoleLink = prevParentRole.SubBusinessRoleLinks.Single(link => link.SubBusinessRole == businessRole);

                prevParentRole.RemoveDetail(prevParentRoleLink);

                this.Context.Logics.Default.Create<SubBusinessRoleLink>().Remove(prevParentRoleLink);
                this.Save(prevParentRole);
            }
        }

        this.Save(businessRole);

        if (newParentRole.Maybe(v => !v.SubBusinessRoles.Contains(businessRole)))
        {
            var newParentRoleLink = new SubBusinessRoleLink(newParentRole)
                                    {
                                            SubBusinessRole = businessRole
                                    };

            this.Context.Logics.Default.Create<SubBusinessRoleLink>().Save(newParentRoleLink);
            this.Save(newParentRole);
        }

        return businessRole;
    }
}
