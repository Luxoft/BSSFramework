namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class ExceptionMessageController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        /// <summary>
        /// Check ExceptionMessage access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckExceptionMessageAccess")]
        public virtual void CheckExceptionMessageAccess(CheckExceptionMessageAccessAutoRequest checkExceptionMessageAccessAutoRequest)
        {
            string securityOperationName = checkExceptionMessageAccessAutoRequest.securityOperationName;
            Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdent = checkExceptionMessageAccessAutoRequest.exceptionMessageIdent;
            this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.CheckExceptionMessageAccessInternal(exceptionMessageIdent, securityOperationName, evaluateData));
        }
        
        protected virtual void CheckExceptionMessageAccessInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessage;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(Framework.Configuration.ConfigurationSecurityOperation), securityOperationName);
            Framework.Configuration.Domain.ExceptionMessage domainObject = bll.GetById(exceptionMessageIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderBaseExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.ExceptionMessage>(operation), domainObject, evaluateData.Context.AccessDeniedExceptionService);
        }
        
        /// <summary>
        /// Get ExceptionMessage (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullExceptionMessage")]
        public virtual Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO GetFullExceptionMessage([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExceptionMessageInternal(exceptionMessageIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO GetFullExceptionMessageInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.ExceptionMessage domainObject = bll.GetById(exceptionMessageIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ExceptionMessages (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullExceptionMessages")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessages()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExceptionMessagesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ExceptionMessages (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullExceptionMessagesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO[] exceptionMessageIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExceptionMessagesByIdentsInternal(exceptionMessageIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesByIdentsInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO[] exceptionMessageIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(exceptionMessageIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ExceptionMessages (FullDTO) by filter (Framework.Configuration.Domain.ExceptionMessageRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullExceptionMessagesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExceptionMessagesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesByRootFilterInternal(Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.ExceptionMessageRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageFullDTO> GetFullExceptionMessagesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ExceptionMessage (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichExceptionMessage")]
        public virtual Framework.Configuration.Generated.DTO.ExceptionMessageRichDTO GetRichExceptionMessage([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichExceptionMessageInternal(exceptionMessageIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ExceptionMessageRichDTO GetRichExceptionMessageInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.ExceptionMessage domainObject = bll.GetById(exceptionMessageIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ExceptionMessage (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleExceptionMessage")]
        public virtual Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO GetSimpleExceptionMessage([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExceptionMessageInternal(exceptionMessageIdentity, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO GetSimpleExceptionMessageInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.ExceptionMessage domainObject = bll.GetById(exceptionMessageIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ExceptionMessages (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleExceptionMessages")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessages()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExceptionMessagesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ExceptionMessages (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleExceptionMessagesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO[] exceptionMessageIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExceptionMessagesByIdentsInternal(exceptionMessageIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesByIdentsInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO[] exceptionMessageIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(exceptionMessageIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ExceptionMessages (SimpleDTO) by filter (Framework.Configuration.Domain.ExceptionMessageRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleExceptionMessagesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExceptionMessagesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesByRootFilterInternal(Framework.Configuration.Generated.DTO.ExceptionMessageRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.ExceptionMessageRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.ExceptionMessageSimpleDTO> GetSimpleExceptionMessagesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessageFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.ExceptionMessage>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ExceptionMessage
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasExceptionMessageAccess")]
        public virtual bool HasExceptionMessageAccess(HasExceptionMessageAccessAutoRequest hasExceptionMessageAccessAutoRequest)
        {
            string securityOperationName = hasExceptionMessageAccessAutoRequest.securityOperationName;
            Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdent = hasExceptionMessageAccessAutoRequest.exceptionMessageIdent;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.HasExceptionMessageAccessInternal(exceptionMessageIdent, securityOperationName, evaluateData));
        }
        
        protected virtual bool HasExceptionMessageAccessInternal(Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdent, string securityOperationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IExceptionMessageBLL bll = evaluateData.Context.Logics.ExceptionMessage;
            Framework.SecuritySystem.SecurityOperation operation = Framework.Security.SecurityOperationHelper.Parse(typeof(Framework.Configuration.ConfigurationSecurityOperation), securityOperationName);
            Framework.Configuration.Domain.ExceptionMessage domainObject = bll.GetById(exceptionMessageIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.ExceptionMessage>(operation).HasAccess(domainObject);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO SaveExceptionMessageInternal(Framework.Configuration.Generated.DTO.ExceptionMessageStrictDTO exceptionMessageStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.IExceptionMessageBLL bll)
        {
            Framework.Configuration.Domain.ExceptionMessage domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, exceptionMessageStrict.Id);
            exceptionMessageStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckExceptionMessageAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasExceptionMessageAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.ExceptionMessageIdentityDTO exceptionMessageIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string securityOperationName;
    }
}
