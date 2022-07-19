namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeeRegistrationTypeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get EmployeeRegistrationType Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRegistrationTypePropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRegistrationTypePropertyRevisionByDateRange(GetEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest getEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest.employeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeRegistrationTypePropertyRevisionByDateRangeInternal(employeeRegistrationTypeIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRegistrationTypePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRegistrationType>(employeeRegistrationTypeIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRegistrationTypePropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRegistrationTypePropertyRevisions(GetEmployeeRegistrationTypePropertyRevisionsAutoRequest getEmployeeRegistrationTypePropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeRegistrationTypePropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getEmployeeRegistrationTypePropertyRevisionsAutoRequest.employeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeRegistrationTypePropertyRevisionsInternal(employeeRegistrationTypeIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRegistrationTypePropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRegistrationType>(employeeRegistrationTypeIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRegistrationTypeRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRegistrationTypeRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeRegistrationTypeRevisionsInternal(employeeRegistrationTypeIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRegistrationTypeRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeRegistrationTypeIdentity.Id));
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRegistrationTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO GetFullEmployeeRegistrationTypeWithRevision(GetFullEmployeeRegistrationTypeWithRevisionAutoRequest getFullEmployeeRegistrationTypeWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeRegistrationTypeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getFullEmployeeRegistrationTypeWithRevisionAutoRequest.employeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRegistrationTypeWithRevisionInternal(employeeRegistrationTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO GetFullEmployeeRegistrationTypeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetObjectByRevision(employeeRegistrationTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRegistrationTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeRichDTO GetRichEmployeeRegistrationTypeWithRevision(GetRichEmployeeRegistrationTypeWithRevisionAutoRequest getRichEmployeeRegistrationTypeWithRevisionAutoRequest)
        {
            long revision = getRichEmployeeRegistrationTypeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getRichEmployeeRegistrationTypeWithRevisionAutoRequest.employeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRegistrationTypeWithRevisionInternal(employeeRegistrationTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeRichDTO GetRichEmployeeRegistrationTypeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetObjectByRevision(employeeRegistrationTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRegistrationTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO GetSimpleEmployeeRegistrationTypeWithRevision(GetSimpleEmployeeRegistrationTypeWithRevisionAutoRequest getSimpleEmployeeRegistrationTypeWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeeRegistrationTypeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getSimpleEmployeeRegistrationTypeWithRevisionAutoRequest.employeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRegistrationTypeWithRevisionInternal(employeeRegistrationTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO GetSimpleEmployeeRegistrationTypeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetObjectByRevision(employeeRegistrationTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRegistrationTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO GetVisualEmployeeRegistrationTypeWithRevision(GetVisualEmployeeRegistrationTypeWithRevisionAutoRequest getVisualEmployeeRegistrationTypeWithRevisionAutoRequest)
        {
            long revision = getVisualEmployeeRegistrationTypeWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getVisualEmployeeRegistrationTypeWithRevisionAutoRequest.employeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRegistrationTypeWithRevisionInternal(employeeRegistrationTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO GetVisualEmployeeRegistrationTypeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetObjectByRevision(employeeRegistrationTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeRegistrationTypePropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullEmployeeRegistrationTypeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichEmployeeRegistrationTypeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleEmployeeRegistrationTypeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualEmployeeRegistrationTypeWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
