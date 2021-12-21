namespace Configuration.WebApi.Controllers
{
    using Framework.Configuration.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("configApi/v{version:apiVersion}/[controller]")]
    public partial class MessageTemplateController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext>, Framework.Configuration.BLL.IConfigurationBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>>
    {
        
        public MessageTemplateController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Configuration.BLL.IConfigurationBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check MessageTemplate access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckMessageTemplateAccess")]
        public virtual void CheckMessageTemplateAccess(CheckMessageTemplateAccessAutoRequest checkMessageTemplateAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = checkMessageTemplateAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent = checkMessageTemplateAccessAutoRequest.messageTemplateIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckMessageTemplateAccessInternal(messageTemplateIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckMessageTemplateAccessInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplate;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.MessageTemplate domainObject = bll.GetById(messageTemplateIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.MessageTemplate>(securityOperationCode), domainObject);
        }
        
        /// <summary>
        /// Create MessageTemplate by model (MessageTemplateCreateModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CreateMessageTemplate")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateRichDTO CreateMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateCreateModelStrictDTO messageTemplateCreateModel)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CreateMessageTemplateInternal(messageTemplateCreateModel, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateRichDTO CreateMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateCreateModelStrictDTO messageTemplateCreateModel, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            Framework.Configuration.Domain.MessageTemplateCreateModel createModel = messageTemplateCreateModel.ToDomainObject(evaluateData.MappingService);
            Framework.Configuration.Domain.MessageTemplate domainObject = bll.Create(createModel);
            bll.CheckAccess(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Configuration.BLL.IConfigurationBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService>(session, context, new ConfigurationServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get MessageTemplate (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplate")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateFullDTO GetFullMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplateInternal(messageTemplateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplate (FullDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplateByCode")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateFullDTO GetFullMessageTemplateByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string messageTemplateCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplateByCodeInternal(messageTemplateCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateFullDTO GetFullMessageTemplateByCodeInternal(string messageTemplateCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplate domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, messageTemplateCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateFullDTO GetFullMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplate domainObject = bll.GetById(messageTemplateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of MessageTemplates (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateFullDTO> GetFullMessageTemplates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplates (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateFullDTO> GetFullMessageTemplatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO[] messageTemplateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplatesByIdentsInternal(messageTemplateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateFullDTO> GetFullMessageTemplatesByIdentsInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO[] messageTemplateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(messageTemplateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplates (FullDTO) by filter (Framework.Configuration.Domain.MessageTemplateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullMessageTemplatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateFullDTO> GetFullMessageTemplatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullMessageTemplatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateFullDTO> GetFullMessageTemplatesByRootFilterInternal(Framework.Configuration.Generated.DTO.MessageTemplateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateFullDTO> GetFullMessageTemplatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplate (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichMessageTemplate")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateRichDTO GetRichMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichMessageTemplateInternal(messageTemplateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplate (RichDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichMessageTemplateByCode")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateRichDTO GetRichMessageTemplateByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string messageTemplateCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichMessageTemplateByCodeInternal(messageTemplateCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateRichDTO GetRichMessageTemplateByCodeInternal(string messageTemplateCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplate domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, messageTemplateCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateRichDTO GetRichMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplate domainObject = bll.GetById(messageTemplateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplate (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplate")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO GetSimpleMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplateInternal(messageTemplateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplate (SimpleDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplateByCode")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO GetSimpleMessageTemplateByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string messageTemplateCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplateByCodeInternal(messageTemplateCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO GetSimpleMessageTemplateByCodeInternal(string messageTemplateCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplate domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, messageTemplateCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO GetSimpleMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplate domainObject = bll.GetById(messageTemplateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of MessageTemplates (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO> GetSimpleMessageTemplates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplates (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO> GetSimpleMessageTemplatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO[] messageTemplateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplatesByIdentsInternal(messageTemplateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO> GetSimpleMessageTemplatesByIdentsInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO[] messageTemplateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(messageTemplateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplates (SimpleDTO) by filter (Framework.Configuration.Domain.MessageTemplateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleMessageTemplatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO> GetSimpleMessageTemplatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleMessageTemplatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO> GetSimpleMessageTemplatesByRootFilterInternal(Framework.Configuration.Generated.DTO.MessageTemplateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateSimpleDTO> GetSimpleMessageTemplatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplate (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualMessageTemplate")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO GetVisualMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualMessageTemplateInternal(messageTemplateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplate (VisualDTO) by code
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualMessageTemplateByCode")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO GetVisualMessageTemplateByCode([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string messageTemplateCode)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualMessageTemplateByCodeInternal(messageTemplateCode, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO GetVisualMessageTemplateByCodeInternal(string messageTemplateCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplate domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByCode(bll, messageTemplateCode, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO GetVisualMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplate domainObject = bll.GetById(messageTemplateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of MessageTemplates (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualMessageTemplates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO> GetVisualMessageTemplates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualMessageTemplatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get MessageTemplates (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualMessageTemplatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO> GetVisualMessageTemplatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO[] messageTemplateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualMessageTemplatesByIdentsInternal(messageTemplateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO> GetVisualMessageTemplatesByIdentsInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO[] messageTemplateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(messageTemplateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get MessageTemplates (VisualDTO) by filter (Framework.Configuration.Domain.MessageTemplateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualMessageTemplatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO> GetVisualMessageTemplatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualMessageTemplatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO> GetVisualMessageTemplatesByRootFilterInternal(Framework.Configuration.Generated.DTO.MessageTemplateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Configuration.Domain.MessageTemplateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Configuration.Generated.DTO.MessageTemplateVisualDTO> GetVisualMessageTemplatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Configuration.Domain.MessageTemplate>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for MessageTemplate
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasMessageTemplateAccess")]
        public virtual bool HasMessageTemplateAccess(HasMessageTemplateAccessAutoRequest hasMessageTemplateAccessAutoRequest)
        {
            Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode = hasMessageTemplateAccessAutoRequest.securityOperationCode;
            Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent = hasMessageTemplateAccessAutoRequest.messageTemplateIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasMessageTemplateAccessInternal(messageTemplateIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasMessageTemplateAccessInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent, Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplate;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Configuration.Domain.MessageTemplate domainObject = bll.GetById(messageTemplateIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Configuration.Domain.MessageTemplate>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove MessageTemplate
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveMessageTemplate")]
        public virtual void RemoveMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveMessageTemplateInternal(messageTemplateIdent, evaluateData));
        }
        
        protected virtual void RemoveMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveMessageTemplateInternal(messageTemplateIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.IMessageTemplateBLL bll)
        {
            Framework.Configuration.Domain.MessageTemplate domainObject = bll.GetById(messageTemplateIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save MessageTemplates
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveMessageTemplate")]
        public virtual Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO SaveMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Configuration.Generated.DTO.MessageTemplateStrictDTO messageTemplateStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveMessageTemplateInternal(messageTemplateStrict, evaluateData));
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO SaveMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateStrictDTO messageTemplateStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData)
        {
            Framework.Configuration.BLL.IMessageTemplateBLL bll = evaluateData.Context.Logics.MessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveMessageTemplateInternal(messageTemplateStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO SaveMessageTemplateInternal(Framework.Configuration.Generated.DTO.MessageTemplateStrictDTO messageTemplateStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Configuration.BLL.IConfigurationBLLContext, Framework.Configuration.Generated.DTO.IConfigurationDTOMappingService> evaluateData, Framework.Configuration.BLL.IMessageTemplateBLL bll)
        {
            Framework.Configuration.Domain.MessageTemplate domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByIdOrCreate(bll, messageTemplateStrict.Id);
            messageTemplateStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Configuration.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckMessageTemplateAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasMessageTemplateAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Configuration.Generated.DTO.MessageTemplateIdentityDTO messageTemplateIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Configuration.ConfigurationSecurityOperationCode securityOperationCode;
    }
}
