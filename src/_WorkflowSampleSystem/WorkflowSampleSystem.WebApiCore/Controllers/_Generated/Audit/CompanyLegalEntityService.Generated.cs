namespace WorkflowSampleSystem.WebApiCore.Controllers.Audit
{
    using WorkflowSampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class CompanyLegalEntityController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext>, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>>
    {
        
        public CompanyLegalEntityController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Get CompanyLegalEntity Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCompanyLegalEntityPropertyRevisionByDateRange")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionByDateRange(GetCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetCompanyLegalEntityPropertyRevisionByDateRangeInternal(companyLegalEntityIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionByDateRangeInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.CompanyLegalEntity>(companyLegalEntityIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCompanyLegalEntityPropertyRevisions")]
        public virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisions(GetCompanyLegalEntityPropertyRevisionsAutoRequest getCompanyLegalEntityPropertyRevisionsAutoRequest)
        {
            string propertyName = getCompanyLegalEntityPropertyRevisionsAutoRequest.propertyName;
            WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getCompanyLegalEntityPropertyRevisionsAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetCompanyLegalEntityPropertyRevisionsInternal(companyLegalEntityIdentity, propertyName, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO GetCompanyLegalEntityPropertyRevisionsInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLFactoryContainer, WorkflowSampleSystem.BLL.IWorkflowSampleSystemSecurityService, WorkflowSampleSystem.WorkflowSampleSystemSecurityOperationCode, WorkflowSampleSystem.Domain.PersistentDomainObjectBase, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemDomainObjectPropertiesRevisionDTO, WorkflowSampleSystem.Generated.DTO.WorkflowSampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<WorkflowSampleSystem.Domain.CompanyLegalEntity>(companyLegalEntityIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetCompanyLegalEntityRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetCompanyLegalEntityRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetCompanyLegalEntityRevisionsInternal(companyLegalEntityIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetCompanyLegalEntityRevisionsInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(companyLegalEntityIdentity.Id));
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService>(session, context, new WorkflowSampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullCompanyLegalEntityWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityWithRevision(GetFullCompanyLegalEntityWithRevisionAutoRequest getFullCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getFullCompanyLegalEntityWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getFullCompanyLegalEntityWithRevisionAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityFullDTO GetFullCompanyLegalEntityWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichCompanyLegalEntityWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityWithRevision(GetRichCompanyLegalEntityWithRevisionAutoRequest getRichCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getRichCompanyLegalEntityWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getRichCompanyLegalEntityWithRevisionAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityRichDTO GetRichCompanyLegalEntityWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleCompanyLegalEntityWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityWithRevision(GetSimpleCompanyLegalEntityWithRevisionAutoRequest getSimpleCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getSimpleCompanyLegalEntityWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getSimpleCompanyLegalEntityWithRevisionAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntitySimpleDTO GetSimpleCompanyLegalEntityWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get CompanyLegalEntity (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualCompanyLegalEntityWithRevision")]
        public virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityWithRevision(GetVisualCompanyLegalEntityWithRevisionAutoRequest getVisualCompanyLegalEntityWithRevisionAutoRequest)
        {
            long revision = getVisualCompanyLegalEntityWithRevisionAutoRequest.revision;
            WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity = getVisualCompanyLegalEntityWithRevisionAutoRequest.companyLegalEntityIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualCompanyLegalEntityWithRevisionInternal(companyLegalEntityIdentity, revision, evaluateData));
        }
        
        protected virtual WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityVisualDTO GetVisualCompanyLegalEntityWithRevisionInternal(WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<WorkflowSampleSystem.BLL.IWorkflowSampleSystemBLLContext, WorkflowSampleSystem.Generated.DTO.IWorkflowSampleSystemDTOMappingService> evaluateData)
        {
            WorkflowSampleSystem.BLL.ICompanyLegalEntityBLL bll = evaluateData.Context.Logics.CompanyLegalEntityFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            WorkflowSampleSystem.Domain.CompanyLegalEntity domainObject = bll.GetObjectByRevision(companyLegalEntityIdentity.Id, revision);
            return WorkflowSampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetCompanyLegalEntityPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetCompanyLegalEntityPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullCompanyLegalEntityWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichCompanyLegalEntityWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleCompanyLegalEntityWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualCompanyLegalEntityWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public WorkflowSampleSystem.Generated.DTO.CompanyLegalEntityIdentityDTO companyLegalEntityIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
