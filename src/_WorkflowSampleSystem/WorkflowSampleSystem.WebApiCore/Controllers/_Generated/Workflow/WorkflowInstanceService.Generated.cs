namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class WorkflowInstanceController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public WorkflowInstanceController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check WorkflowInstance access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckWorkflowInstanceAccess")]
        public virtual void CheckWorkflowInstanceAccess(CheckWorkflowInstanceAccessAutoRequest checkWorkflowInstanceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkWorkflowInstanceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent = checkWorkflowInstanceAccessAutoRequest.workflowInstanceIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckWorkflowInstanceAccessInternal(workflowInstanceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckWorkflowInstanceAccessInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstance;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = bll.GetById(workflowInstanceIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Runtime.WorkflowInstance>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get WorkflowInstance (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowInstance")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO GetFullWorkflowInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowInstanceInternal(workflowInstanceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowInstance (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowInstanceByName")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO GetFullWorkflowInstanceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workflowInstanceName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowInstanceByNameInternal(workflowInstanceName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO GetFullWorkflowInstanceByNameInternal(string workflowInstanceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workflowInstanceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO GetFullWorkflowInstanceInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = bll.GetById(workflowInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkflowInstances (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO> GetFullWorkflowInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowInstances (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO> GetFullWorkflowInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO[] workflowInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowInstancesByIdentsInternal(workflowInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO> GetFullWorkflowInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO[] workflowInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(workflowInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowInstances (FullDTO) by filter (Framework.Workflow.Domain.WorkflowInstanceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowInstancesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO> GetFullWorkflowInstancesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowInstancesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO> GetFullWorkflowInstancesByRootFilterInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.WorkflowInstanceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceFullDTO> GetFullWorkflowInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowInstance (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichWorkflowInstance")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceRichDTO GetRichWorkflowInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichWorkflowInstanceInternal(workflowInstanceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowInstance (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichWorkflowInstanceByName")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceRichDTO GetRichWorkflowInstanceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workflowInstanceName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichWorkflowInstanceByNameInternal(workflowInstanceName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceRichDTO GetRichWorkflowInstanceByNameInternal(string workflowInstanceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workflowInstanceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceRichDTO GetRichWorkflowInstanceInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = bll.GetById(workflowInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowInstance (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowInstance")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO GetSimpleWorkflowInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowInstanceInternal(workflowInstanceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowInstance (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowInstanceByName")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO GetSimpleWorkflowInstanceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workflowInstanceName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowInstanceByNameInternal(workflowInstanceName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO GetSimpleWorkflowInstanceByNameInternal(string workflowInstanceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workflowInstanceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO GetSimpleWorkflowInstanceInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = bll.GetById(workflowInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkflowInstances (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO> GetSimpleWorkflowInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowInstances (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO> GetSimpleWorkflowInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO[] workflowInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowInstancesByIdentsInternal(workflowInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO> GetSimpleWorkflowInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO[] workflowInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(workflowInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowInstances (SimpleDTO) by filter (Framework.Workflow.Domain.WorkflowInstanceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowInstancesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO> GetSimpleWorkflowInstancesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowInstancesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO> GetSimpleWorkflowInstancesByRootFilterInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.WorkflowInstanceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceSimpleDTO> GetSimpleWorkflowInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowInstance (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowInstance")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO GetVisualWorkflowInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowInstanceInternal(workflowInstanceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowInstance (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowInstanceByName")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO GetVisualWorkflowInstanceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workflowInstanceName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowInstanceByNameInternal(workflowInstanceName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO GetVisualWorkflowInstanceByNameInternal(string workflowInstanceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workflowInstanceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO GetVisualWorkflowInstanceInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = bll.GetById(workflowInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkflowInstances (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO> GetVisualWorkflowInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowInstances (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO> GetVisualWorkflowInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO[] workflowInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowInstancesByIdentsInternal(workflowInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO> GetVisualWorkflowInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO[] workflowInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(workflowInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowInstances (VisualDTO) by filter (Framework.Workflow.Domain.WorkflowInstanceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowInstancesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO> GetVisualWorkflowInstancesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowInstancesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO> GetVisualWorkflowInstancesByRootFilterInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.WorkflowInstanceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowInstanceVisualDTO> GetVisualWorkflowInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.WorkflowInstance>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for WorkflowInstance
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasWorkflowInstanceAccess")]
        public virtual bool HasWorkflowInstanceAccess(HasWorkflowInstanceAccessAutoRequest hasWorkflowInstanceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasWorkflowInstanceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent = hasWorkflowInstanceAccessAutoRequest.workflowInstanceIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasWorkflowInstanceAccessInternal(workflowInstanceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasWorkflowInstanceAccessInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstance;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = bll.GetById(workflowInstanceIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Runtime.WorkflowInstance>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove WorkflowInstance
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveWorkflowInstance")]
        public virtual void RemoveWorkflowInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveWorkflowInstanceInternal(workflowInstanceIdent, evaluateData));
        }
        
        protected virtual void RemoveWorkflowInstanceInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveWorkflowInstanceInternal(workflowInstanceIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveWorkflowInstanceInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.IWorkflowInstanceBLL bll)
        {
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = bll.GetById(workflowInstanceIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Remove WorkflowInstances
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveWorkflowInstances")]
        public virtual void RemoveWorkflowInstances([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO[] idents)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveWorkflowInstancesInternal(idents, evaluateData));
        }
        
        protected virtual void RemoveWorkflowInstancesInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO[] idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            Framework.Core.EnumerableExtensions.Foreach(idents, workflowInstance => this.RemoveWorkflowInstanceInternal(workflowInstance, evaluateData, bll));
        }
        
        /// <summary>
        /// Save WorkflowInstances
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveWorkflowInstance")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO SaveWorkflowInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowInstanceStrictDTO workflowInstanceStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveWorkflowInstanceInternal(workflowInstanceStrict, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO SaveWorkflowInstanceInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceStrictDTO workflowInstanceStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowInstanceBLL bll = evaluateData.Context.Logics.WorkflowInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveWorkflowInstanceInternal(workflowInstanceStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO SaveWorkflowInstanceInternal(Framework.Workflow.Generated.DTO.WorkflowInstanceStrictDTO workflowInstanceStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.IWorkflowInstanceBLL bll)
        {
            Framework.Workflow.Domain.Runtime.WorkflowInstance domainObject = bll.GetById(workflowInstanceStrict.Id, true);
            workflowInstanceStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckWorkflowInstanceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasWorkflowInstanceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.WorkflowInstanceIdentityDTO workflowInstanceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
