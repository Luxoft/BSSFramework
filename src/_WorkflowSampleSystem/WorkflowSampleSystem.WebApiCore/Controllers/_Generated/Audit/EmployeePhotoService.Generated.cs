namespace WorkflowSampleSystem.WebApiCore.Controllers.Audit
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeePhotoController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public EmployeePhotoController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Get EmployeePhoto Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeePhotoPropertyRevisionByDateRange")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePhotoPropertyRevisionByDateRange(GetEmployeePhotoPropertyRevisionByDateRangeAutoRequest getEmployeePhotoPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeePhotoPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getEmployeePhotoPropertyRevisionByDateRangeAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getEmployeePhotoPropertyRevisionByDateRangeAutoRequest.employeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeePhotoPropertyRevisionByDateRangeInternal(employeePhotoIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePhotoPropertyRevisionByDateRangeInternal(WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.EmployeePhoto>(employeePhotoIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeePhoto Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeePhotoPropertyRevisions")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePhotoPropertyRevisions(GetEmployeePhotoPropertyRevisionsAutoRequest getEmployeePhotoPropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeePhotoPropertyRevisionsAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getEmployeePhotoPropertyRevisionsAutoRequest.employeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeePhotoPropertyRevisionsInternal(employeePhotoIdentity, propertyName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetEmployeePhotoPropertyRevisionsInternal(WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.EmployeePhoto>(employeePhotoIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeePhoto revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeePhotoRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeePhotoRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeePhotoRevisionsInternal(employeePhotoIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeePhotoRevisionsInternal(WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeePhotoIdentity.Id));
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get EmployeePhoto (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeePhotoWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeePhotoFullDTO GetFullEmployeePhotoWithRevision(GetFullEmployeePhotoWithRevisionAutoRequest getFullEmployeePhotoWithRevisionAutoRequest)
        {
            long revision = getFullEmployeePhotoWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getFullEmployeePhotoWithRevisionAutoRequest.employeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeePhotoWithRevisionInternal(employeePhotoIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeePhotoFullDTO GetFullEmployeePhotoWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeePhoto domainObject = bll.GetObjectByRevision(employeePhotoIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhoto (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeePhotoWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeePhotoRichDTO GetRichEmployeePhotoWithRevision(GetRichEmployeePhotoWithRevisionAutoRequest getRichEmployeePhotoWithRevisionAutoRequest)
        {
            long revision = getRichEmployeePhotoWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getRichEmployeePhotoWithRevisionAutoRequest.employeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeePhotoWithRevisionInternal(employeePhotoIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeePhotoRichDTO GetRichEmployeePhotoWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeePhoto domainObject = bll.GetObjectByRevision(employeePhotoIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeePhoto (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeePhotoWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeePhotoSimpleDTO GetSimpleEmployeePhotoWithRevision(GetSimpleEmployeePhotoWithRevisionAutoRequest getSimpleEmployeePhotoWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeePhotoWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity = getSimpleEmployeePhotoWithRevisionAutoRequest.employeePhotoIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeePhotoWithRevisionInternal(employeePhotoIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeePhotoSimpleDTO GetSimpleEmployeePhotoWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeePhotoBLL bll = evaluateData.Context.Logics.EmployeePhotoFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeePhoto domainObject = bll.GetObjectByRevision(employeePhotoIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeePhotoPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeePhotoPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullEmployeePhotoWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichEmployeePhotoWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleEmployeePhotoWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeePhotoIdentityDTO employeePhotoIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
