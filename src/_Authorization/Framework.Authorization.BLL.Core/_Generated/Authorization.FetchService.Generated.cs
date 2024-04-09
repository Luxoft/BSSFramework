﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Framework.Authorization.BLL
{
    
    
    public partial class AuthorizationMainFetchServiceBase : Framework.DomainDriven.MainFetchServiceBase<Framework.Authorization.Domain.PersistentDomainObjectBase>
    {
        
        protected virtual Framework.DomainDriven.IFetchContainer<Framework.Authorization.Domain.BusinessRole> GetBusinessRoleContainer(Framework.Transfering.ViewDTOType rule)
        {
            if ((rule == Framework.Transfering.ViewDTOType.VisualDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.BusinessRole>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.SimpleDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.BusinessRole>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.FullDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.BusinessRole>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.RichDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.BusinessRole>.Empty;
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("rule");
            }
        }
        
        protected override Framework.DomainDriven.IFetchContainer<TDomainObject> GetContainer<TDomainObject>(Framework.Transfering.ViewDTOType rule)
        {
            if ((typeof(TDomainObject) == typeof(Framework.Authorization.Domain.BusinessRole)))
            {
                return ((Framework.DomainDriven.IFetchContainer<TDomainObject>)(this.GetBusinessRoleContainer(rule)));
            }
            else if ((typeof(TDomainObject) == typeof(Framework.Authorization.Domain.EntityType)))
            {
                return ((Framework.DomainDriven.IFetchContainer<TDomainObject>)(this.GetEntityTypeContainer(rule)));
            }
            else if ((typeof(TDomainObject) == typeof(Framework.Authorization.Domain.Permission)))
            {
                return ((Framework.DomainDriven.IFetchContainer<TDomainObject>)(this.GetPermissionContainer(rule)));
            }
            else if ((typeof(TDomainObject) == typeof(Framework.Authorization.Domain.PermissionFilterEntity)))
            {
                return ((Framework.DomainDriven.IFetchContainer<TDomainObject>)(this.GetPermissionFilterEntityContainer(rule)));
            }
            else if ((typeof(TDomainObject) == typeof(Framework.Authorization.Domain.PermissionFilterItem)))
            {
                return ((Framework.DomainDriven.IFetchContainer<TDomainObject>)(this.GetPermissionFilterItemContainer(rule)));
            }
            else if ((typeof(TDomainObject) == typeof(Framework.Authorization.Domain.Principal)))
            {
                return ((Framework.DomainDriven.IFetchContainer<TDomainObject>)(this.GetPrincipalContainer(rule)));
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("TDomainObject");
            }
        }
        
        protected virtual Framework.DomainDriven.IFetchContainer<Framework.Authorization.Domain.EntityType> GetEntityTypeContainer(Framework.Transfering.ViewDTOType rule)
        {
            if ((rule == Framework.Transfering.ViewDTOType.VisualDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.EntityType>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.SimpleDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.EntityType>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.FullDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.EntityType>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.RichDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.EntityType>.Empty;
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("rule");
            }
        }
        
        protected virtual Framework.DomainDriven.IFetchContainer<Framework.Authorization.Domain.Permission> GetPermissionContainer(Framework.Transfering.ViewDTOType rule)
        {
            if ((rule == Framework.Transfering.ViewDTOType.VisualDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.Permission>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.SimpleDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.Permission>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.FullDTO))
            {
                return Framework.DomainDriven.FetchContainer.Create<Framework.Authorization.Domain.Permission>(
                    fetchRootRule => fetchRootRule.SelectNested(permission => permission.DelegatedFrom).SelectNested(permission => permission.Principal),
                    fetchRootRule => fetchRootRule.SelectNested(permission => permission.Principal),
                    fetchRootRule => fetchRootRule.SelectNested(permission => permission.Role));
            }
            else if ((rule == Framework.Transfering.ViewDTOType.RichDTO))
            {
                return Framework.DomainDriven.FetchContainer.Create<Framework.Authorization.Domain.Permission>(
                    fetchRootRule => fetchRootRule.SelectNested(permission => permission.DelegatedFrom).SelectNested(permission => permission.Principal),
                    fetchRootRule => fetchRootRule.SelectMany(permission => permission.DelegatedTo).SelectNested(permission => permission.DelegatedFrom).SelectNested(permission => permission.Principal),
                    fetchRootRule => fetchRootRule.SelectMany(permission => permission.DelegatedTo).SelectMany(permission => permission.FilterItems).SelectNested(permissionFilterItem => permissionFilterItem.Entity),
                    fetchRootRule => fetchRootRule.SelectMany(permission => permission.DelegatedTo).SelectMany(permission => permission.FilterItems).SelectNested(permissionFilterItem => permissionFilterItem.EntityType),
                    fetchRootRule => fetchRootRule.SelectMany(permission => permission.DelegatedTo).SelectNested(permission => permission.Principal),
                    fetchRootRule => fetchRootRule.SelectMany(permission => permission.DelegatedTo).SelectNested(permission => permission.Role),
                    fetchRootRule => fetchRootRule.SelectMany(permission => permission.FilterItems).SelectNested(permissionFilterItem => permissionFilterItem.Entity),
                    fetchRootRule => fetchRootRule.SelectMany(permission => permission.FilterItems).SelectNested(permissionFilterItem => permissionFilterItem.EntityType),
                    fetchRootRule => fetchRootRule.SelectNested(permission => permission.Principal),
                    fetchRootRule => fetchRootRule.SelectNested(permission => permission.Role));
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("rule");
            }
        }
        
        protected virtual Framework.DomainDriven.IFetchContainer<Framework.Authorization.Domain.PermissionFilterEntity> GetPermissionFilterEntityContainer(Framework.Transfering.ViewDTOType rule)
        {
            if ((rule == Framework.Transfering.ViewDTOType.VisualDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.PermissionFilterEntity>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.SimpleDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.PermissionFilterEntity>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.FullDTO))
            {
                return Framework.DomainDriven.FetchContainer.Create<Framework.Authorization.Domain.PermissionFilterEntity>(fetchRootRule => fetchRootRule.SelectNested(permissionFilterEntity => permissionFilterEntity.EntityType));
            }
            else if ((rule == Framework.Transfering.ViewDTOType.RichDTO))
            {
                return Framework.DomainDriven.FetchContainer.Create<Framework.Authorization.Domain.PermissionFilterEntity>(fetchRootRule => fetchRootRule.SelectNested(permissionFilterEntity => permissionFilterEntity.EntityType));
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("rule");
            }
        }
        
        protected virtual Framework.DomainDriven.IFetchContainer<Framework.Authorization.Domain.PermissionFilterItem> GetPermissionFilterItemContainer(Framework.Transfering.ViewDTOType rule)
        {
            if ((rule == Framework.Transfering.ViewDTOType.VisualDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.PermissionFilterItem>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.SimpleDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.PermissionFilterItem>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.FullDTO))
            {
                return Framework.DomainDriven.FetchContainer.Create<Framework.Authorization.Domain.PermissionFilterItem>(
                    fetchRootRule => fetchRootRule.SelectNested(permissionFilterItem => permissionFilterItem.Entity),
                    fetchRootRule => fetchRootRule.SelectNested(permissionFilterItem => permissionFilterItem.EntityType),
                    fetchRootRule => fetchRootRule.SelectNested(permissionFilterItem => permissionFilterItem.Permission));
            }
            else if ((rule == Framework.Transfering.ViewDTOType.RichDTO))
            {
                return Framework.DomainDriven.FetchContainer.Create<Framework.Authorization.Domain.PermissionFilterItem>(
                    fetchRootRule => fetchRootRule.SelectNested(permissionFilterItem => permissionFilterItem.Entity),
                    fetchRootRule => fetchRootRule.SelectNested(permissionFilterItem => permissionFilterItem.EntityType),
                    fetchRootRule => fetchRootRule.SelectNested(permissionFilterItem => permissionFilterItem.Permission));
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("rule");
            }
        }
        
        protected virtual Framework.DomainDriven.IFetchContainer<Framework.Authorization.Domain.Principal> GetPrincipalContainer(Framework.Transfering.ViewDTOType rule)
        {
            if ((rule == Framework.Transfering.ViewDTOType.VisualDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.Principal>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.SimpleDTO))
            {
                return Framework.DomainDriven.FetchContainer<Framework.Authorization.Domain.Principal>.Empty;
            }
            else if ((rule == Framework.Transfering.ViewDTOType.FullDTO))
            {
                return Framework.DomainDriven.FetchContainer.Create<Framework.Authorization.Domain.Principal>(fetchRootRule => fetchRootRule.SelectNested(principal => principal.RunAs));
            }
            else if ((rule == Framework.Transfering.ViewDTOType.RichDTO))
            {
                return Framework.DomainDriven.FetchContainer.Create<Framework.Authorization.Domain.Principal>(
                    fetchRootRule => fetchRootRule.SelectMany(principal => principal.Permissions).SelectMany(permission => permission.DelegatedTo).SelectNested(permission => permission.DelegatedFrom).SelectNested(permission => permission.Principal),
                    fetchRootRule => fetchRootRule.SelectMany(principal => principal.Permissions).SelectMany(permission => permission.DelegatedTo).SelectMany(permission => permission.FilterItems).SelectNested(permissionFilterItem => permissionFilterItem.Entity),
                    fetchRootRule => fetchRootRule.SelectMany(principal => principal.Permissions).SelectMany(permission => permission.DelegatedTo).SelectMany(permission => permission.FilterItems).SelectNested(permissionFilterItem => permissionFilterItem.EntityType),
                    fetchRootRule => fetchRootRule.SelectMany(principal => principal.Permissions).SelectMany(permission => permission.DelegatedTo).SelectNested(permission => permission.Principal),
                    fetchRootRule => fetchRootRule.SelectMany(principal => principal.Permissions).SelectMany(permission => permission.DelegatedTo).SelectNested(permission => permission.Role),
                    fetchRootRule => fetchRootRule.SelectMany(principal => principal.Permissions).SelectMany(permission => permission.FilterItems).SelectNested(permissionFilterItem => permissionFilterItem.Entity),
                    fetchRootRule => fetchRootRule.SelectMany(principal => principal.Permissions).SelectMany(permission => permission.FilterItems).SelectNested(permissionFilterItem => permissionFilterItem.EntityType),
                    fetchRootRule => fetchRootRule.SelectMany(principal => principal.Permissions).SelectNested(permission => permission.Role),
                    fetchRootRule => fetchRootRule.SelectNested(principal => principal.RunAs));
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("rule");
            }
        }
    }
    
    public partial class AuthorizationMainFetchService : Framework.Authorization.BLL.AuthorizationMainFetchServiceBase
    {
    }
}
