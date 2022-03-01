namespace WorkflowSampleSystem.WebApiCore.Controllers.Audit
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestSecuritySubObjItem3Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public TestSecuritySubObjItem3Controller(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem3WithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3WithRevision(GetFullTestSecuritySubObjItem3WithRevisionAutoRequest getFullTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getFullTestSecuritySubObjItem3WithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getFullTestSecuritySubObjItem3WithRevisionAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3WithRevisionInternal(WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecuritySubObjItem3WithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3WithRevision(GetRichTestSecuritySubObjItem3WithRevisionAutoRequest getRichTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getRichTestSecuritySubObjItem3WithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getRichTestSecuritySubObjItem3WithRevisionAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3WithRevisionInternal(WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem3WithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3WithRevision(GetSimpleTestSecuritySubObjItem3WithRevisionAutoRequest getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3WithRevisionInternal(WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3PropertyRevisionByDateRange")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionByDateRange(GetTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3PropertyRevisionByDateRangeInternal(testSecuritySubObjItem3Identity, propertyName, period, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionByDateRangeInternal(WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.TestSecuritySubObjItem3>(testSecuritySubObjItem3Identity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3PropertyRevisions")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisions(GetTestSecuritySubObjItem3PropertyRevisionsAutoRequest getTestSecuritySubObjItem3PropertyRevisionsAutoRequest)
        {
            string propertyName = getTestSecuritySubObjItem3PropertyRevisionsAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getTestSecuritySubObjItem3PropertyRevisionsAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3PropertyRevisionsInternal(testSecuritySubObjItem3Identity, propertyName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionsInternal(WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.TestSecuritySubObjItem3>(testSecuritySubObjItem3Identity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3Revisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItem3Revisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3RevisionsInternal(testSecuritySubObjItem3Identity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItem3RevisionsInternal(WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testSecuritySubObjItem3Identity.Id));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem3WithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3WithRevision(GetVisualTestSecuritySubObjItem3WithRevisionAutoRequest getVisualTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getVisualTestSecuritySubObjItem3WithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getVisualTestSecuritySubObjItem3WithRevisionAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3WithRevisionInternal(WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecuritySubObjItem3PropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
