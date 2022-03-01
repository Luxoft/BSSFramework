namespace WorkflowSampleSystem.WebApiCore.Controllers.Audit
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class HRDepartmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public HRDepartmentController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get HRDepartment (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullHRDepartmentWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentWithRevision(GetFullHRDepartmentWithRevisionAutoRequest getFullHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getFullHRDepartmentWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getFullHRDepartmentWithRevisionAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.HRDepartmentFullDTO GetFullHRDepartmentWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetHRDepartmentPropertyRevisionByDateRange")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisionByDateRange(GetHRDepartmentPropertyRevisionByDateRangeAutoRequest getHRDepartmentPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getHRDepartmentPropertyRevisionByDateRangeAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetHRDepartmentPropertyRevisionByDateRangeInternal(hRDepartmentIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisionByDateRangeInternal(WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.HRDepartment>(hRDepartmentIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get HRDepartment Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetHRDepartmentPropertyRevisions")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisions(GetHRDepartmentPropertyRevisionsAutoRequest getHRDepartmentPropertyRevisionsAutoRequest)
        {
            string propertyName = getHRDepartmentPropertyRevisionsAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getHRDepartmentPropertyRevisionsAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetHRDepartmentPropertyRevisionsInternal(hRDepartmentIdentity, propertyName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetHRDepartmentPropertyRevisionsInternal(WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.HRDepartment>(hRDepartmentIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get HRDepartment revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetHRDepartmentRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetHRDepartmentRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetHRDepartmentRevisionsInternal(hRDepartmentIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetHRDepartmentRevisionsInternal(WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(hRDepartmentIdentity.Id));
        }
        
        /// <summary>
        /// Get HRDepartment (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichHRDepartmentWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentWithRevision(GetRichHRDepartmentWithRevisionAutoRequest getRichHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getRichHRDepartmentWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getRichHRDepartmentWithRevisionAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.HRDepartmentRichDTO GetRichHRDepartmentWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleHRDepartmentWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentWithRevision(GetSimpleHRDepartmentWithRevisionAutoRequest getSimpleHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getSimpleHRDepartmentWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getSimpleHRDepartmentWithRevisionAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.HRDepartmentSimpleDTO GetSimpleHRDepartmentWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get HRDepartment (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualHRDepartmentWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentWithRevision(GetVisualHRDepartmentWithRevisionAutoRequest getVisualHRDepartmentWithRevisionAutoRequest)
        {
            long revision = getVisualHRDepartmentWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity = getVisualHRDepartmentWithRevisionAutoRequest.hRDepartmentIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualHRDepartmentWithRevisionInternal(hRDepartmentIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.HRDepartmentVisualDTO GetVisualHRDepartmentWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IHRDepartmentBLL bll = evaluateData.Context.Logics.HRDepartmentFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.HRDepartment domainObject = bll.GetObjectByRevision(hRDepartmentIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullHRDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetHRDepartmentPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetHRDepartmentPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichHRDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleHRDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualHRDepartmentWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.HRDepartmentIdentityDTO hRDepartmentIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
