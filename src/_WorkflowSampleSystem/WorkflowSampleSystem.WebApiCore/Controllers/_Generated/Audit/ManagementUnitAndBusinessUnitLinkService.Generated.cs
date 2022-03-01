namespace WorkflowSampleSystem.WebApiCore.Controllers.Audit
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class ManagementUnitAndBusinessUnitLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public ManagementUnitAndBusinessUnitLinkController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitAndBusinessUnitLinkWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO GetFullManagementUnitAndBusinessUnitLinkWithRevision(GetFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest getFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest)
        {
            long revision = getFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitAndBusinessUnitLinkWithRevisionInternal(managementUnitAndBusinessUnitLinkIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkFullDTO GetFullManagementUnitAndBusinessUnitLinkWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetObjectByRevision(managementUnitAndBusinessUnitLinkIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRange")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRange(GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest getManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeInternal(managementUnitAndBusinessUnitLinkIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeInternal(WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(managementUnitAndBusinessUnitLinkIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndBusinessUnitLinkPropertyRevisions")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndBusinessUnitLinkPropertyRevisions(GetManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest getManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest)
        {
            string propertyName = getManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndBusinessUnitLinkPropertyRevisionsInternal(managementUnitAndBusinessUnitLinkIdentity, propertyName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitAndBusinessUnitLinkPropertyRevisionsInternal(WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.ManagementUnitAndBusinessUnitLink>(managementUnitAndBusinessUnitLinkIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitAndBusinessUnitLinkRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitAndBusinessUnitLinkRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetManagementUnitAndBusinessUnitLinkRevisionsInternal(managementUnitAndBusinessUnitLinkIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitAndBusinessUnitLinkRevisionsInternal(WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(managementUnitAndBusinessUnitLinkIdentity.Id));
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnitAndBusinessUnitLinkWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkRichDTO GetRichManagementUnitAndBusinessUnitLinkWithRevision(GetRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest getRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest)
        {
            long revision = getRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitAndBusinessUnitLinkWithRevisionInternal(managementUnitAndBusinessUnitLinkIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkRichDTO GetRichManagementUnitAndBusinessUnitLinkWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetObjectByRevision(managementUnitAndBusinessUnitLinkIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnitAndBusinessUnitLink (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitAndBusinessUnitLinkWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO GetSimpleManagementUnitAndBusinessUnitLinkWithRevision(GetSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest getSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest)
        {
            long revision = getSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity = getSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest.managementUnitAndBusinessUnitLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitAndBusinessUnitLinkWithRevisionInternal(managementUnitAndBusinessUnitLinkIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkSimpleDTO GetSimpleManagementUnitAndBusinessUnitLinkWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IManagementUnitAndBusinessUnitLinkBLL bll = evaluateData.Context.Logics.ManagementUnitAndBusinessUnitLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.ManagementUnitAndBusinessUnitLink domainObject = bll.GetObjectByRevision(managementUnitAndBusinessUnitLinkIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetManagementUnitAndBusinessUnitLinkPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetManagementUnitAndBusinessUnitLinkPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleManagementUnitAndBusinessUnitLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.ManagementUnitAndBusinessUnitLinkIdentityDTO managementUnitAndBusinessUnitLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
