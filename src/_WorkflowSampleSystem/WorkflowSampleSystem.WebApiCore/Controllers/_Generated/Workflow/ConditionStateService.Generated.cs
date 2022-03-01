namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class ConditionStateController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public ConditionStateController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check ConditionState access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckConditionStateAccess")]
        public virtual void CheckConditionStateAccess(CheckConditionStateAccessAutoRequest checkConditionStateAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkConditionStateAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent = checkConditionStateAccessAutoRequest.conditionStateIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckConditionStateAccessInternal(conditionStateIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckConditionStateAccessInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionState;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = bll.GetById(conditionStateIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.ConditionState>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get ConditionState (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionState")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateFullDTO GetFullConditionState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStateInternal(conditionStateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ConditionState (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionStateByName")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateFullDTO GetFullConditionStateByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string conditionStateName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStateByNameInternal(conditionStateName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateFullDTO GetFullConditionStateByNameInternal(string conditionStateName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, conditionStateName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateFullDTO GetFullConditionStateInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = bll.GetById(conditionStateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ConditionStates (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionStates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateFullDTO> GetFullConditionStates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStates (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionStatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateFullDTO> GetFullConditionStatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO[] conditionStateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStatesByIdentsInternal(conditionStateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateFullDTO> GetFullConditionStatesByIdentsInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO[] conditionStateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(conditionStateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionStates (FullDTO) by filter (Framework.Workflow.Domain.ConditionStateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullConditionStatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateFullDTO> GetFullConditionStatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullConditionStatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateFullDTO> GetFullConditionStatesByRootFilterInternal(Framework.Workflow.Generated.DTO.ConditionStateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.ConditionStateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateFullDTO> GetFullConditionStatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionState (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichConditionState")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateRichDTO GetRichConditionState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichConditionStateInternal(conditionStateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ConditionState (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichConditionStateByName")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateRichDTO GetRichConditionStateByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string conditionStateName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichConditionStateByNameInternal(conditionStateName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateRichDTO GetRichConditionStateByNameInternal(string conditionStateName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, conditionStateName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateRichDTO GetRichConditionStateInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = bll.GetById(conditionStateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionState (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionState")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO GetSimpleConditionState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStateInternal(conditionStateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ConditionState (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionStateByName")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO GetSimpleConditionStateByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string conditionStateName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStateByNameInternal(conditionStateName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO GetSimpleConditionStateByNameInternal(string conditionStateName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, conditionStateName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO GetSimpleConditionStateInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = bll.GetById(conditionStateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ConditionStates (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionStates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO> GetSimpleConditionStates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStates (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionStatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO> GetSimpleConditionStatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO[] conditionStateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStatesByIdentsInternal(conditionStateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO> GetSimpleConditionStatesByIdentsInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO[] conditionStateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(conditionStateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionStates (SimpleDTO) by filter (Framework.Workflow.Domain.ConditionStateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleConditionStatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO> GetSimpleConditionStatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleConditionStatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO> GetSimpleConditionStatesByRootFilterInternal(Framework.Workflow.Generated.DTO.ConditionStateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.ConditionStateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateSimpleDTO> GetSimpleConditionStatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionState (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionState")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateVisualDTO GetVisualConditionState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStateInternal(conditionStateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ConditionState (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionStateByName")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateVisualDTO GetVisualConditionStateByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string conditionStateName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStateByNameInternal(conditionStateName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateVisualDTO GetVisualConditionStateByNameInternal(string conditionStateName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, conditionStateName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateVisualDTO GetVisualConditionStateInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = bll.GetById(conditionStateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ConditionStates (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionStates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateVisualDTO> GetVisualConditionStates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ConditionStates (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionStatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateVisualDTO> GetVisualConditionStatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO[] conditionStateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStatesByIdentsInternal(conditionStateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateVisualDTO> GetVisualConditionStatesByIdentsInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO[] conditionStateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(conditionStateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ConditionStates (VisualDTO) by filter (Framework.Workflow.Domain.ConditionStateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualConditionStatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateVisualDTO> GetVisualConditionStatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualConditionStatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateVisualDTO> GetVisualConditionStatesByRootFilterInternal(Framework.Workflow.Generated.DTO.ConditionStateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.ConditionStateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ConditionStateVisualDTO> GetVisualConditionStatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ConditionState>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ConditionState
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasConditionStateAccess")]
        public virtual bool HasConditionStateAccess(HasConditionStateAccessAutoRequest hasConditionStateAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasConditionStateAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent = hasConditionStateAccessAutoRequest.conditionStateIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasConditionStateAccessInternal(conditionStateIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasConditionStateAccessInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionState;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.ConditionState domainObject = bll.GetById(conditionStateIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.ConditionState>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove ConditionState
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveConditionState")]
        public virtual void RemoveConditionState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveConditionStateInternal(conditionStateIdent, evaluateData));
        }
        
        protected virtual void RemoveConditionStateInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveConditionStateInternal(conditionStateIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveConditionStateInternal(Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.IConditionStateBLL bll)
        {
            Framework.Workflow.Domain.Definition.ConditionState domainObject = bll.GetById(conditionStateIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save ConditionStates
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveConditionState")]
        public virtual Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO SaveConditionState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ConditionStateStrictDTO conditionStateStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveConditionStateInternal(conditionStateStrict, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO SaveConditionStateInternal(Framework.Workflow.Generated.DTO.ConditionStateStrictDTO conditionStateStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IConditionStateBLL bll = evaluateData.Context.Logics.ConditionStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveConditionStateInternal(conditionStateStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO SaveConditionStateInternal(Framework.Workflow.Generated.DTO.ConditionStateStrictDTO conditionStateStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.IConditionStateBLL bll)
        {
            Framework.Workflow.Domain.Definition.ConditionState domainObject = bll.GetById(conditionStateStrict.Id, true);
            conditionStateStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckConditionStateAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasConditionStateAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.ConditionStateIdentityDTO conditionStateIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
