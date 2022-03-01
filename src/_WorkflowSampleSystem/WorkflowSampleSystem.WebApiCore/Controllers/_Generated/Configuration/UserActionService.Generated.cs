namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class UserActionController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext>, Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        public UserActionController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check UserAction access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckUserActionAccess")]
        public virtual void CheckUserActionAccess(CheckUserActionAccessAutoRequest checkUserActionAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkUserActionAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdent = checkUserActionAccessAutoRequest.userActionIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckUserActionAccessInternal(userActionIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckUserActionAccessInternal(Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserAction;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.UserAction domainObject = bll.GetById(userActionIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.UserAction>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Create UserAction by model (UserActionCreateModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CreateUserAction")]
        public virtual Framework.Configuration.Generated.DTO.UserActionRichDTO CreateUserAction([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionCreateModelStrictDTO userActionCreateModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.CreateUserActionInternal(userActionCreateModel, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionRichDTO CreateUserActionInternal(Framework.Configuration.Generated.DTO.UserActionCreateModelStrictDTO userActionCreateModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            Framework.Configuration.Domain.Models.Create.UserActionCreateModel createModel = userActionCreateModel.ToDomainObject(evaluateData.MappingService);
            Framework.Configuration.Domain.UserAction domainObject = bll.Create(createModel);
            bll.CheckAccess(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Configuration.BLL.IConfigurationBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>(session, context, new ConfigurationServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get UserAction (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserAction")]
        public virtual Framework.Configuration.Generated.DTO.UserActionFullDTO GetFullUserAction([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionInternal(userActionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get UserAction (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserActionByName")]
        public virtual Framework.Configuration.Generated.DTO.UserActionFullDTO GetFullUserActionByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string userActionName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionByNameInternal(userActionName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionFullDTO GetFullUserActionByNameInternal(string userActionName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserAction domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, userActionName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionFullDTO GetFullUserActionInternal(Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserAction domainObject = bll.GetById(userActionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of UserActions (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserActions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionFullDTO> GetFullUserActions()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get UserActions (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullUserActionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionFullDTO> GetFullUserActionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionIdentityDTO[] userActionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullUserActionsByIdentsInternal(userActionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionFullDTO> GetFullUserActionsByIdentsInternal(Framework.Configuration.Generated.DTO.UserActionIdentityDTO[] userActionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(userActionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionFullDTO> GetFullUserActionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get UserAction (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichUserAction")]
        public virtual Framework.Configuration.Generated.DTO.UserActionRichDTO GetRichUserAction([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichUserActionInternal(userActionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get UserAction (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichUserActionByName")]
        public virtual Framework.Configuration.Generated.DTO.UserActionRichDTO GetRichUserActionByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string userActionName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichUserActionByNameInternal(userActionName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionRichDTO GetRichUserActionByNameInternal(string userActionName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserAction domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, userActionName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionRichDTO GetRichUserActionInternal(Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserAction domainObject = bll.GetById(userActionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get UserAction (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserAction")]
        public virtual Framework.Configuration.Generated.DTO.UserActionSimpleDTO GetSimpleUserAction([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionInternal(userActionIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get UserAction (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserActionByName")]
        public virtual Framework.Configuration.Generated.DTO.UserActionSimpleDTO GetSimpleUserActionByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string userActionName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionByNameInternal(userActionName, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionSimpleDTO GetSimpleUserActionByNameInternal(string userActionName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserAction domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, userActionName, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.UserActionSimpleDTO GetSimpleUserActionInternal(Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.UserAction domainObject = bll.GetById(userActionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of UserActions (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserActions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionSimpleDTO> GetSimpleUserActions()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get UserActions (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleUserActionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionSimpleDTO> GetSimpleUserActionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.UserActionIdentityDTO[] userActionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleUserActionsByIdentsInternal(userActionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionSimpleDTO> GetSimpleUserActionsByIdentsInternal(Framework.Configuration.Generated.DTO.UserActionIdentityDTO[] userActionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(userActionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.UserActionSimpleDTO> GetSimpleUserActionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserActionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.UserAction>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for UserAction
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasUserActionAccess")]
        public virtual bool HasUserActionAccess(HasUserActionAccessAutoRequest hasUserActionAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasUserActionAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdent = hasUserActionAccessAutoRequest.userActionIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasUserActionAccessInternal(userActionIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasUserActionAccessInternal(Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IUserActionBLL bll = evaluateData.Context.Logics.UserAction;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.UserAction domainObject = bll.GetById(userActionIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.UserAction>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckUserActionAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasUserActionAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.UserActionIdentityDTO userActionIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
