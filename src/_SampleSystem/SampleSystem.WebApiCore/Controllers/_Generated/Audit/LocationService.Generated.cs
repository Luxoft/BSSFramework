namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]")]
    public partial class LocationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Location (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullLocationWithRevision")]
        public virtual SampleSystem.Generated.DTO.LocationFullDTO GetFullLocationWithRevision(GetFullLocationWithRevisionAutoRequest getFullLocationWithRevisionAutoRequest)
        {
            long revision = getFullLocationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity = getFullLocationWithRevisionAutoRequest.LocationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullLocationWithRevisionInternal(locationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationFullDTO GetFullLocationWithRevisionInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = bll.GetObjectByRevision(locationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetLocationPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetLocationPropertyRevisionByDateRange(GetLocationPropertyRevisionByDateRangeAutoRequest getLocationPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getLocationPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getLocationPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity = getLocationPropertyRevisionByDateRangeAutoRequest.LocationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetLocationPropertyRevisionByDateRangeInternal(locationIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetLocationPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Location>(locationIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get Location Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetLocationPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetLocationPropertyRevisions(GetLocationPropertyRevisionsAutoRequest getLocationPropertyRevisionsAutoRequest)
        {
            string propertyName = getLocationPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity = getLocationPropertyRevisionsAutoRequest.LocationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetLocationPropertyRevisionsInternal(locationIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetLocationPropertyRevisionsInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Location>(locationIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get Location revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetLocationRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetLocationRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetLocationRevisionsInternal(locationIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetLocationRevisionsInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(locationIdentity.Id));
        }
        
        /// <summary>
        /// Get Location (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichLocationWithRevision")]
        public virtual SampleSystem.Generated.DTO.LocationRichDTO GetRichLocationWithRevision(GetRichLocationWithRevisionAutoRequest getRichLocationWithRevisionAutoRequest)
        {
            long revision = getRichLocationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity = getRichLocationWithRevisionAutoRequest.LocationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichLocationWithRevisionInternal(locationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationRichDTO GetRichLocationWithRevisionInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = bll.GetObjectByRevision(locationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleLocationWithRevision")]
        public virtual SampleSystem.Generated.DTO.LocationSimpleDTO GetSimpleLocationWithRevision(GetSimpleLocationWithRevisionAutoRequest getSimpleLocationWithRevisionAutoRequest)
        {
            long revision = getSimpleLocationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity = getSimpleLocationWithRevisionAutoRequest.LocationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleLocationWithRevisionInternal(locationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationSimpleDTO GetSimpleLocationWithRevisionInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = bll.GetObjectByRevision(locationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Location (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualLocationWithRevision")]
        public virtual SampleSystem.Generated.DTO.LocationVisualDTO GetVisualLocationWithRevision(GetVisualLocationWithRevisionAutoRequest getVisualLocationWithRevisionAutoRequest)
        {
            long revision = getVisualLocationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity = getVisualLocationWithRevisionAutoRequest.LocationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualLocationWithRevisionInternal(locationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.LocationVisualDTO GetVisualLocationWithRevisionInternal(SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ILocationBLL bll = evaluateData.Context.Logics.LocationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Location domainObject = bll.GetObjectByRevision(locationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullLocationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.LocationIdentityDTO LocationIdentity
        {
            get
            {
                return this.locationIdentity;
            }
            set
            {
                this.locationIdentity = value;
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
    public partial class GetLocationPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.LocationIdentityDTO LocationIdentity
        {
            get
            {
                return this.locationIdentity;
            }
            set
            {
                this.locationIdentity = value;
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
    public partial class GetLocationPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.LocationIdentityDTO LocationIdentity
        {
            get
            {
                return this.locationIdentity;
            }
            set
            {
                this.locationIdentity = value;
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
    public partial class GetRichLocationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.LocationIdentityDTO LocationIdentity
        {
            get
            {
                return this.locationIdentity;
            }
            set
            {
                this.locationIdentity = value;
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
    public partial class GetSimpleLocationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.LocationIdentityDTO LocationIdentity
        {
            get
            {
                return this.locationIdentity;
            }
            set
            {
                this.locationIdentity = value;
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
    public partial class GetVisualLocationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.LocationIdentityDTO locationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.LocationIdentityDTO LocationIdentity
        {
            get
            {
                return this.locationIdentity;
            }
            set
            {
                this.locationIdentity = value;
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
