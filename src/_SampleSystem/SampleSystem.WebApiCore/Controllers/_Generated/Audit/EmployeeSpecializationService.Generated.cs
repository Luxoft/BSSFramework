namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]")]
    public partial class EmployeeSpecializationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeSpecialization Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionByDateRange(GetEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest.EmployeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationPropertyRevisionByDateRangeInternal(employeeSpecializationIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeSpecialization>(employeeSpecializationIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisions(GetEmployeeSpecializationPropertyRevisionsAutoRequest getEmployeeSpecializationPropertyRevisionsAutoRequest)
        {
            string propertyName = getEmployeeSpecializationPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getEmployeeSpecializationPropertyRevisionsAutoRequest.EmployeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationPropertyRevisionsInternal(employeeSpecializationIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetEmployeeSpecializationPropertyRevisionsInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.EmployeeSpecialization>(employeeSpecializationIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get EmployeeSpecialization revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetEmployeeSpecializationRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeSpecializationRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetEmployeeSpecializationRevisionsInternal(employeeSpecializationIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetEmployeeSpecializationRevisionsInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(employeeSpecializationIdentity.Id));
        }
        
        /// <summary>
        /// Get EmployeeSpecialization (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeSpecializationWithRevision")]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationWithRevision(GetFullEmployeeSpecializationWithRevisionAutoRequest getFullEmployeeSpecializationWithRevisionAutoRequest)
        {
            long revision = getFullEmployeeSpecializationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getFullEmployeeSpecializationWithRevisionAutoRequest.EmployeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationFullDTO GetFullEmployeeSpecializationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getRichEmployeeSpecializationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getRichEmployeeSpecializationWithRevisionAutoRequest.EmployeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationRichDTO GetRichEmployeeSpecializationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getSimpleEmployeeSpecializationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getSimpleEmployeeSpecializationWithRevisionAutoRequest.EmployeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationSimpleDTO GetSimpleEmployeeSpecializationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getVisualEmployeeSpecializationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity = getVisualEmployeeSpecializationWithRevisionAutoRequest.EmployeeSpecializationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeSpecializationWithRevisionInternal(employeeSpecializationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeSpecializationVisualDTO GetVisualEmployeeSpecializationWithRevisionInternal(SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeSpecializationBLL bll = evaluateData.Context.Logics.EmployeeSpecializationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeSpecialization domainObject = bll.GetObjectByRevision(employeeSpecializationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetEmployeeSpecializationPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO EmployeeSpecializationIdentity
        {
            get
            {
                return this.employeeSpecializationIdentity;
            }
            set
            {
                this.employeeSpecializationIdentity = value;
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
    public partial class GetEmployeeSpecializationPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO EmployeeSpecializationIdentity
        {
            get
            {
                return this.employeeSpecializationIdentity;
            }
            set
            {
                this.employeeSpecializationIdentity = value;
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
    public partial class GetFullEmployeeSpecializationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO EmployeeSpecializationIdentity
        {
            get
            {
                return this.employeeSpecializationIdentity;
            }
            set
            {
                this.employeeSpecializationIdentity = value;
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
    public partial class GetRichEmployeeSpecializationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO EmployeeSpecializationIdentity
        {
            get
            {
                return this.employeeSpecializationIdentity;
            }
            set
            {
                this.employeeSpecializationIdentity = value;
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
    public partial class GetSimpleEmployeeSpecializationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO EmployeeSpecializationIdentity
        {
            get
            {
                return this.employeeSpecializationIdentity;
            }
            set
            {
                this.employeeSpecializationIdentity = value;
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
    public partial class GetVisualEmployeeSpecializationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO employeeSpecializationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.EmployeeSpecializationIdentityDTO EmployeeSpecializationIdentity
        {
            get
            {
                return this.employeeSpecializationIdentity;
            }
            set
            {
                this.employeeSpecializationIdentity = value;
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
