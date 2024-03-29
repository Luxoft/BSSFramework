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
    
    
    public partial class AuthorizationBLLContext : Framework.DomainDriven.BLL.Security.SecurityBLLBaseContext<Framework.Authorization.Domain.PersistentDomainObjectBase, System.Guid, Framework.Authorization.BLL.IAuthorizationBLLFactoryContainer>, Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, System.Guid>>>, Framework.Authorization.BLL.IAuthorizationBLLContext
    {
        
        Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.IDefaultBLLFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, System.Guid>> Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.IDefaultBLLFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, System.Guid>>>.Logics
        {
            get
            {
                return this.Logics;
            }
        }
        
        Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, System.Guid>> Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, System.Guid>>>.Logics
        {
            get
            {
                return this.Logics;
            }
        }
    }
    
    public partial interface IAuthorizationBLLContext : Framework.DomainDriven.BLL.Security.IAccessDeniedExceptionServiceContainer, Framework.DomainDriven.BLL.Security.ISecurityServiceContainer<Framework.DomainDriven.BLL.Security.IRootSecurityService<Framework.Authorization.Domain.PersistentDomainObjectBase>>, Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.Authorization.BLL.IAuthorizationBLLFactoryContainer>, Framework.DomainDriven.IFetchServiceContainer<Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.DomainDriven.FetchBuildRule>
    {
        
        new Framework.Authorization.BLL.IAuthorizationBLLFactoryContainer Logics
        {
            get;
        }
    }
    
    public partial class SecurityDomainBLLBase<TDomainObject> : Framework.DomainDriven.BLL.Security.DefaultSecurityDomainBLLBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, TDomainObject, System.Guid>
        where TDomainObject : Framework.Authorization.Domain.PersistentDomainObjectBase
    {
        
        public SecurityDomainBLLBase(Framework.Authorization.BLL.IAuthorizationBLLContext context, nuSpec.Abstraction.ISpecificationEvaluator specificationEvaluator = null) : 
                base(context, specificationEvaluator)
        {
        }
        
        public SecurityDomainBLLBase(Framework.Authorization.BLL.IAuthorizationBLLContext context, Framework.SecuritySystem.ISecurityProvider<TDomainObject> securityProvider, nuSpec.Abstraction.ISpecificationEvaluator specificationEvaluator = null) : 
                base(context, securityProvider, specificationEvaluator)
        {
        }
    }
    
    public partial interface IAuthorizationBLLFactoryContainer : Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Authorization.Domain.PersistentDomainObjectBase, System.Guid>>
    {
        
        Framework.Authorization.BLL.IBusinessRoleBLL BusinessRole
        {
            get;
        }
        
        Framework.Authorization.BLL.IBusinessRoleBLLFactory BusinessRoleFactory
        {
            get;
        }
        
        Framework.Authorization.BLL.IEntityTypeBLL EntityType
        {
            get;
        }
        
        Framework.Authorization.BLL.IEntityTypeBLLFactory EntityTypeFactory
        {
            get;
        }
        
        Framework.Authorization.BLL.IOperationBLL Operation
        {
            get;
        }
        
        Framework.Authorization.BLL.IOperationBLLFactory OperationFactory
        {
            get;
        }
        
        Framework.Authorization.BLL.IPermissionBLL Permission
        {
            get;
        }
        
        Framework.Authorization.BLL.IPermissionBLLFactory PermissionFactory
        {
            get;
        }
        
        Framework.Authorization.BLL.IPermissionFilterEntityBLL PermissionFilterEntity
        {
            get;
        }
        
        Framework.Authorization.BLL.IPermissionFilterEntityBLLFactory PermissionFilterEntityFactory
        {
            get;
        }
        
        Framework.Authorization.BLL.IPermissionFilterItemBLL PermissionFilterItem
        {
            get;
        }
        
        Framework.Authorization.BLL.IPermissionFilterItemBLLFactory PermissionFilterItemFactory
        {
            get;
        }
        
        Framework.Authorization.BLL.IPrincipalBLL Principal
        {
            get;
        }
        
        Framework.Authorization.BLL.IPrincipalBLLFactory PrincipalFactory
        {
            get;
        }
    }
    
    public partial interface IBusinessRoleBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.BusinessRole, System.Guid>
    {
        
        Framework.Authorization.Domain.BusinessRole Create(Framework.Authorization.Domain.BusinessRoleCreateModel createModel);
    }
    
    public partial interface IBusinessRoleBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Authorization.BLL.IBusinessRoleBLL, Framework.Authorization.Domain.BusinessRole>
    {
    }
    
    public partial interface IEntityTypeBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.EntityType, System.Guid>
    {
    }
    
    public partial interface IEntityTypeBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Authorization.BLL.IEntityTypeBLL, Framework.Authorization.Domain.EntityType>
    {
    }
    
    public partial interface IOperationBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.Operation, System.Guid>
    {
    }
    
    public partial interface IOperationBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Authorization.BLL.IOperationBLL, Framework.Authorization.Domain.Operation>
    {
    }
    
    public partial interface IPermissionBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.Permission, System.Guid>
    {
        
        System.Collections.Generic.List<Framework.Authorization.Domain.Permission> GetListBy(Framework.Authorization.Domain.PermissionDirectFilterModel filter, Framework.DomainDriven.IFetchContainer<Framework.Authorization.Domain.Permission> fetchs);
    }
    
    public partial interface IPermissionBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Authorization.BLL.IPermissionBLL, Framework.Authorization.Domain.Permission>
    {
    }
    
    public partial interface IPermissionFilterEntityBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.PermissionFilterEntity, System.Guid>
    {
    }
    
    public partial interface IPermissionFilterEntityBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Authorization.BLL.IPermissionFilterEntityBLL, Framework.Authorization.Domain.PermissionFilterEntity>
    {
    }
    
    public partial interface IPermissionFilterItemBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.PermissionFilterItem, System.Guid>
    {
    }
    
    public partial interface IPermissionFilterItemBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Authorization.BLL.IPermissionFilterItemBLL, Framework.Authorization.Domain.PermissionFilterItem>
    {
    }
    
    public partial interface IPrincipalBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Authorization.BLL.IAuthorizationBLLContext, Framework.Authorization.Domain.PersistentDomainObjectBase, Framework.Authorization.Domain.Principal, System.Guid>
    {
        
        Framework.Authorization.Domain.Principal Create(Framework.Authorization.Domain.PrincipalCreateModel createModel);
    }
    
    public partial interface IPrincipalBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Authorization.BLL.IPrincipalBLL, Framework.Authorization.Domain.Principal>
    {
    }
}
