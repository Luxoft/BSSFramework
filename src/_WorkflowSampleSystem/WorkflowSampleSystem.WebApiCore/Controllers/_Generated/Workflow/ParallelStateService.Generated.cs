namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class ParallelStateController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public ParallelStateController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check ParallelState access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckParallelStateAccess")]
        public virtual void CheckParallelStateAccess(CheckParallelStateAccessAutoRequest checkParallelStateAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkParallelStateAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent = checkParallelStateAccessAutoRequest.parallelStateIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckParallelStateAccessInternal(parallelStateIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckParallelStateAccessInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelState;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = bll.GetById(parallelStateIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.ParallelState>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get ParallelState (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullParallelState")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateFullDTO GetFullParallelState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullParallelStateInternal(parallelStateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ParallelState (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullParallelStateByName")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateFullDTO GetFullParallelStateByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string parallelStateName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullParallelStateByNameInternal(parallelStateName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateFullDTO GetFullParallelStateByNameInternal(string parallelStateName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, parallelStateName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateFullDTO GetFullParallelStateInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = bll.GetById(parallelStateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ParallelStates (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullParallelStates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateFullDTO> GetFullParallelStates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullParallelStatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ParallelStates (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullParallelStatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateFullDTO> GetFullParallelStatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO[] parallelStateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullParallelStatesByIdentsInternal(parallelStateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateFullDTO> GetFullParallelStatesByIdentsInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO[] parallelStateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(parallelStateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ParallelStates (FullDTO) by filter (Framework.Workflow.Domain.ParallelStateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullParallelStatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateFullDTO> GetFullParallelStatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullParallelStatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateFullDTO> GetFullParallelStatesByRootFilterInternal(Framework.Workflow.Generated.DTO.ParallelStateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.ParallelStateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateFullDTO> GetFullParallelStatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ParallelState (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichParallelState")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateRichDTO GetRichParallelState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichParallelStateInternal(parallelStateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ParallelState (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichParallelStateByName")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateRichDTO GetRichParallelStateByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string parallelStateName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichParallelStateByNameInternal(parallelStateName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateRichDTO GetRichParallelStateByNameInternal(string parallelStateName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, parallelStateName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateRichDTO GetRichParallelStateInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = bll.GetById(parallelStateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ParallelState (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleParallelState")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO GetSimpleParallelState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleParallelStateInternal(parallelStateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ParallelState (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleParallelStateByName")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO GetSimpleParallelStateByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string parallelStateName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleParallelStateByNameInternal(parallelStateName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO GetSimpleParallelStateByNameInternal(string parallelStateName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, parallelStateName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO GetSimpleParallelStateInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = bll.GetById(parallelStateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ParallelStates (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleParallelStates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO> GetSimpleParallelStates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleParallelStatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ParallelStates (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleParallelStatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO> GetSimpleParallelStatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO[] parallelStateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleParallelStatesByIdentsInternal(parallelStateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO> GetSimpleParallelStatesByIdentsInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO[] parallelStateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(parallelStateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ParallelStates (SimpleDTO) by filter (Framework.Workflow.Domain.ParallelStateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleParallelStatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO> GetSimpleParallelStatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleParallelStatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO> GetSimpleParallelStatesByRootFilterInternal(Framework.Workflow.Generated.DTO.ParallelStateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.ParallelStateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateSimpleDTO> GetSimpleParallelStatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ParallelState (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualParallelState")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateVisualDTO GetVisualParallelState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualParallelStateInternal(parallelStateIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get ParallelState (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualParallelStateByName")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateVisualDTO GetVisualParallelStateByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string parallelStateName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualParallelStateByNameInternal(parallelStateName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateVisualDTO GetVisualParallelStateByNameInternal(string parallelStateName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, parallelStateName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateVisualDTO GetVisualParallelStateInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = bll.GetById(parallelStateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of ParallelStates (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualParallelStates")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateVisualDTO> GetVisualParallelStates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualParallelStatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get ParallelStates (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualParallelStatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateVisualDTO> GetVisualParallelStatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO[] parallelStateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualParallelStatesByIdentsInternal(parallelStateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateVisualDTO> GetVisualParallelStatesByIdentsInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO[] parallelStateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(parallelStateIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ParallelStates (VisualDTO) by filter (Framework.Workflow.Domain.ParallelStateRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualParallelStatesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateVisualDTO> GetVisualParallelStatesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualParallelStatesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateVisualDTO> GetVisualParallelStatesByRootFilterInternal(Framework.Workflow.Generated.DTO.ParallelStateRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.ParallelStateRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.ParallelStateVisualDTO> GetVisualParallelStatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.ParallelState>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for ParallelState
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasParallelStateAccess")]
        public virtual bool HasParallelStateAccess(HasParallelStateAccessAutoRequest hasParallelStateAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasParallelStateAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent = hasParallelStateAccessAutoRequest.parallelStateIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasParallelStateAccessInternal(parallelStateIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasParallelStateAccessInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelState;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.ParallelState domainObject = bll.GetById(parallelStateIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.ParallelState>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove ParallelState
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveParallelState")]
        public virtual void RemoveParallelState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveParallelStateInternal(parallelStateIdent, evaluateData));
        }
        
        protected virtual void RemoveParallelStateInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveParallelStateInternal(parallelStateIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveParallelStateInternal(Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.IParallelStateBLL bll)
        {
            Framework.Workflow.Domain.Definition.ParallelState domainObject = bll.GetById(parallelStateIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save ParallelStates
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveParallelState")]
        public virtual Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO SaveParallelState([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.ParallelStateStrictDTO parallelStateStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveParallelStateInternal(parallelStateStrict, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO SaveParallelStateInternal(Framework.Workflow.Generated.DTO.ParallelStateStrictDTO parallelStateStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IParallelStateBLL bll = evaluateData.Context.Logics.ParallelStateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveParallelStateInternal(parallelStateStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO SaveParallelStateInternal(Framework.Workflow.Generated.DTO.ParallelStateStrictDTO parallelStateStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.IParallelStateBLL bll)
        {
            Framework.Workflow.Domain.Definition.ParallelState domainObject = bll.GetById(parallelStateStrict.Id, true);
            parallelStateStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckParallelStateAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasParallelStateAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.ParallelStateIdentityDTO parallelStateIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
