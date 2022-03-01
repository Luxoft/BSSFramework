namespace WorkflowSampleSystem.WebApiCore.Controllers.Audit
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeeSpecializationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public EmployeeSpecializationController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Get EmployeeSpecialization Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationPropertyRevisionByDateRange")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionByDateRange(GetEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationPropertyRevisionByDateRangeInternal(employeeSpecializationIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionByDateRangeInternal(WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.EmployeeSpecialization>(employeeSpecializationIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationPropertyRevisions")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisions(GetEmployeeSpecializationPropertyRevisionsAutoRequest getEmployeeSpecializationPropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeSpecializationPropertyRevisionsAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getEmployeeSpecializationPropertyRevisionsAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationPropertyRevisionsInternal(employeeSpecializationIdentity, propertyName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionsInternal(WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.EmployeeSpecialization>(employeeSpecializationIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeSpecializationRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationRevisionsInternal(employeeSpecializationIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeSpecializationRevisionsInternal(WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeSpecializationIdentity.Id));
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeSpecializationWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationWithRevision(GetFullEmployeeSpecializationWithRevisionAutoRequest getFullEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeSpecializationWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getFullEmployeeSpecializationWithRevisionAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeSpecializationWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecializationWithRevision(GetRichEmployeeSpecializationWithRevisionAutoRequest getRichEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getRichEmployeeSpecializationWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getRichEmployeeSpecializationWithRevisionAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecializationWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeSpecializationWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecializationWithRevision(GetSimpleEmployeeSpecializationWithRevisionAutoRequest getSimpleEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeeSpecializationWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getSimpleEmployeeSpecializationWithRevisionAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecializationWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeSpecializationWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecializationWithRevision(GetVisualEmployeeSpecializationWithRevisionAutoRequest getVisualEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getVisualEmployeeSpecializationWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getVisualEmployeeSpecializationWithRevisionAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecializationWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeSpecializationPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullEmployeeSpecializationWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichEmployeeSpecializationWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleEmployeeSpecializationWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualEmployeeSpecializationWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
