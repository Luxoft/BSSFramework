namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class UserActionObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext>, Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        public UserActionObjectController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check UserActionObject access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckUserActionObjectAccess")]
        public virtual void CheckUserActionObjectAccess(CheckUserActionObjectAccessAutoRequest checkUserActionObjectAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkUserActionObjectAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdent = checkUserActionObjectAccessAutoRequest.userActionObjectIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckUserActionObjectAccessInternal(userActionObjectIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckUserActionObjectAccessInternal(Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObject;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.UserActionObject domainObject = bll.GetById(userActionObjectIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.UserActionObject>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Configuration.BLL.IConfigurationBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>(session, context, new ConfigurationServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get UserActionObject (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserActionObject")]
        public virtual Framework.Configuration.Generated.DTO.UserActionObjectFullDTO GetFullUserActionObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionObjectInternal(userActionObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get UserActionObject (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserActionObjectByName")]
        public virtual Framework.Configuration.Generated.DTO.UserActionObjectFullDTO GetFullUserActionObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string userActionObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionObjectByNameInternal(userActionObjectName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionObjectFullDTO GetFullUserActionObjectByNameInternal(string userActionObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserActionObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, userActionObjectName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionObjectFullDTO GetFullUserActionObjectInternal(Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserActionObject domainObject = bll.GetById(userActionObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of UserActionObjects (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserActionObjects")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> GetFullUserActionObjects()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get UserActionObjects (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserActionObjectsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> GetFullUserActionObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO[] userActionObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionObjectsByIdentsInternal(userActionObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> GetFullUserActionObjectsByIdentsInternal(Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO[] userActionObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(userActionObjectIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get UserActionObjects (FullDTO) by filter (Framework.Configuration.Domain.Models.Filters.UserActionObjectRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserActionObjectsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> GetFullUserActionObjectsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionObjectRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionObjectsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> GetFullUserActionObjectsByRootFilterInternal(Framework.Configuration.Generated.DTO.UserActionObjectRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Models.Filters.UserActionObjectRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectFullDTO> GetFullUserActionObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get UserActionObject (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichUserActionObject")]
        public virtual Framework.Configuration.Generated.DTO.UserActionObjectRichDTO GetRichUserActionObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichUserActionObjectInternal(userActionObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get UserActionObject (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichUserActionObjectByName")]
        public virtual Framework.Configuration.Generated.DTO.UserActionObjectRichDTO GetRichUserActionObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string userActionObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichUserActionObjectByNameInternal(userActionObjectName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionObjectRichDTO GetRichUserActionObjectByNameInternal(string userActionObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserActionObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, userActionObjectName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionObjectRichDTO GetRichUserActionObjectInternal(Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserActionObject domainObject = bll.GetById(userActionObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get UserActionObject (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserActionObject")]
        public virtual Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO GetSimpleUserActionObject([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionObjectInternal(userActionObjectIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get UserActionObject (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserActionObjectByName")]
        public virtual Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO GetSimpleUserActionObjectByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string userActionObjectName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionObjectByNameInternal(userActionObjectName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO GetSimpleUserActionObjectByNameInternal(string userActionObjectName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserActionObject domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, userActionObjectName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO GetSimpleUserActionObjectInternal(Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserActionObject domainObject = bll.GetById(userActionObjectIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of UserActionObjects (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserActionObjects")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO> GetSimpleUserActionObjects()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionObjectsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get UserActionObjects (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserActionObjectsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO> GetSimpleUserActionObjectsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO[] userActionObjectIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionObjectsByIdentsInternal(userActionObjectIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO> GetSimpleUserActionObjectsByIdentsInternal(Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO[] userActionObjectIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(userActionObjectIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get UserActionObjects (SimpleDTO) by filter (Framework.Configuration.Domain.Models.Filters.UserActionObjectRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserActionObjectsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO> GetSimpleUserActionObjectsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionObjectRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionObjectsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO> GetSimpleUserActionObjectsByRootFilterInternal(Framework.Configuration.Generated.DTO.UserActionObjectRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.Models.Filters.UserActionObjectRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionObjectSimpleDTO> GetSimpleUserActionObjectsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserActionObject>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for UserActionObject
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasUserActionObjectAccess")]
        public virtual bool HasUserActionObjectAccess(HasUserActionObjectAccessAutoRequest hasUserActionObjectAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasUserActionObjectAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdent = hasUserActionObjectAccessAutoRequest.userActionObjectIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasUserActionObjectAccessInternal(userActionObjectIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasUserActionObjectAccessInternal(Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionObjectBLL bll = evaluateData.Context.Logics.UserActionObject;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.UserActionObject domainObject = bll.GetById(userActionObjectIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.UserActionObject>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckUserActionObjectAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasUserActionObjectAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.UserActionObjectIdentityDTO userActionObjectIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
