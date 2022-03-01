namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class StateDomainObjectEventController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public StateDomainObjectEventController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check StateDomainObjectEvent access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckStateDomainObjectEventAccess")]
        public virtual void CheckStateDomainObjectEventAccess(CheckStateDomainObjectEventAccessAutoRequest checkStateDomainObjectEventAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkStateDomainObjectEventAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdent = checkStateDomainObjectEventAccessAutoRequest.stateDomainObjectEventIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckStateDomainObjectEventAccessInternal(stateDomainObjectEventIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckStateDomainObjectEventAccessInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEvent;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = bll.GetById(stateDomainObjectEventIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get StateDomainObjectEvent (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateDomainObjectEvent")]
        public virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventFullDTO GetFullStateDomainObjectEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateDomainObjectEventInternal(stateDomainObjectEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get StateDomainObjectEvent (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateDomainObjectEventByName")]
        public virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventFullDTO GetFullStateDomainObjectEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string stateDomainObjectEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateDomainObjectEventByNameInternal(stateDomainObjectEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventFullDTO GetFullStateDomainObjectEventByNameInternal(string stateDomainObjectEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, stateDomainObjectEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventFullDTO GetFullStateDomainObjectEventInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = bll.GetById(stateDomainObjectEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StateDomainObjectEvents (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateDomainObjectEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventFullDTO> GetFullStateDomainObjectEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateDomainObjectEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StateDomainObjectEvents (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateDomainObjectEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventFullDTO> GetFullStateDomainObjectEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO[] stateDomainObjectEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateDomainObjectEventsByIdentsInternal(stateDomainObjectEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventFullDTO> GetFullStateDomainObjectEventsByIdentsInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO[] stateDomainObjectEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(stateDomainObjectEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventFullDTO> GetFullStateDomainObjectEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StateDomainObjectEvent (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichStateDomainObjectEvent")]
        public virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventRichDTO GetRichStateDomainObjectEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichStateDomainObjectEventInternal(stateDomainObjectEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get StateDomainObjectEvent (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichStateDomainObjectEventByName")]
        public virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventRichDTO GetRichStateDomainObjectEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string stateDomainObjectEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichStateDomainObjectEventByNameInternal(stateDomainObjectEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventRichDTO GetRichStateDomainObjectEventByNameInternal(string stateDomainObjectEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, stateDomainObjectEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventRichDTO GetRichStateDomainObjectEventInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = bll.GetById(stateDomainObjectEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StateDomainObjectEvent (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateDomainObjectEvent")]
        public virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventSimpleDTO GetSimpleStateDomainObjectEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateDomainObjectEventInternal(stateDomainObjectEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get StateDomainObjectEvent (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateDomainObjectEventByName")]
        public virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventSimpleDTO GetSimpleStateDomainObjectEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string stateDomainObjectEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateDomainObjectEventByNameInternal(stateDomainObjectEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventSimpleDTO GetSimpleStateDomainObjectEventByNameInternal(string stateDomainObjectEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, stateDomainObjectEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventSimpleDTO GetSimpleStateDomainObjectEventInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = bll.GetById(stateDomainObjectEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StateDomainObjectEvents (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateDomainObjectEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventSimpleDTO> GetSimpleStateDomainObjectEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateDomainObjectEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StateDomainObjectEvents (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateDomainObjectEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventSimpleDTO> GetSimpleStateDomainObjectEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO[] stateDomainObjectEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateDomainObjectEventsByIdentsInternal(stateDomainObjectEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventSimpleDTO> GetSimpleStateDomainObjectEventsByIdentsInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO[] stateDomainObjectEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(stateDomainObjectEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventSimpleDTO> GetSimpleStateDomainObjectEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StateDomainObjectEvent (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualStateDomainObjectEvent")]
        public virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventVisualDTO GetVisualStateDomainObjectEvent([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualStateDomainObjectEventInternal(stateDomainObjectEventIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get StateDomainObjectEvent (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualStateDomainObjectEventByName")]
        public virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventVisualDTO GetVisualStateDomainObjectEventByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string stateDomainObjectEventName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualStateDomainObjectEventByNameInternal(stateDomainObjectEventName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventVisualDTO GetVisualStateDomainObjectEventByNameInternal(string stateDomainObjectEventName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, stateDomainObjectEventName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateDomainObjectEventVisualDTO GetVisualStateDomainObjectEventInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = bll.GetById(stateDomainObjectEventIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StateDomainObjectEvents (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualStateDomainObjectEvents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventVisualDTO> GetVisualStateDomainObjectEvents()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualStateDomainObjectEventsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StateDomainObjectEvents (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualStateDomainObjectEventsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventVisualDTO> GetVisualStateDomainObjectEventsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO[] stateDomainObjectEventIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualStateDomainObjectEventsByIdentsInternal(stateDomainObjectEventIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventVisualDTO> GetVisualStateDomainObjectEventsByIdentsInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO[] stateDomainObjectEventIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(stateDomainObjectEventIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateDomainObjectEventVisualDTO> GetVisualStateDomainObjectEventsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEventFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for StateDomainObjectEvent
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasStateDomainObjectEventAccess")]
        public virtual bool HasStateDomainObjectEventAccess(HasStateDomainObjectEventAccessAutoRequest hasStateDomainObjectEventAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasStateDomainObjectEventAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdent = hasStateDomainObjectEventAccessAutoRequest.stateDomainObjectEventIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasStateDomainObjectEventAccessInternal(stateDomainObjectEventIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasStateDomainObjectEventAccessInternal(Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateDomainObjectEventBLL bll = evaluateData.Context.Logics.StateDomainObjectEvent;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.StateDomainObjectEvent domainObject = bll.GetById(stateDomainObjectEventIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.StateDomainObjectEvent>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckStateDomainObjectEventAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasStateDomainObjectEventAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.StateDomainObjectEventIdentityDTO stateDomainObjectEventIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
