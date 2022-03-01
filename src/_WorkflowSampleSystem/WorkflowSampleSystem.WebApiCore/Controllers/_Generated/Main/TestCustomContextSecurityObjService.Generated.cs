namespace WorkflowSampleSystem.WebApiCore.Controllers.Main
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class TestCustomContextSecurityObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public TestCustomContextSecurityObjController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObj")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObjByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testCustomContextSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjByNameInternal(testCustomContextSecurityObjName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjByNameInternal(string testCustomContextSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testCustomContextSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetById(testCustomContextSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestCustomContextSecurityObjs (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO> GetFullTestCustomContextSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObjs (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO> GetFullTestCustomContextSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjsByIdentsInternal(testCustomContextSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO> GetFullTestCustomContextSecurityObjsByIdentsInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(testCustomContextSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO> GetFullTestCustomContextSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestCustomContextSecurityObj")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestCustomContextSecurityObjInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestCustomContextSecurityObjByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testCustomContextSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestCustomContextSecurityObjByNameInternal(testCustomContextSecurityObjName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjByNameInternal(string testCustomContextSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testCustomContextSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetById(testCustomContextSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.FullDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObj")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObjByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testCustomContextSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjByNameInternal(testCustomContextSecurityObjName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjByNameInternal(string testCustomContextSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testCustomContextSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetById(testCustomContextSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestCustomContextSecurityObjs (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO> GetSimpleTestCustomContextSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObjs (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO> GetSimpleTestCustomContextSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjsByIdentsInternal(testCustomContextSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO> GetSimpleTestCustomContextSecurityObjsByIdentsInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(testCustomContextSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO> GetSimpleTestCustomContextSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObj")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObj([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObjByName")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string testCustomContextSecurityObjName)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjByNameInternal(testCustomContextSecurityObjName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjByNameInternal(string testCustomContextSecurityObjName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, testCustomContextSecurityObjName, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetById(testCustomContextSecurityObjIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of TestCustomContextSecurityObjs (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObjs")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO> GetVisualTestCustomContextSecurityObjs()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObjs (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObjsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO> GetVisualTestCustomContextSecurityObjsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjsByIdentsInternal(testCustomContextSecurityObjIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO> GetVisualTestCustomContextSecurityObjsByIdentsInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO[] testCustomContextSecurityObjIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(testCustomContextSecurityObjIdents, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO> GetVisualTestCustomContextSecurityObjsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObjProjection (ProjectionDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestCustomContextSecurityObjProjection")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjProjectionDTO GetTestCustomContextSecurityObjProjection([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjProjectionIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjProjectionInternal(testCustomContextSecurityObjProjectionIdentity, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjProjectionDTO GetTestCustomContextSecurityObjProjectionInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjProjectionIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjProjectionBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjProjectionFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.Projections.TestCustomContextSecurityObjProjection domainObject = bll.GetById(testCustomContextSecurityObjProjectionIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<WorkflowSampleSystem.Domain.Projections.TestCustomContextSecurityObjProjection>(Framework.Transfering.ViewDTOType.ProjectionDTO));
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToProjectionDTO(domainObject, evaluateData.MappingService);
        }
    }
}
