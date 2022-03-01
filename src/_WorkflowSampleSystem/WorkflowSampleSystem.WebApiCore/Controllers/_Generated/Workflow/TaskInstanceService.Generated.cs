namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class TaskInstanceController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public TaskInstanceController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check TaskInstance access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckTaskInstanceAccess")]
        public virtual void CheckTaskInstanceAccess(CheckTaskInstanceAccessAutoRequest checkTaskInstanceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkTaskInstanceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdent = checkTaskInstanceAccessAutoRequest.taskInstanceIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckTaskInstanceAccessInternal(taskInstanceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckTaskInstanceAccessInternal(Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstance;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Runtime.TaskInstance domainObject = bll.GetById(taskInstanceIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Runtime.TaskInstance>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TaskInstance (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTaskInstance")]
        public virtual Framework.Workflow.Generated.DTO.TaskInstanceFullDTO GetFullTaskInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTaskInstanceInternal(taskInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.TaskInstanceFullDTO GetFullTaskInstanceInternal(Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.TaskInstance domainObject = bll.GetById(taskInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TaskInstances (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTaskInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceFullDTO> GetFullTaskInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTaskInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TaskInstances (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTaskInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceFullDTO> GetFullTaskInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO[] taskInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTaskInstancesByIdentsInternal(taskInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceFullDTO> GetFullTaskInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO[] taskInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(taskInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TaskInstances (FullDTO) by filter (Framework.Workflow.Domain.TaskInstanceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTaskInstancesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceFullDTO> GetFullTaskInstancesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TaskInstanceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTaskInstancesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceFullDTO> GetFullTaskInstancesByRootFilterInternal(Framework.Workflow.Generated.DTO.TaskInstanceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.TaskInstanceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceFullDTO> GetFullTaskInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TaskInstance (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTaskInstance")]
        public virtual Framework.Workflow.Generated.DTO.TaskInstanceRichDTO GetRichTaskInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTaskInstanceInternal(taskInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.TaskInstanceRichDTO GetRichTaskInstanceInternal(Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.TaskInstance domainObject = bll.GetById(taskInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TaskInstance (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTaskInstance")]
        public virtual Framework.Workflow.Generated.DTO.TaskInstanceSimpleDTO GetSimpleTaskInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTaskInstanceInternal(taskInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.TaskInstanceSimpleDTO GetSimpleTaskInstanceInternal(Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.TaskInstance domainObject = bll.GetById(taskInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TaskInstances (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTaskInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceSimpleDTO> GetSimpleTaskInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTaskInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TaskInstances (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTaskInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceSimpleDTO> GetSimpleTaskInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO[] taskInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTaskInstancesByIdentsInternal(taskInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceSimpleDTO> GetSimpleTaskInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO[] taskInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(taskInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TaskInstances (SimpleDTO) by filter (Framework.Workflow.Domain.TaskInstanceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTaskInstancesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceSimpleDTO> GetSimpleTaskInstancesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TaskInstanceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTaskInstancesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceSimpleDTO> GetSimpleTaskInstancesByRootFilterInternal(Framework.Workflow.Generated.DTO.TaskInstanceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.TaskInstanceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TaskInstanceSimpleDTO> GetSimpleTaskInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TaskInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for TaskInstance
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasTaskInstanceAccess")]
        public virtual bool HasTaskInstanceAccess(HasTaskInstanceAccessAutoRequest hasTaskInstanceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasTaskInstanceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdent = hasTaskInstanceAccessAutoRequest.taskInstanceIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasTaskInstanceAccessInternal(taskInstanceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasTaskInstanceAccessInternal(Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstance;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Runtime.TaskInstance domainObject = bll.GetById(taskInstanceIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Runtime.TaskInstance>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Save TaskInstances
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveTaskInstance")]
        public virtual Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO SaveTaskInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TaskInstanceStrictDTO taskInstanceStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveTaskInstanceInternal(taskInstanceStrict, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO SaveTaskInstanceInternal(Framework.Workflow.Generated.DTO.TaskInstanceStrictDTO taskInstanceStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITaskInstanceBLL bll = evaluateData.Context.Logics.TaskInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveTaskInstanceInternal(taskInstanceStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO SaveTaskInstanceInternal(Framework.Workflow.Generated.DTO.TaskInstanceStrictDTO taskInstanceStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.ITaskInstanceBLL bll)
        {
            Framework.Workflow.Domain.Runtime.TaskInstance domainObject = bll.GetById(taskInstanceStrict.Id, true);
            taskInstanceStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckTaskInstanceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasTaskInstanceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.TaskInstanceIdentityDTO taskInstanceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
