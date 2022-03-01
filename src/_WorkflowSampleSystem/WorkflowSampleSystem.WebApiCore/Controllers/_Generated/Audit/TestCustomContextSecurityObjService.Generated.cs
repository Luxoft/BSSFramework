namespace WorkflowSampleSystem.WebApiCore.Controllers.Audit
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
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
        /// Get TestCustomContextSecurityObj (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestCustomContextSecurityObjWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjWithRevision(GetFullTestCustomContextSecurityObjWithRevisionAutoRequest getFullTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getFullTestCustomContextSecurityObjWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getFullTestCustomContextSecurityObjWithRevisionAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjFullDTO GetFullTestCustomContextSecurityObjWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestCustomContextSecurityObjWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjWithRevision(GetRichTestCustomContextSecurityObjWithRevisionAutoRequest getRichTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getRichTestCustomContextSecurityObjWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getRichTestCustomContextSecurityObjWithRevisionAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjRichDTO GetRichTestCustomContextSecurityObjWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestCustomContextSecurityObjWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjWithRevision(GetSimpleTestCustomContextSecurityObjWithRevisionAutoRequest getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getSimpleTestCustomContextSecurityObjWithRevisionAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjSimpleDTO GetSimpleTestCustomContextSecurityObjWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestCustomContextSecurityObjPropertyRevisionByDateRange")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionByDateRange(GetTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjPropertyRevisionByDateRangeInternal(testCustomContextSecurityObjIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionByDateRangeInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(testCustomContextSecurityObjIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestCustomContextSecurityObjPropertyRevisions")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisions(GetTestCustomContextSecurityObjPropertyRevisionsAutoRequest getTestCustomContextSecurityObjPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestCustomContextSecurityObjPropertyRevisionsAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getTestCustomContextSecurityObjPropertyRevisionsAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjPropertyRevisionsInternal(testCustomContextSecurityObjIdentity, propertyName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetTestCustomContextSecurityObjPropertyRevisionsInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.TestCustomContextSecurityObj>(testCustomContextSecurityObjIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestCustomContextSecurityObjRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestCustomContextSecurityObjRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestCustomContextSecurityObjRevisionsInternal(testCustomContextSecurityObjIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestCustomContextSecurityObjRevisionsInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testCustomContextSecurityObjIdentity.Id));
        }
        
        /// <summary>
        /// Get TestCustomContextSecurityObj (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestCustomContextSecurityObjWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjWithRevision(GetVisualTestCustomContextSecurityObjWithRevisionAutoRequest getVisualTestCustomContextSecurityObjWithRevisionAutoRequest)
        {
            long revision = getVisualTestCustomContextSecurityObjWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity = getVisualTestCustomContextSecurityObjWithRevisionAutoRequest.testCustomContextSecurityObjIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestCustomContextSecurityObjWithRevisionInternal(testCustomContextSecurityObjIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjVisualDTO GetVisualTestCustomContextSecurityObjWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestCustomContextSecurityObjBLL bll = evaluateData.Context.Logics.TestCustomContextSecurityObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestCustomContextSecurityObj domainObject = bll.GetObjectByRevision(testCustomContextSecurityObjIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestCustomContextSecurityObjPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestCustomContextSecurityObjPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestCustomContextSecurityObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestCustomContextSecurityObjIdentityDTO testCustomContextSecurityObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
