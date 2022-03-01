namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class ConditionStateEventController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public ConditionStateEventController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check ConditionStateEvent access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckConditionStateEventAccess")]
        public virtual void CheckConditionStateEventAccess(CheckConditionStateEventAccessAutoRequest checkConditionStateEventAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkConditionStateEventAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdent = checkConditionStateEventAccessAutoRequest.conditionStateEventIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckConditionStateEventAccessInternal(conditionStateEventIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckConditionStateEventAccessInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEvent;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = bll.GetById(conditionStateEventIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.ConditionStateEvent>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get ConditionStateEvent (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionStateEvent")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateEventFullDTO GetFullConditionStateEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStateEventInternal(conditionStateEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStateEvent (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionStateEventByName")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateEventFullDTO GetFullConditionStateEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string conditionStateEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStateEventByNameInternal(conditionStateEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateEventFullDTO GetFullConditionStateEventByNameInternal(string conditionStateEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, conditionStateEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateEventFullDTO GetFullConditionStateEventInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = bll.GetById(conditionStateEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ConditionStateEvents (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionStateEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventFullDTO> GetFullConditionStateEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStateEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStateEvents (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionStateEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventFullDTO> GetFullConditionStateEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO[] conditionStateEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStateEventsByIdentsInternal(conditionStateEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventFullDTO> GetFullConditionStateEventsByIdentsInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO[] conditionStateEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(conditionStateEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventFullDTO> GetFullConditionStateEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionStateEvent (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichConditionStateEvent")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateEventRichDTO GetRichConditionStateEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichConditionStateEventInternal(conditionStateEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStateEvent (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichConditionStateEventByName")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateEventRichDTO GetRichConditionStateEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string conditionStateEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichConditionStateEventByNameInternal(conditionStateEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateEventRichDTO GetRichConditionStateEventByNameInternal(string conditionStateEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, conditionStateEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateEventRichDTO GetRichConditionStateEventInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = bll.GetById(conditionStateEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionStateEvent (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionStateEvent")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateEventSimpleDTO GetSimpleConditionStateEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStateEventInternal(conditionStateEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStateEvent (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionStateEventByName")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateEventSimpleDTO GetSimpleConditionStateEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string conditionStateEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStateEventByNameInternal(conditionStateEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateEventSimpleDTO GetSimpleConditionStateEventByNameInternal(string conditionStateEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, conditionStateEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateEventSimpleDTO GetSimpleConditionStateEventInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = bll.GetById(conditionStateEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ConditionStateEvents (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionStateEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventSimpleDTO> GetSimpleConditionStateEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStateEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStateEvents (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionStateEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventSimpleDTO> GetSimpleConditionStateEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO[] conditionStateEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStateEventsByIdentsInternal(conditionStateEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventSimpleDTO> GetSimpleConditionStateEventsByIdentsInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO[] conditionStateEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(conditionStateEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventSimpleDTO> GetSimpleConditionStateEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionStateEvent (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionStateEvent")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateEventVisualDTO GetVisualConditionStateEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStateEventInternal(conditionStateEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStateEvent (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionStateEventByName")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateEventVisualDTO GetVisualConditionStateEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string conditionStateEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStateEventByNameInternal(conditionStateEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateEventVisualDTO GetVisualConditionStateEventByNameInternal(string conditionStateEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, conditionStateEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateEventVisualDTO GetVisualConditionStateEventInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = bll.GetById(conditionStateEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ConditionStateEvents (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionStateEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventVisualDTO> GetVisualConditionStateEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStateEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStateEvents (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionStateEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventVisualDTO> GetVisualConditionStateEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO[] conditionStateEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStateEventsByIdentsInternal(conditionStateEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventVisualDTO> GetVisualConditionStateEventsByIdentsInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO[] conditionStateEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(conditionStateEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateEventVisualDTO> GetVisualConditionStateEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionStateEvent>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ConditionStateEvent
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasConditionStateEventAccess")]
        public virtual bool HasConditionStateEventAccess(HasConditionStateEventAccessAutoRequest hasConditionStateEventAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasConditionStateEventAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdent = hasConditionStateEventAccessAutoRequest.conditionStateEventIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasConditionStateEventAccessInternal(conditionStateEventIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasConditionStateEventAccessInternal(Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateEventBLL bll = evaluateData.Context.Logics.ConditionStateEvent;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.ConditionStateEvent domainObject = bll.GetById(conditionStateEventIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.ConditionStateEvent>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckConditionStateEventAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasConditionStateEventAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.ConditionStateEventIdentityDTO conditionStateEventIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
