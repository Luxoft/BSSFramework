namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class StateInstanceController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public StateInstanceController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check StateInstance access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckStateInstanceAccess")]
        public virtual void CheckStateInstanceAccess(CheckStateInstanceAccessAutoRequest checkStateInstanceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkStateInstanceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdent = checkStateInstanceAccessAutoRequest.stateInstanceIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckStateInstanceAccessInternal(stateInstanceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckStateInstanceAccessInternal(Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstance;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Runtime.StateInstance domainObject = bll.GetById(stateInstanceIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Runtime.StateInstance>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get StateInstance (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateInstance")]
        public virtual Framework.Workflow.Generated.DTO.StateInstanceFullDTO GetFullStateInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateInstanceInternal(stateInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateInstanceFullDTO GetFullStateInstanceInternal(Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.StateInstance domainObject = bll.GetById(stateInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.StateInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StateInstances (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateInstanceFullDTO> GetFullStateInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StateInstances (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStateInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateInstanceFullDTO> GetFullStateInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO[] stateInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStateInstancesByIdentsInternal(stateInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateInstanceFullDTO> GetFullStateInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO[] stateInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(stateInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.StateInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateInstanceFullDTO> GetFullStateInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.StateInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StateInstance (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichStateInstance")]
        public virtual Framework.Workflow.Generated.DTO.StateInstanceRichDTO GetRichStateInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichStateInstanceInternal(stateInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateInstanceRichDTO GetRichStateInstanceInternal(Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.StateInstance domainObject = bll.GetById(stateInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.StateInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StateInstance (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateInstance")]
        public virtual Framework.Workflow.Generated.DTO.StateInstanceSimpleDTO GetSimpleStateInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateInstanceInternal(stateInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StateInstanceSimpleDTO GetSimpleStateInstanceInternal(Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.StateInstance domainObject = bll.GetById(stateInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.StateInstance>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StateInstances (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateInstanceSimpleDTO> GetSimpleStateInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StateInstances (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStateInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateInstanceSimpleDTO> GetSimpleStateInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO[] stateInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStateInstancesByIdentsInternal(stateInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateInstanceSimpleDTO> GetSimpleStateInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO[] stateInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(stateInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.StateInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StateInstanceSimpleDTO> GetSimpleStateInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.StateInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for StateInstance
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasStateInstanceAccess")]
        public virtual bool HasStateInstanceAccess(HasStateInstanceAccessAutoRequest hasStateInstanceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasStateInstanceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdent = hasStateInstanceAccessAutoRequest.stateInstanceIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasStateInstanceAccessInternal(stateInstanceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasStateInstanceAccessInternal(Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStateInstanceBLL bll = evaluateData.Context.Logics.StateInstance;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Runtime.StateInstance domainObject = bll.GetById(stateInstanceIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Runtime.StateInstance>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckStateInstanceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasStateInstanceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.StateInstanceIdentityDTO stateInstanceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
