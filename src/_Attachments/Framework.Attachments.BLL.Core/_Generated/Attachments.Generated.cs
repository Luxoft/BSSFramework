﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Framework.Attachments.BLL
{
    
    
    #region 
	static
    public class AttachmentsSecurityOperation
    {
        
        private static Framework.SecuritySystem.DisabledSecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode> _disabled = new Framework.SecuritySystem.DisabledSecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode>();
        
        public static Framework.SecuritySystem.DisabledSecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode> Disabled
        {
            get
            {
                return _disabled;
            }
        }
        
        public static Framework.SecuritySystem.SecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode> GetByCode(Framework.Attachments.AttachmentsSecurityOperationCode code)
        {
            if ((code == Framework.Attachments.AttachmentsSecurityOperationCode.Disabled))
            {
                return Framework.Attachments.BLL.AttachmentsSecurityOperation.Disabled;
            }
            else
            {
                throw new System.ArgumentOutOfRangeException("code");
            }
        }
        
        public static Framework.SecuritySystem.SecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode> GetByMode<TDomainObject>(Framework.SecuritySystem.BLLSecurityMode mode)
            where TDomainObject : Framework.Attachments.Domain.PersistentDomainObjectBase
        {
            Framework.Attachments.AttachmentsSecurityOperationCode securityOperationCode = Framework.Attachments.BLL.AttachmentsSecurityOperation.GetCodeByMode<TDomainObject>(mode);
            if ((securityOperationCode == Framework.Attachments.AttachmentsSecurityOperationCode.Disabled))
            {
                return Framework.Attachments.BLL.AttachmentsSecurityOperation.Disabled;
            }
            else
            {
                return Framework.Attachments.BLL.AttachmentsSecurityOperation.GetByCode(securityOperationCode);
            }
        }
        
        public static Framework.Attachments.AttachmentsSecurityOperationCode GetCodeByMode(System.Type domainType, Framework.SecuritySystem.BLLSecurityMode mode)
        {
            return Framework.Attachments.AttachmentsSecurityOperationCode.Disabled;
        }
        
        private static Framework.Attachments.AttachmentsSecurityOperationCode GetCodeByMode<TDomainObject>(Framework.SecuritySystem.BLLSecurityMode mode)
            where TDomainObject : Framework.Attachments.Domain.PersistentDomainObjectBase
        {
            return Framework.Attachments.BLL.AttachmentsSecurityOperation.GetCodeByMode(typeof(TDomainObject), mode);
        }
    }
    #endregion
    
    public partial class AttachmentsSecurityPath<TDomainObject> : Framework.SecuritySystem.SecurityPathWrapper<Framework.Attachments.Domain.PersistentDomainObjectBase, TDomainObject, System.Guid>
        where TDomainObject : Framework.Attachments.Domain.PersistentDomainObjectBase
    {
        
        private AttachmentsSecurityPath(Framework.SecuritySystem.SecurityPath<Framework.Attachments.Domain.PersistentDomainObjectBase, TDomainObject, System.Guid> securityPath) : 
                base(securityPath)
        {
        }
        
        public static implicit operator Framework.Attachments.BLL.AttachmentsSecurityPath<TDomainObject> (Framework.SecuritySystem.SecurityPath<Framework.Attachments.Domain.PersistentDomainObjectBase, TDomainObject, System.Guid> securityPath)
        {
            return new Framework.Attachments.BLL.AttachmentsSecurityPath<TDomainObject>(securityPath);
        }
    }
    
    public partial class AttachmentsBLLContext : Framework.DomainDriven.BLL.Security.SecurityBLLBaseContext<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.DomainObjectBase, System.Guid, Framework.Attachments.BLL.IAttachmentsBLLFactoryContainer, Framework.Attachments.AttachmentsSecurityOperationCode>, Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode, System.Guid>>>, Framework.Attachments.BLL.IAttachmentsBLLContext
    {
        
        Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.IDefaultBLLFactory<Framework.Attachments.Domain.PersistentDomainObjectBase, System.Guid>> Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.IDefaultBLLFactory<Framework.Attachments.Domain.PersistentDomainObjectBase, System.Guid>>>.Logics
        {
            get
            {
                return this.Logics;
            }
        }
        
        Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode, System.Guid>> Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode, System.Guid>>>.Logics
        {
            get
            {
                return this.Logics;
            }
        }
        
        public override Framework.SecuritySystem.SecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode> GetSecurityOperation(Framework.Attachments.AttachmentsSecurityOperationCode securityOperationCode)
        {
            return Framework.Attachments.BLL.AttachmentsSecurityOperation.GetByCode(securityOperationCode);
        }
        
        public override Framework.SecuritySystem.SecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode> GetSecurityOperation<TDomainObject>(Framework.SecuritySystem.BLLSecurityMode securitMode)
        {
            return Framework.Attachments.BLL.AttachmentsSecurityOperation.GetByMode<TDomainObject>(securitMode);
        }
    }
    
    public partial interface IAttachmentsBLLContext : Framework.DomainDriven.BLL.Security.IAccessDeniedExceptionServiceContainer<Framework.Attachments.Domain.PersistentDomainObjectBase>, Framework.DomainDriven.BLL.Security.ISecurityServiceContainer<Framework.Attachments.BLL.IAttachmentsSecurityService>, Framework.DomainDriven.BLL.IBLLFactoryContainerContext<Framework.Attachments.BLL.IAttachmentsBLLFactoryContainer>, Framework.DomainDriven.IFetchServiceContainer<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.DomainDriven.FetchBuildRule>, Framework.SecuritySystem.ISecurityOperationResolver<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode>
    {
        
        new Framework.Attachments.BLL.IAttachmentsBLLFactoryContainer Logics
        {
            get;
        }
    }
    
    public partial class DomainBLLBase<TDomainObject, TOperation> : Framework.DomainDriven.BLL.DefaultDomainBLLBase<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.DomainObjectBase, TDomainObject, System.Guid, TOperation>
        where TDomainObject : Framework.Attachments.Domain.PersistentDomainObjectBase
        where TOperation :  struct, System.Enum
    {
        
        public DomainBLLBase(Framework.Attachments.BLL.IAttachmentsBLLContext context, nuSpec.Abstraction.ISpecificationEvaluator specificationEvaluator = null) : 
                base(context, specificationEvaluator)
        {
        }
    }
    
    public abstract partial class SecurityDomainBLLBase<TDomainObject, TOperation> : Framework.DomainDriven.BLL.Security.DefaultSecurityDomainBLLBase<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.DomainObjectBase, TDomainObject, System.Guid, TOperation>
        where TDomainObject : Framework.Attachments.Domain.PersistentDomainObjectBase
        where TOperation :  struct, System.Enum
    {
        
        protected SecurityDomainBLLBase(Framework.Attachments.BLL.IAttachmentsBLLContext context, nuSpec.Abstraction.ISpecificationEvaluator specificationEvaluator = null) : 
                base(context, specificationEvaluator)
        {
        }
        
        protected SecurityDomainBLLBase(Framework.Attachments.BLL.IAttachmentsBLLContext context, Framework.SecuritySystem.ISecurityProvider<TDomainObject> securityOperation, nuSpec.Abstraction.ISpecificationEvaluator specificationEvaluator = null) : 
                base(context, securityOperation, specificationEvaluator)
        {
        }
    }
    
    public partial class DomainBLLBase<TDomainObject> : Framework.Attachments.BLL.DomainBLLBase<TDomainObject, Framework.DomainDriven.BLL.BLLBaseOperation>
        where TDomainObject : Framework.Attachments.Domain.PersistentDomainObjectBase
    {
        
        public DomainBLLBase(Framework.Attachments.BLL.IAttachmentsBLLContext context) : 
                base(context)
        {
        }
    }
    
    public partial class SecurityDomainBLLBase<TDomainObject> : Framework.Attachments.BLL.SecurityDomainBLLBase<TDomainObject, Framework.DomainDriven.BLL.BLLBaseOperation>
        where TDomainObject : Framework.Attachments.Domain.PersistentDomainObjectBase
    {
        
        public SecurityDomainBLLBase(Framework.Attachments.BLL.IAttachmentsBLLContext context) : 
                base(context)
        {
        }
        
        public SecurityDomainBLLBase(Framework.Attachments.BLL.IAttachmentsBLLContext context, Framework.SecuritySystem.ISecurityProvider<TDomainObject> securityOperation) : 
                base(context, securityOperation)
        {
        }
    }
    
    public partial class AttachmentsSecurityService : Framework.Attachments.BLL.AttachmentsSecurityServiceBase, Framework.Attachments.BLL.IAttachmentsSecurityService
    {
        
        public AttachmentsSecurityService(Framework.Attachments.BLL.IAttachmentsBLLContext context) : 
                base(context)
        {
        }
    }
    
    public abstract partial class AttachmentsSecurityServiceBase : Framework.DomainDriven.BLL.Security.RootSecurityService<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode>
    {
        
        protected AttachmentsSecurityServiceBase(Framework.Attachments.BLL.IAttachmentsBLLContext context) : 
                base(context)
        {
        }
        
        public static void Register(Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection)
        {
            Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped<Framework.SecuritySystem.IDomainSecurityService<Framework.Attachments.Domain.Attachment, Framework.Attachments.AttachmentsSecurityOperationCode>, Framework.Attachments.BLL.AttachmentsAttachmentSecurityService>(serviceCollection);
            Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddScoped<Framework.SecuritySystem.IDomainSecurityService<Framework.Attachments.Domain.AttachmentContainer, Framework.Attachments.AttachmentsSecurityOperationCode>, Framework.Attachments.BLL.AttachmentsAttachmentContainerSecurityService>(serviceCollection);
        }
    }
    
    public partial interface IAttachmentsSecurityService : Framework.DomainDriven.BLL.Security.IRootSecurityService<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode>, Framework.Attachments.BLL.IAttachmentsSecurityPathContainer
    {
    }
    
    public partial interface IAttachmentsSecurityPathContainer
    {
    }
    
    public partial class AttachmentsAttachmentSecurityService : Framework.SecuritySystem.NonContextDomainSecurityService<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.Attachment, System.Guid, Framework.Attachments.AttachmentsSecurityOperationCode>
    {
        
        public AttachmentsAttachmentSecurityService(Framework.SecuritySystem.IAccessDeniedExceptionService<Framework.Attachments.Domain.PersistentDomainObjectBase> accessDeniedExceptionService, Framework.SecuritySystem.IDisabledSecurityProviderContainer<Framework.Attachments.Domain.PersistentDomainObjectBase> disabledSecurityProviderContainer, Framework.SecuritySystem.ISecurityOperationResolver<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode> securityOperationResolver, Framework.SecuritySystem.IAuthorizationSystem<System.Guid> authorizationSystem) : 
                base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem)
        {
        }
    }
    
    public partial class AttachmentsAttachmentContainerSecurityService : Framework.SecuritySystem.NonContextDomainSecurityService<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.AttachmentContainer, System.Guid, Framework.Attachments.AttachmentsSecurityOperationCode>
    {
        
        public AttachmentsAttachmentContainerSecurityService(Framework.SecuritySystem.IAccessDeniedExceptionService<Framework.Attachments.Domain.PersistentDomainObjectBase> accessDeniedExceptionService, Framework.SecuritySystem.IDisabledSecurityProviderContainer<Framework.Attachments.Domain.PersistentDomainObjectBase> disabledSecurityProviderContainer, Framework.SecuritySystem.ISecurityOperationResolver<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode> securityOperationResolver, Framework.SecuritySystem.IAuthorizationSystem<System.Guid> authorizationSystem) : 
                base(accessDeniedExceptionService, disabledSecurityProviderContainer, securityOperationResolver, authorizationSystem)
        {
        }
    }
    
    public partial interface IAttachmentsBLLFactoryContainer : Framework.DomainDriven.BLL.IBLLFactoryContainer<Framework.DomainDriven.BLL.Security.IDefaultSecurityBLLFactory<Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.AttachmentsSecurityOperationCode, System.Guid>>
    {
        
        Framework.Attachments.BLL.IAttachmentBLL Attachment
        {
            get;
        }
        
        Framework.Attachments.BLL.IAttachmentContainerBLL AttachmentContainer
        {
            get;
        }
        
        Framework.Attachments.BLL.IAttachmentContainerBLLFactory AttachmentContainerFactory
        {
            get;
        }
        
        Framework.Attachments.BLL.IAttachmentBLLFactory AttachmentFactory
        {
            get;
        }
        
        Framework.Attachments.BLL.IDomainTypeBLL DomainType
        {
            get;
        }
        
        Framework.Attachments.BLL.IDomainTypeBLLFactory DomainTypeFactory
        {
            get;
        }
    }
    
    public partial interface IAttachmentBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.Attachment, System.Guid>
    {
    }
    
    public partial interface IAttachmentBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IAttachmentBLL, Framework.SecuritySystem.ISecurityProvider<Framework.Attachments.Domain.Attachment>>, Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IAttachmentBLL, Framework.Attachments.AttachmentsSecurityOperationCode>, Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IAttachmentBLL, Framework.SecuritySystem.SecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode>>, Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IAttachmentBLL, Framework.SecuritySystem.BLLSecurityMode>
    {
    }
    
    public partial interface IAttachmentContainerBLL : Framework.DomainDriven.BLL.Security.IDefaultSecurityDomainBLLBase<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.AttachmentContainer, System.Guid>
    {
    }
    
    public partial interface IAttachmentContainerBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IAttachmentContainerBLL, Framework.SecuritySystem.ISecurityProvider<Framework.Attachments.Domain.AttachmentContainer>>, Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IAttachmentContainerBLL, Framework.Attachments.AttachmentsSecurityOperationCode>, Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IAttachmentContainerBLL, Framework.SecuritySystem.SecurityOperation<Framework.Attachments.AttachmentsSecurityOperationCode>>, Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IAttachmentContainerBLL, Framework.SecuritySystem.BLLSecurityMode>
    {
    }
    
    public partial interface IDomainTypeBLL : Framework.DomainDriven.BLL.IDefaultDomainBLLBase<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Domain.PersistentDomainObjectBase, Framework.Attachments.Domain.DomainType, System.Guid>
    {
    }
    
    public partial interface IDomainTypeBLLFactory : Framework.DomainDriven.BLL.Security.ISecurityBLLFactory<Framework.Attachments.BLL.IDomainTypeBLL, Framework.SecuritySystem.ISecurityProvider<Framework.Attachments.Domain.DomainType>>
    {
    }
}
