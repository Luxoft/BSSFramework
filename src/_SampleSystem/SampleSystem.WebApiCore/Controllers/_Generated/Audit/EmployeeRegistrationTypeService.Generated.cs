namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class EmployeeRegistrationTypeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeRegistrationType Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRegistrationTypePropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRegistrationTypePropertyRevisionByDateRange(GetEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest getEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest.EmployeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRegistrationTypePropertyRevisionByDateRangeInternal(employeeRegistrationTypeIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRegistrationTypePropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRegistrationType>(employeeRegistrationTypeIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRegistrationTypePropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRegistrationTypePropertyRevisions(GetEmployeeRegistrationTypePropertyRevisionsAutoRequest getEmployeeRegistrationTypePropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeRegistrationTypePropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getEmployeeRegistrationTypePropertyRevisionsAutoRequest.EmployeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRegistrationTypePropertyRevisionsInternal(employeeRegistrationTypeIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeRegistrationTypePropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeRegistrationType>(employeeRegistrationTypeIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeRegistrationTypeRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRegistrationTypeRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeRegistrationTypeRevisionsInternal(employeeRegistrationTypeIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeRegistrationTypeRevisionsInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeRegistrationTypeIdentity.Id));
        }
        
        /// <summary>
        /// Get EmployeeRegistrationType (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRegistrationTypeWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO GetFullEmployeeRegistrationTypeWithRevision(GetFullEmployeeRegistrationTypeWithRevisionAutoRequest getFullEmployeeRegistrationTypeWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeRegistrationTypeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getFullEmployeeRegistrationTypeWithRevisionAutoRequest.EmployeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRegistrationTypeWithRevisionInternal(employeeRegistrationTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeFullDTO GetFullEmployeeRegistrationTypeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getRichEmployeeRegistrationTypeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getRichEmployeeRegistrationTypeWithRevisionAutoRequest.EmployeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRegistrationTypeWithRevisionInternal(employeeRegistrationTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeRichDTO GetRichEmployeeRegistrationTypeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getSimpleEmployeeRegistrationTypeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getSimpleEmployeeRegistrationTypeWithRevisionAutoRequest.EmployeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRegistrationTypeWithRevisionInternal(employeeRegistrationTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeSimpleDTO GetSimpleEmployeeRegistrationTypeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getVisualEmployeeRegistrationTypeWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity = getVisualEmployeeRegistrationTypeWithRevisionAutoRequest.EmployeeRegistrationTypeIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRegistrationTypeWithRevisionInternal(employeeRegistrationTypeIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeVisualDTO GetVisualEmployeeRegistrationTypeWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRegistrationTypeBLL bll = evaluateData.Context.Logics.EmployeeRegistrationTypeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRegistrationType domainObject = bll.GetObjectByRevision(employeeRegistrationTypeIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeRegistrationTypePropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO EmployeeRegistrationTypeIdentity
        {
            get
            {
                return this.employeeRegistrationTypeIdentity;
            }
            set
            {
                this.employeeRegistrationTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual string PropertyName
        {
            get
            {
                return this.propertyName;
            }
            set
            {
                this.propertyName = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public virtual Framework.Core.Period? Period
        {
            get
            {
                return this.period;
            }
            set
            {
                this.period = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeRegistrationTypePropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO EmployeeRegistrationTypeIdentity
        {
            get
            {
                return this.employeeRegistrationTypeIdentity;
            }
            set
            {
                this.employeeRegistrationTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual string PropertyName
        {
            get
            {
                return this.propertyName;
            }
            set
            {
                this.propertyName = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullEmployeeRegistrationTypeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO EmployeeRegistrationTypeIdentity
        {
            get
            {
                return this.employeeRegistrationTypeIdentity;
            }
            set
            {
                this.employeeRegistrationTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual long Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichEmployeeRegistrationTypeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO EmployeeRegistrationTypeIdentity
        {
            get
            {
                return this.employeeRegistrationTypeIdentity;
            }
            set
            {
                this.employeeRegistrationTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual long Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleEmployeeRegistrationTypeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO EmployeeRegistrationTypeIdentity
        {
            get
            {
                return this.employeeRegistrationTypeIdentity;
            }
            set
            {
                this.employeeRegistrationTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual long Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualEmployeeRegistrationTypeWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO employeeRegistrationTypeIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeRegistrationTypeIdentityDTO EmployeeRegistrationTypeIdentity
        {
            get
            {
                return this.employeeRegistrationTypeIdentity;
            }
            set
            {
                this.employeeRegistrationTypeIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public virtual long Revision
        {
            get
            {
                return this.revision;
            }
            set
            {
                this.revision = value;
            }
        }
    }
}
