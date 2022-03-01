namespace WorkflowSampleSystem.WebApiCore.Controllers.Audit
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class BusinessUnitManagerCommissionLinkController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public BusinessUnitManagerCommissionLinkController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRange")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRange(GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeInternal(businessUnitManagerCommissionLinkIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.BusinessUnitManagerCommissionLink>(businessUnitManagerCommissionLinkIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitManagerCommissionLinkPropertyRevisions")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisions(GetBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest)
        {
            string propertyName = getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetBusinessUnitManagerCommissionLinkPropertyRevisionsInternal(businessUnitManagerCommissionLinkIdentity, propertyName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetBusinessUnitManagerCommissionLinkPropertyRevisionsInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.BusinessUnitManagerCommissionLink>(businessUnitManagerCommissionLinkIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetBusinessUnitManagerCommissionLinkRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitManagerCommissionLinkRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetBusinessUnitManagerCommissionLinkRevisionsInternal(businessUnitManagerCommissionLinkIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetBusinessUnitManagerCommissionLinkRevisionsInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(businessUnitManagerCommissionLinkIdentity.Id));
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullBusinessUnitManagerCommissionLinkWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkFullDTO GetFullBusinessUnitManagerCommissionLinkWithRevision(GetFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullBusinessUnitManagerCommissionLinkWithRevisionInternal(businessUnitManagerCommissionLinkIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkFullDTO GetFullBusinessUnitManagerCommissionLinkWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnitManagerCommissionLink domainObject = bll.GetObjectByRevision(businessUnitManagerCommissionLinkIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichBusinessUnitManagerCommissionLinkWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkRichDTO GetRichBusinessUnitManagerCommissionLinkWithRevision(GetRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichBusinessUnitManagerCommissionLinkWithRevisionInternal(businessUnitManagerCommissionLinkIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkRichDTO GetRichBusinessUnitManagerCommissionLinkWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnitManagerCommissionLink domainObject = bll.GetObjectByRevision(businessUnitManagerCommissionLinkIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get BusinessUnitManagerCommissionLink (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleBusinessUnitManagerCommissionLinkWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkSimpleDTO GetSimpleBusinessUnitManagerCommissionLinkWithRevision(GetSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest)
        {
            long revision = getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity = getSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest.businessUnitManagerCommissionLinkIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleBusinessUnitManagerCommissionLinkWithRevisionInternal(businessUnitManagerCommissionLinkIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkSimpleDTO GetSimpleBusinessUnitManagerCommissionLinkWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.IBusinessUnitManagerCommissionLinkBLL bll = evaluateData.Context.Logics.BusinessUnitManagerCommissionLinkFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.BusinessUnitManagerCommissionLink domainObject = bll.GetObjectByRevision(businessUnitManagerCommissionLinkIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitManagerCommissionLinkPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetBusinessUnitManagerCommissionLinkPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleBusinessUnitManagerCommissionLinkWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.BusinessUnitManagerCommissionLinkIdentityDTO businessUnitManagerCommissionLinkIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
