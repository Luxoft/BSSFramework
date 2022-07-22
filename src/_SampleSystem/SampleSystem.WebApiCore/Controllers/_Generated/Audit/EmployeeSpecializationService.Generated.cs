namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class EmployeeSpecializationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get EmployeeSpecialization Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionByDateRange(GetEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationPropertyRevisionByDateRangeInternal(employeeSpecializationIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeSpecialization>(employeeSpecializationIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisions(GetEmployeeSpecializationPropertyRevisionsAutoRequest getEmployeeSpecializationPropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeSpecializationPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getEmployeeSpecializationPropertyRevisionsAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationPropertyRevisionsInternal(employeeSpecializationIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeSpecialization>(employeeSpecializationIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeSpecializationRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationRevisionsInternal(employeeSpecializationIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeSpecializationRevisionsInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeSpecializationIdentity.Id));
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeSpecializationWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationWithRevision(GetFullEmployeeSpecializationWithRevisionAutoRequest getFullEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeSpecializationWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getFullEmployeeSpecializationWithRevisionAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeSpecializationWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecializationWithRevision(GetRichEmployeeSpecializationWithRevisionAutoRequest getRichEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getRichEmployeeSpecializationWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getRichEmployeeSpecializationWithRevisionAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecializationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeSpecializationWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecializationWithRevision(GetSimpleEmployeeSpecializationWithRevisionAutoRequest getSimpleEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getSimpleEmployeeSpecializationWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getSimpleEmployeeSpecializationWithRevisionAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecializationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeSpecializationWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecializationWithRevision(GetVisualEmployeeSpecializationWithRevisionAutoRequest getVisualEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getVisualEmployeeSpecializationWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getVisualEmployeeSpecializationWithRevisionAutoRequest.employeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecializationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
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
        public SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
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
        public SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
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
        public SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
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
        public SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
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
        public SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
