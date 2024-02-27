﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Framework.Configuration.BLL
{
    
    
    public partial class ConfigurationBLLContext : Framework.DomainDriven.BLL.Security.SecurityBLLBaseContext<Framework.Configuration.Domain.PersistentDomainObjectBase, System.Guid, Framework.Configuration.BLL.IConfigurationBLLFactoryContainer>, Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, System.Guid>>>, Framework.Configuration.BLL.IConfigurationBLLContext
    {
        
        Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.IDefaultBLLFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, System.Guid>> Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.IDefaultBLLFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, System.Guid>>>.Logics
        {
            get
            {
                return this.Logics;
            }
        }
        
        Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, System.Guid>> Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, System.Guid>>>.Logics
        {
            get
            {
                return this.Logics;
            }
        }
    }
    
    public partial interface IConfigurationBLLContext : Framework.DomainDriven.BLL.Security.IAccessDeniedExceptionServiceContainer, Framework.DomainDriven.BLL.Security.ISecurityServiceContainer<Framework.DomainDriven.BLL.Security.IRootSecurityService<Framework.Configuration.Domain.PersistentDomainObjectBase>>, Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.Configuration.BLL.IConfigurationBLLFactoryContainer>, Framework.DomainDriven.IFetchServiceContainer<Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.DomainDriven.FetchBuildRule>
    {
        
        new Framework.Configuration.BLL.IConfigurationBLLFactoryContainer Logics
        {
            get;
        }
    }
    
    public partial class SecurityDomainBLLBase<TDomainObject, TOperation> : Framework.DomainDriven.BLL.Security.DefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, TDomainObject, System.Guid, TOperation>
        where TDomainObject : Framework.Configuration.Domain.PersistentDomainObjectBase
        where TOperation :  struct, System.Enum
    {
        
        public SecurityDomainBLLBase(Framework.Configuration.BLL.IConfigurationBLLContext context, nuSpec.Abstraction.ISpecificationEvaluator specificationEvaluator = null) : 
                base(context, specificationEvaluator)
        {
        }
        
        public SecurityDomainBLLBase(Framework.Configuration.BLL.IConfigurationBLLContext context, Framework.SecuritySystem.ISecurityProvider<TDomainObject> securityProvider, nuSpec.Abstraction.ISpecificationEvaluator specificationEvaluator = null) : 
                base(context, securityProvider, specificationEvaluator)
        {
        }
    }
    
    public partial class SecurityDomainBLLBase<TDomainObject> : Framework.Configuration.BLL.SecurityDomainBLLBase<TDomainObject, Framework.DomainDriven.BLL.BLLBaseOperation>
        where TDomainObject : Framework.Configuration.Domain.PersistentDomainObjectBase
    {
        
        public SecurityDomainBLLBase(Framework.Configuration.BLL.IConfigurationBLLContext context) : 
                base(context)
        {
        }
        
        public SecurityDomainBLLBase(Framework.Configuration.BLL.IConfigurationBLLContext context, Framework.SecuritySystem.ISecurityProvider<TDomainObject> securityProvider) : 
                base(context, securityProvider)
        {
        }
    }
    
    public partial interface IConfigurationBLLFactoryContainer : Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Configuration.Domain.PersistentDomainObjectBase, System.Guid>>
    {
        
        Framework.Configuration.BLL.ICodeFirstSubscriptionBLL CodeFirstSubscription
        {
            get;
        }
        
        Framework.Configuration.BLL.ICodeFirstSubscriptionBLLFactory CodeFirstSubscriptionFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.IDomainObjectEventBLL DomainObjectEvent
        {
            get;
        }
        
        Framework.Configuration.BLL.IDomainObjectEventBLLFactory DomainObjectEventFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.IDomainObjectModificationBLL DomainObjectModification
        {
            get;
        }
        
        Framework.Configuration.BLL.IDomainObjectModificationBLLFactory DomainObjectModificationFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.IDomainObjectNotificationBLL DomainObjectNotification
        {
            get;
        }
        
        Framework.Configuration.BLL.IDomainObjectNotificationBLLFactory DomainObjectNotificationFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.IDomainTypeBLL DomainType
        {
            get;
        }
        
        Framework.Configuration.BLL.IDomainTypeBLLFactory DomainTypeFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.IExceptionMessageBLL ExceptionMessage
        {
            get;
        }
        
        Framework.Configuration.BLL.IExceptionMessageBLLFactory ExceptionMessageFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.IGenericNamedLockBLL GenericNamedLock
        {
            get;
        }
        
        Framework.Configuration.BLL.IGenericNamedLockBLLFactory GenericNamedLockFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.ISentMessageBLL SentMessage
        {
            get;
        }
        
        Framework.Configuration.BLL.ISentMessageBLLFactory SentMessageFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.ISequenceBLL Sequence
        {
            get;
        }
        
        Framework.Configuration.BLL.ISequenceBLLFactory SequenceFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.ISystemConstantBLL SystemConstant
        {
            get;
        }
        
        Framework.Configuration.BLL.ISystemConstantBLLFactory SystemConstantFactory
        {
            get;
        }
        
        Framework.Configuration.BLL.ITargetSystemBLL TargetSystem
        {
            get;
        }
        
        Framework.Configuration.BLL.ITargetSystemBLLFactory TargetSystemFactory
        {
            get;
        }
    }
    
    public partial interface ICodeFirstSubscriptionBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.CodeFirstSubscription, System.Guid>
    {
    }
    
    public partial interface ICodeFirstSubscriptionBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.ICodeFirstSubscriptionBLL, Framework.Configuration.Domain.CodeFirstSubscription>
    {
    }
    
    public partial interface IDomainObjectEventBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainObjectEvent, System.Guid>
    {
    }
    
    public partial interface IDomainObjectEventBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.IDomainObjectEventBLL, Framework.Configuration.Domain.DomainObjectEvent>
    {
    }
    
    public partial interface IDomainObjectModificationBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainObjectModification, System.Guid>
    {
    }
    
    public partial interface IDomainObjectModificationBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.IDomainObjectModificationBLL, Framework.Configuration.Domain.DomainObjectModification>
    {
    }
    
    public partial interface IDomainObjectNotificationBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainObjectNotification, System.Guid>
    {
    }
    
    public partial interface IDomainObjectNotificationBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.IDomainObjectNotificationBLL, Framework.Configuration.Domain.DomainObjectNotification>
    {
    }
    
    public partial interface IDomainTypeBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.DomainType, System.Guid>
    {
    }
    
    public partial interface IDomainTypeBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.IDomainTypeBLL, Framework.Configuration.Domain.DomainType>
    {
    }
    
    public partial interface IExceptionMessageBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.ExceptionMessage, System.Guid>
    {
    }
    
    public partial interface IExceptionMessageBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.IExceptionMessageBLL, Framework.Configuration.Domain.ExceptionMessage>
    {
    }
    
    public partial interface IGenericNamedLockBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.GenericNamedLock, System.Guid>
    {
    }
    
    public partial interface IGenericNamedLockBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.IGenericNamedLockBLL, Framework.Configuration.Domain.GenericNamedLock>
    {
    }
    
    public partial interface ISentMessageBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.SentMessage, System.Guid>
    {
    }
    
    public partial interface ISentMessageBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.ISentMessageBLL, Framework.Configuration.Domain.SentMessage>
    {
    }
    
    public partial interface ISequenceBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.Sequence, System.Guid>
    {
        
        Framework.Configuration.Domain.Sequence Create(Framework.Configuration.Domain.SequenceCreateModel createModel);
    }
    
    public partial interface ISequenceBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.ISequenceBLL, Framework.Configuration.Domain.Sequence>
    {
    }
    
    public partial interface ISystemConstantBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.SystemConstant, System.Guid>
    {
    }
    
    public partial interface ISystemConstantBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.ISystemConstantBLL, Framework.Configuration.Domain.SystemConstant>
    {
    }
    
    public partial interface ITargetSystemBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Domain.PersistentDomainObjectBase, Framework.Configuration.Domain.TargetSystem, System.Guid>
    {
    }
    
    public partial interface ITargetSystemBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Configuration.BLL.ITargetSystemBLL, Framework.Configuration.Domain.TargetSystem>
    {
    }
}
