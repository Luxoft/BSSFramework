namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class StateTimeoutEventController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public StateTimeoutEventController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check StateTimeoutEvent access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckStateTimeoutEventAccess")]
        public virtual void CheckStateTimeoutEventAccess(CheckStateTimeoutEventAccessAutoRequest checkStateTimeoutEventAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkStateTimeoutEventAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdent = checkStateTimeoutEventAccessAutoRequest.stateTimeoutEventIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckStateTimeoutEventAccessInternal(stateTimeoutEventIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckStateTimeoutEventAccessInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEvent;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = bll.GetById(stateTimeoutEventIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get StateTimeoutEvent (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateTimeoutEvent")]
        public virtual Framework.Workflow.Generated.DTO.StateTimeoutEventFullDTO GetFullStateTimeoutEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateTimeoutEventInternal(stateTimeoutEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get StateTimeoutEvent (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateTimeoutEventByName")]
        public virtual Framework.Workflow.Generated.DTO.StateTimeoutEventFullDTO GetFullStateTimeoutEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string stateTimeoutEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateTimeoutEventByNameInternal(stateTimeoutEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateTimeoutEventFullDTO GetFullStateTimeoutEventByNameInternal(string stateTimeoutEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, stateTimeoutEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateTimeoutEventFullDTO GetFullStateTimeoutEventInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = bll.GetById(stateTimeoutEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StateTimeoutEvents (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateTimeoutEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventFullDTO> GetFullStateTimeoutEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateTimeoutEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StateTimeoutEvents (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateTimeoutEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventFullDTO> GetFullStateTimeoutEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO[] stateTimeoutEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateTimeoutEventsByIdentsInternal(stateTimeoutEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventFullDTO> GetFullStateTimeoutEventsByIdentsInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO[] stateTimeoutEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(stateTimeoutEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventFullDTO> GetFullStateTimeoutEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StateTimeoutEvent (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichStateTimeoutEvent")]
        public virtual Framework.Workflow.Generated.DTO.StateTimeoutEventRichDTO GetRichStateTimeoutEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichStateTimeoutEventInternal(stateTimeoutEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get StateTimeoutEvent (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichStateTimeoutEventByName")]
        public virtual Framework.Workflow.Generated.DTO.StateTimeoutEventRichDTO GetRichStateTimeoutEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string stateTimeoutEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichStateTimeoutEventByNameInternal(stateTimeoutEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateTimeoutEventRichDTO GetRichStateTimeoutEventByNameInternal(string stateTimeoutEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, stateTimeoutEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateTimeoutEventRichDTO GetRichStateTimeoutEventInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = bll.GetById(stateTimeoutEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StateTimeoutEvent (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateTimeoutEvent")]
        public virtual Framework.Workflow.Generated.DTO.StateTimeoutEventSimpleDTO GetSimpleStateTimeoutEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateTimeoutEventInternal(stateTimeoutEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get StateTimeoutEvent (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateTimeoutEventByName")]
        public virtual Framework.Workflow.Generated.DTO.StateTimeoutEventSimpleDTO GetSimpleStateTimeoutEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string stateTimeoutEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateTimeoutEventByNameInternal(stateTimeoutEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateTimeoutEventSimpleDTO GetSimpleStateTimeoutEventByNameInternal(string stateTimeoutEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, stateTimeoutEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateTimeoutEventSimpleDTO GetSimpleStateTimeoutEventInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = bll.GetById(stateTimeoutEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StateTimeoutEvents (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateTimeoutEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventSimpleDTO> GetSimpleStateTimeoutEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateTimeoutEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StateTimeoutEvents (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateTimeoutEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventSimpleDTO> GetSimpleStateTimeoutEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO[] stateTimeoutEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateTimeoutEventsByIdentsInternal(stateTimeoutEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventSimpleDTO> GetSimpleStateTimeoutEventsByIdentsInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO[] stateTimeoutEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(stateTimeoutEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventSimpleDTO> GetSimpleStateTimeoutEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StateTimeoutEvent (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualStateTimeoutEvent")]
        public virtual Framework.Workflow.Generated.DTO.StateTimeoutEventVisualDTO GetVisualStateTimeoutEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualStateTimeoutEventInternal(stateTimeoutEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get StateTimeoutEvent (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualStateTimeoutEventByName")]
        public virtual Framework.Workflow.Generated.DTO.StateTimeoutEventVisualDTO GetVisualStateTimeoutEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string stateTimeoutEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualStateTimeoutEventByNameInternal(stateTimeoutEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateTimeoutEventVisualDTO GetVisualStateTimeoutEventByNameInternal(string stateTimeoutEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, stateTimeoutEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateTimeoutEventVisualDTO GetVisualStateTimeoutEventInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = bll.GetById(stateTimeoutEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StateTimeoutEvents (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualStateTimeoutEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventVisualDTO> GetVisualStateTimeoutEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualStateTimeoutEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StateTimeoutEvents (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualStateTimeoutEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventVisualDTO> GetVisualStateTimeoutEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO[] stateTimeoutEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualStateTimeoutEventsByIdentsInternal(stateTimeoutEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventVisualDTO> GetVisualStateTimeoutEventsByIdentsInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO[] stateTimeoutEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(stateTimeoutEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateTimeoutEventVisualDTO> GetVisualStateTimeoutEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for StateTimeoutEvent
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasStateTimeoutEventAccess")]
        public virtual bool HasStateTimeoutEventAccess(HasStateTimeoutEventAccessAutoRequest hasStateTimeoutEventAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasStateTimeoutEventAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdent = hasStateTimeoutEventAccessAutoRequest.stateTimeoutEventIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasStateTimeoutEventAccessInternal(stateTimeoutEventIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasStateTimeoutEventAccessInternal(Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateTimeoutEventBLL bll = evaluateData.Context.Logics.StateTimeoutEvent;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.StateTimeoutEvent domainObject = bll.GetById(stateTimeoutEventIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.StateTimeoutEvent>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckStateTimeoutEventAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasStateTimeoutEventAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.StateTimeoutEventIdentityDTO stateTimeoutEventIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
