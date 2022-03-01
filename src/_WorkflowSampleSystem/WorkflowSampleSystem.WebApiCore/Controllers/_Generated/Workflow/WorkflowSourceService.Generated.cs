namespace Workflow.WebApi.Controllers
{
    using Framework.Workflow.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("workflowApi/v{version:apiVersion}/[controller]")]
    public partial class WorkflowSourceController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext>, Framework.Workflow.BLL.IWorkflowBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>>
    {
        
        public WorkflowSourceController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Workflow.BLL.IWorkflowBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check WorkflowSource access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckWorkflowSourceAccess")]
        public virtual void CheckWorkflowSourceAccess(CheckWorkflowSourceAccessAutoRequest checkWorkflowSourceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = checkWorkflowSourceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdent = checkWorkflowSourceAccessAutoRequest.workflowSourceIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckWorkflowSourceAccessInternal(workflowSourceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckWorkflowSourceAccessInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSource;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = bll.GetById(workflowSourceIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.WorkflowSource>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Workflow.BLL.IWorkflowBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService>(session, context, new WorkflowServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get WorkflowSource (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowSource")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO GetFullWorkflowSource([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowSourceInternal(workflowSourceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowSource (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowSourceByName")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO GetFullWorkflowSourceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workflowSourceName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowSourceByNameInternal(workflowSourceName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO GetFullWorkflowSourceByNameInternal(string workflowSourceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workflowSourceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO GetFullWorkflowSourceInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = bll.GetById(workflowSourceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkflowSources (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowSources")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO> GetFullWorkflowSources()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowSourcesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowSources (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowSourcesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO> GetFullWorkflowSourcesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO[] workflowSourceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowSourcesByIdentsInternal(workflowSourceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO> GetFullWorkflowSourcesByIdentsInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO[] workflowSourceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(workflowSourceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowSources (FullDTO) by filter (Framework.Workflow.Domain.WorkflowSourceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkflowSourcesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO> GetFullWorkflowSourcesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkflowSourcesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO> GetFullWorkflowSourcesByRootFilterInternal(Framework.Workflow.Generated.DTO.WorkflowSourceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.WorkflowSourceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceFullDTO> GetFullWorkflowSourcesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowSource (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichWorkflowSource")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowSourceRichDTO GetRichWorkflowSource([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichWorkflowSourceInternal(workflowSourceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowSource (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichWorkflowSourceByName")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowSourceRichDTO GetRichWorkflowSourceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workflowSourceName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichWorkflowSourceByNameInternal(workflowSourceName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowSourceRichDTO GetRichWorkflowSourceByNameInternal(string workflowSourceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workflowSourceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowSourceRichDTO GetRichWorkflowSourceInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = bll.GetById(workflowSourceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.FullDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowSource (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowSource")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO GetSimpleWorkflowSource([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowSourceInternal(workflowSourceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowSource (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowSourceByName")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO GetSimpleWorkflowSourceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workflowSourceName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowSourceByNameInternal(workflowSourceName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO GetSimpleWorkflowSourceByNameInternal(string workflowSourceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workflowSourceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO GetSimpleWorkflowSourceInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = bll.GetById(workflowSourceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkflowSources (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowSources")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO> GetSimpleWorkflowSources()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowSourcesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowSources (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowSourcesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO> GetSimpleWorkflowSourcesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO[] workflowSourceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowSourcesByIdentsInternal(workflowSourceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO> GetSimpleWorkflowSourcesByIdentsInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO[] workflowSourceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(workflowSourceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowSources (SimpleDTO) by filter (Framework.Workflow.Domain.WorkflowSourceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkflowSourcesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO> GetSimpleWorkflowSourcesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkflowSourcesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO> GetSimpleWorkflowSourcesByRootFilterInternal(Framework.Workflow.Generated.DTO.WorkflowSourceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.WorkflowSourceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceSimpleDTO> GetSimpleWorkflowSourcesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowSource (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowSource")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO GetVisualWorkflowSource([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowSourceInternal(workflowSourceIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowSource (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowSourceByName")]
        public virtual Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO GetVisualWorkflowSourceByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workflowSourceName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowSourceByNameInternal(workflowSourceName, evaluateData));
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO GetVisualWorkflowSourceByNameInternal(string workflowSourceName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workflowSourceName, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO GetVisualWorkflowSourceInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = bll.GetById(workflowSourceIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.VisualDTO));
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkflowSources (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowSources")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO> GetVisualWorkflowSources()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowSourcesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkflowSources (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowSourcesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO> GetVisualWorkflowSourcesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO[] workflowSourceIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowSourcesByIdentsInternal(workflowSourceIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO> GetVisualWorkflowSourcesByIdentsInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO[] workflowSourceIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(workflowSourceIdents, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkflowSources (VisualDTO) by filter (Framework.Workflow.Domain.WorkflowSourceRootFilterModel)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkflowSourcesByRootFilter")]
        public virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO> GetVisualWorkflowSourcesByRootFilter([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] Framework.Workflow.Generated.DTO.WorkflowSourceRootFilterModelStrictDTO filter)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkflowSourcesByRootFilterInternal(filter, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO> GetVisualWorkflowSourcesByRootFilterInternal(Framework.Workflow.Generated.DTO.WorkflowSourceRootFilterModelStrictDTO filter, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            Framework.Workflow.Domain.WorkflowSourceRootFilterModel typedFilter = filter.ToDomainObject(evaluateData.MappingService);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListBy(typedFilter, evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<Framework.Workflow.Generated.DTO.WorkflowSourceVisualDTO> GetVisualWorkflowSourcesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSourceFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return Framework.Workflow.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<Framework.Workflow.Domain.Definition.WorkflowSource>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for WorkflowSource
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasWorkflowSourceAccess")]
        public virtual bool HasWorkflowSourceAccess(HasWorkflowSourceAccessAutoRequest hasWorkflowSourceAccessAutoRequest)
        {
            Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode = hasWorkflowSourceAccessAutoRequest.securityOperationCode;
            Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdent = hasWorkflowSourceAccessAutoRequest.workflowSourceIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasWorkflowSourceAccessInternal(workflowSourceIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasWorkflowSourceAccessInternal(Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdent, Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Workflow.BLL.IWorkflowBLLContext, Framework.Workflow.Generated.DTO.IWorkflowDTOMappingService> evaluateData)
        {
            Framework.Workflow.BLL.IWorkflowSourceBLL bll = evaluateData.Context.Logics.WorkflowSource;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            Framework.Workflow.Domain.Definition.WorkflowSource domainObject = bll.GetById(workflowSourceIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<Framework.Workflow.Domain.Definition.WorkflowSource>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckWorkflowSourceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasWorkflowSourceAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public Framework.Workflow.Generated.DTO.WorkflowSourceIdentityDTO workflowSourceIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public Framework.Workflow.WorkflowSecurityOperationCode securityOperationCode;
    }
}
