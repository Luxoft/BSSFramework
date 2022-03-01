namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class StartWorkflowDomainObjectConditionController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public StartWorkflowDomainObjectConditionController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check StartWorkflowDomainObjectCondition access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckStartWorkflowDomainObjectConditionAccess")]
        public virtual void CheckStartWorkflowDomainObjectConditionAccess(CheckStartWorkflowDomainObjectConditionAccessAutoRequest checkStartWorkflowDomainObjectConditionAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkStartWorkflowDomainObjectConditionAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent = checkStartWorkflowDomainObjectConditionAccessAutoRequest.startWorkflowDomainObjectConditionIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckStartWorkflowDomainObjectConditionAccessInternal(startWorkflowDomainObjectConditionIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckStartWorkflowDomainObjectConditionAccessInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectCondition;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition domainObject = bll.GetById(startWorkflowDomainObjectConditionIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get StartWorkflowDomainObjectCondition (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStartWorkflowDomainObjectCondition")]
        public virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionFullDTO GetFullStartWorkflowDomainObjectCondition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStartWorkflowDomainObjectConditionInternal(startWorkflowDomainObjectConditionIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionFullDTO GetFullStartWorkflowDomainObjectConditionInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition domainObject = bll.GetById(startWorkflowDomainObjectConditionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StartWorkflowDomainObjectConditions (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStartWorkflowDomainObjectConditions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionFullDTO> GetFullStartWorkflowDomainObjectConditions()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStartWorkflowDomainObjectConditionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StartWorkflowDomainObjectConditions (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStartWorkflowDomainObjectConditionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionFullDTO> GetFullStartWorkflowDomainObjectConditionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO[] startWorkflowDomainObjectConditionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStartWorkflowDomainObjectConditionsByIdentsInternal(startWorkflowDomainObjectConditionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionFullDTO> GetFullStartWorkflowDomainObjectConditionsByIdentsInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO[] startWorkflowDomainObjectConditionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(startWorkflowDomainObjectConditionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StartWorkflowDomainObjectConditions (FullDTO) by filter (Framework.Workflow.Domain.StartWorkflowDomainObjectConditionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullStartWorkflowDomainObjectConditionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionFullDTO> GetFullStartWorkflowDomainObjectConditionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullStartWorkflowDomainObjectConditionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionFullDTO> GetFullStartWorkflowDomainObjectConditionsByRootFilterInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.StartWorkflowDomainObjectConditionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionFullDTO> GetFullStartWorkflowDomainObjectConditionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StartWorkflowDomainObjectCondition (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichStartWorkflowDomainObjectCondition")]
        public virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionRichDTO GetRichStartWorkflowDomainObjectCondition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichStartWorkflowDomainObjectConditionInternal(startWorkflowDomainObjectConditionIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionRichDTO GetRichStartWorkflowDomainObjectConditionInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition domainObject = bll.GetById(startWorkflowDomainObjectConditionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StartWorkflowDomainObjectCondition (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStartWorkflowDomainObjectCondition")]
        public virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionSimpleDTO GetSimpleStartWorkflowDomainObjectCondition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStartWorkflowDomainObjectConditionInternal(startWorkflowDomainObjectConditionIdentity, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionSimpleDTO GetSimpleStartWorkflowDomainObjectConditionInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition domainObject = bll.GetById(startWorkflowDomainObjectConditionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of StartWorkflowDomainObjectConditions (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStartWorkflowDomainObjectConditions")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionSimpleDTO> GetSimpleStartWorkflowDomainObjectConditions()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStartWorkflowDomainObjectConditionsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get StartWorkflowDomainObjectConditions (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStartWorkflowDomainObjectConditionsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionSimpleDTO> GetSimpleStartWorkflowDomainObjectConditionsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO[] startWorkflowDomainObjectConditionIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStartWorkflowDomainObjectConditionsByIdentsInternal(startWorkflowDomainObjectConditionIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionSimpleDTO> GetSimpleStartWorkflowDomainObjectConditionsByIdentsInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO[] startWorkflowDomainObjectConditionIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(startWorkflowDomainObjectConditionIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get StartWorkflowDomainObjectConditions (SimpleDTO) by filter (Framework.Workflow.Domain.StartWorkflowDomainObjectConditionRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleStartWorkflowDomainObjectConditionsByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionSimpleDTO> GetSimpleStartWorkflowDomainObjectConditionsByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleStartWorkflowDomainObjectConditionsByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionSimpleDTO> GetSimpleStartWorkflowDomainObjectConditionsByRootFilterInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.StartWorkflowDomainObjectConditionRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionSimpleDTO> GetSimpleStartWorkflowDomainObjectConditionsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for StartWorkflowDomainObjectCondition
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasStartWorkflowDomainObjectConditionAccess")]
        public virtual bool HasStartWorkflowDomainObjectConditionAccess(HasStartWorkflowDomainObjectConditionAccessAutoRequest hasStartWorkflowDomainObjectConditionAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasStartWorkflowDomainObjectConditionAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent = hasStartWorkflowDomainObjectConditionAccessAutoRequest.startWorkflowDomainObjectConditionIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasStartWorkflowDomainObjectConditionAccessInternal(startWorkflowDomainObjectConditionIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasStartWorkflowDomainObjectConditionAccessInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectCondition;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition domainObject = bll.GetById(startWorkflowDomainObjectConditionIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition>(securityOperationCode).HasAccess(domainObject);
        }
        
        /// <summary>
        /// Remove StartWorkflowDomainObjectCondition
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("RemoveStartWorkflowDomainObjectCondition")]
        public virtual void RemoveStartWorkflowDomainObjectCondition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent)
        {
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.RemoveStartWorkflowDomainObjectConditionInternal(startWorkflowDomainObjectConditionIdent, evaluateData));
        }
        
        protected virtual void RemoveStartWorkflowDomainObjectConditionInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            this.RemoveStartWorkflowDomainObjectConditionInternal(startWorkflowDomainObjectConditionIdent, evaluateData, bll);
        }
        
        protected virtual void RemoveStartWorkflowDomainObjectConditionInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll)
        {
            Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition domainObject = bll.GetById(startWorkflowDomainObjectConditionIdent.Id, true);
            bll.Remove(domainObject);
        }
        
        /// <summary>
        /// Save StartWorkflowDomainObjectConditions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("SaveStartWorkflowDomainObjectCondition")]
        public virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO SaveStartWorkflowDomainObjectCondition([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionStrictDTO startWorkflowDomainObjectConditionStrict)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Write, evaluateData => this.SaveStartWorkflowDomainObjectConditionInternal(startWorkflowDomainObjectConditionStrict, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO SaveStartWorkflowDomainObjectConditionInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionStrictDTO startWorkflowDomainObjectConditionStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll = evaluateData.Context.Logics.StartWorkflowDomainObjectConditionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.Edit);
            return this.SaveStartWorkflowDomainObjectConditionInternal(startWorkflowDomainObjectConditionStrict, evaluateData, bll);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO SaveStartWorkflowDomainObjectConditionInternal(Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionStrictDTO startWorkflowDomainObjectConditionStrict, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData, Framework.Workflow.BLL.IStartWorkflowDomainObjectConditionBLL bll)
        {
            Framework.Workflow.Domain.Definition.StartWorkflowDomainObjectCondition domainObject = bll.GetById(startWorkflowDomainObjectConditionStrict.Id, true);
            startWorkflowDomainObjectConditionStrict.MapToDomainObject(evaluateData.MappingService, domainObject);
            bll.Save(domainObject);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToIdentityDTO(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckStartWorkflowDomainObjectConditionAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasStartWorkflowDomainObjectConditionAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.StartWorkflowDomainObjectConditionIdentityDTO startWorkflowDomainObjectConditionIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
