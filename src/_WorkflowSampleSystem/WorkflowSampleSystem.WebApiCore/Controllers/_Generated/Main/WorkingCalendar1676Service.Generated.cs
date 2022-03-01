namespace WorkflowSampleSystem.WebApiCore.Controllers.Main
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class WorkingCalendar1676Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public WorkingCalendar1676Controller(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check WorkingCalendar1676 access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckWorkingCalendar1676Access")]
        public virtual void CheckWorkingCalendar1676Access(CheckWorkingCalendar1676AccessAutoRequest checkWorkingCalendar1676AccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = checkWorkingCalendar1676AccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Ident = checkWorkingCalendar1676AccessAutoRequest.workingCalendar1676Ident;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckWorkingCalendar1676AccessInternal(workingCalendar1676Ident, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckWorkingCalendar1676AccessInternal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Ident, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Ident.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkingCalendar1676")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676Internal(workingCalendar1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkingCalendar1676ByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workingCalendar1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676ByNameInternal(workingCalendar1676Name, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676ByNameInternal(string workingCalendar1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workingCalendar1676Name, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676FullDTO GetFullWorkingCalendar1676Internal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkingCalendar1676s (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkingCalendar1676s")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676FullDTO> GetFullWorkingCalendar1676s()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676s (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullWorkingCalendar1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676FullDTO> GetFullWorkingCalendar1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullWorkingCalendar1676sByIdentsInternal(workingCalendar1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676FullDTO> GetFullWorkingCalendar1676sByIdentsInternal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(workingCalendar1676Idents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676FullDTO> GetFullWorkingCalendar1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichWorkingCalendar1676")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichWorkingCalendar1676Internal(workingCalendar1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichWorkingCalendar1676ByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workingCalendar1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichWorkingCalendar1676ByNameInternal(workingCalendar1676Name, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676ByNameInternal(string workingCalendar1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workingCalendar1676Name, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676RichDTO GetRichWorkingCalendar1676Internal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkingCalendar1676")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676Internal(workingCalendar1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkingCalendar1676ByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workingCalendar1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676ByNameInternal(workingCalendar1676Name, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676ByNameInternal(string workingCalendar1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workingCalendar1676Name, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO GetSimpleWorkingCalendar1676Internal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkingCalendar1676s (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkingCalendar1676s")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO> GetSimpleWorkingCalendar1676s()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676s (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleWorkingCalendar1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO> GetSimpleWorkingCalendar1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleWorkingCalendar1676sByIdentsInternal(workingCalendar1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO> GetSimpleWorkingCalendar1676sByIdentsInternal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(workingCalendar1676Idents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676SimpleDTO> GetSimpleWorkingCalendar1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkingCalendar1676")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676Internal(workingCalendar1676Identity, evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676 (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkingCalendar1676ByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676ByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string workingCalendar1676Name)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676ByNameInternal(workingCalendar1676Name, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676ByNameInternal(string workingCalendar1676Name, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, workingCalendar1676Name, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO GetVisualWorkingCalendar1676Internal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Identity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of WorkingCalendar1676s (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkingCalendar1676s")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO> GetVisualWorkingCalendar1676s()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676sInternal(evaluateData));
        }
        
        /// <summary>
        /// Get WorkingCalendar1676s (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualWorkingCalendar1676sByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO> GetVisualWorkingCalendar1676sByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualWorkingCalendar1676sByIdentsInternal(workingCalendar1676Idents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO> GetVisualWorkingCalendar1676sByIdentsInternal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO[] workingCalendar1676Idents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(workingCalendar1676Idents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676VisualDTO> GetVisualWorkingCalendar1676sInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for WorkingCalendar1676
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasWorkingCalendar1676Access")]
        public virtual bool HasWorkingCalendar1676Access(HasWorkingCalendar1676AccessAutoRequest hasWorkingCalendar1676AccessAutoRequest)
        {
            WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode = hasWorkingCalendar1676AccessAutoRequest.securityOperationCode;
            WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Ident = hasWorkingCalendar1676AccessAutoRequest.workingCalendar1676Ident;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasWorkingCalendar1676AccessInternal(workingCalendar1676Ident, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasWorkingCalendar1676AccessInternal(WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Ident, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IWorkingCalendar1676BLL bll = evaluateData.Context.Logics.WorkingCalendar1676;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676 domainObject = bll.GetById(workingCalendar1676Ident.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<WorkflowSampleSystem.Domain.EnversBug1676.WorkingCalendar1676>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckWorkingCalendar1676AccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Ident;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasWorkingCalendar1676AccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.WorkingCalendar1676IdentityDTO workingCalendar1676Ident;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode securityOperationCode;
    }
}
