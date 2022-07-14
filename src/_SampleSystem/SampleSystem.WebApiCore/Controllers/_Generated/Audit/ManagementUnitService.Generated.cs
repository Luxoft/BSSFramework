namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class ManagementUnitController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public ManagementUnitController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get ManagementUnit (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullManagementUnitWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitWithRevision(GetFullManagementUnitWithRevisionAutoRequest getFullManagementUnitWithRevisionAutoRequest)
        {
            long revision = getFullManagementUnitWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getFullManagementUnitWithRevisionAutoRequest.managementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullManagementUnitWithRevisionInternal(managementUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitFullDTO GetFullManagementUnitWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetObjectByRevision(managementUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitPropertyRevisionByDateRange(GetManagementUnitPropertyRevisionByDateRangeAutoRequest getManagementUnitPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getManagementUnitPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getManagementUnitPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getManagementUnitPropertyRevisionByDateRangeAutoRequest.managementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetManagementUnitPropertyRevisionByDateRangeInternal(managementUnitIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnit>(managementUnitIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get ManagementUnit Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitPropertyRevisions(GetManagementUnitPropertyRevisionsAutoRequest getManagementUnitPropertyRevisionsAutoRequest)
        {
            string propertyName = getManagementUnitPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getManagementUnitPropertyRevisionsAutoRequest.managementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetManagementUnitPropertyRevisionsInternal(managementUnitIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetManagementUnitPropertyRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ManagementUnit>(managementUnitIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get ManagementUnit revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetManagementUnitRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetManagementUnitRevisionsInternal(managementUnitIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetManagementUnitRevisionsInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(managementUnitIdentity.Id));
        }
        
        /// <summary>
        /// Get ManagementUnit (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichManagementUnitWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitWithRevision(GetRichManagementUnitWithRevisionAutoRequest getRichManagementUnitWithRevisionAutoRequest)
        {
            long revision = getRichManagementUnitWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getRichManagementUnitWithRevisionAutoRequest.managementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichManagementUnitWithRevisionInternal(managementUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitRichDTO GetRichManagementUnitWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetObjectByRevision(managementUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleManagementUnitWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitWithRevision(GetSimpleManagementUnitWithRevisionAutoRequest getSimpleManagementUnitWithRevisionAutoRequest)
        {
            long revision = getSimpleManagementUnitWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getSimpleManagementUnitWithRevisionAutoRequest.managementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleManagementUnitWithRevisionInternal(managementUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitSimpleDTO GetSimpleManagementUnitWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetObjectByRevision(managementUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get ManagementUnit (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualManagementUnitWithRevision")]
        public virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitWithRevision(GetVisualManagementUnitWithRevisionAutoRequest getVisualManagementUnitWithRevisionAutoRequest)
        {
            long revision = getVisualManagementUnitWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity = getVisualManagementUnitWithRevisionAutoRequest.managementUnitIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualManagementUnitWithRevisionInternal(managementUnitIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.ManagementUnitVisualDTO GetVisualManagementUnitWithRevisionInternal(SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IManagementUnitBLL bll = evaluateData.Context.Logics.ManagementUnitFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.ManagementUnit domainObject = bll.GetObjectByRevision(managementUnitIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullManagementUnitWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetManagementUnitPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetManagementUnitPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichManagementUnitWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleManagementUnitWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualManagementUnitWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.ManagementUnitIdentityDTO managementUnitIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
