namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class TransitionInstanceController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public TransitionInstanceController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check TransitionInstance access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckTransitionInstanceAccess")]
        public virtual void CheckTransitionInstanceAccess(CheckTransitionInstanceAccessAutoRequest checkTransitionInstanceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkTransitionInstanceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdent = checkTransitionInstanceAccessAutoRequest.transitionInstanceIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckTransitionInstanceAccessInternal(transitionInstanceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckTransitionInstanceAccessInternal(Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstance;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Runtime.TransitionInstance domainObject = bll.GetById(transitionInstanceIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Runtime.TransitionInstance>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TransitionInstance (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTransitionInstance")]
        public virtual Framework.Workflow.Generated.DTO.TransitionInstanceFullDTO GetFullTransitionInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTransitionInstanceInternal(transitionInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.TransitionInstanceFullDTO GetFullTransitionInstanceInternal(Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.TransitionInstance domainObject = bll.GetById(transitionInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TransitionInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TransitionInstances (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTransitionInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TransitionInstanceFullDTO> GetFullTransitionInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTransitionInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TransitionInstances (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTransitionInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TransitionInstanceFullDTO> GetFullTransitionInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO[] transitionInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTransitionInstancesByIdentsInternal(transitionInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TransitionInstanceFullDTO> GetFullTransitionInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO[] transitionInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(transitionInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TransitionInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TransitionInstanceFullDTO> GetFullTransitionInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TransitionInstance>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TransitionInstance (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTransitionInstance")]
        public virtual Framework.Workflow.Generated.DTO.TransitionInstanceRichDTO GetRichTransitionInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTransitionInstanceInternal(transitionInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.TransitionInstanceRichDTO GetRichTransitionInstanceInternal(Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.TransitionInstance domainObject = bll.GetById(transitionInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TransitionInstance>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TransitionInstance (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTransitionInstance")]
        public virtual Framework.Workflow.Generated.DTO.TransitionInstanceSimpleDTO GetSimpleTransitionInstance([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTransitionInstanceInternal(transitionInstanceIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.TransitionInstanceSimpleDTO GetSimpleTransitionInstanceInternal(Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Runtime.TransitionInstance domainObject = bll.GetById(transitionInstanceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TransitionInstance>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TransitionInstances (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTransitionInstances")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TransitionInstanceSimpleDTO> GetSimpleTransitionInstances()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTransitionInstancesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TransitionInstances (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTransitionInstancesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TransitionInstanceSimpleDTO> GetSimpleTransitionInstancesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO[] transitionInstanceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTransitionInstancesByIdentsInternal(transitionInstanceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TransitionInstanceSimpleDTO> GetSimpleTransitionInstancesByIdentsInternal(Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO[] transitionInstanceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(transitionInstanceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TransitionInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.TransitionInstanceSimpleDTO> GetSimpleTransitionInstancesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstanceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Runtime.TransitionInstance>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for TransitionInstance
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasTransitionInstanceAccess")]
        public virtual bool HasTransitionInstanceAccess(HasTransitionInstanceAccessAutoRequest hasTransitionInstanceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasTransitionInstanceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdent = hasTransitionInstanceAccessAutoRequest.transitionInstanceIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasTransitionInstanceAccessInternal(transitionInstanceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasTransitionInstanceAccessInternal(Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.ITransitionInstanceBLL bll = evaluateData.Context.Logics.TransitionInstance;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Runtime.TransitionInstance domainObject = bll.GetById(transitionInstanceIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Runtime.TransitionInstance>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckTransitionInstanceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasTransitionInstanceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.TransitionInstanceIdentityDTO transitionInstanceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
