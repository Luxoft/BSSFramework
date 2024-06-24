namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]")]
    public partial class InformationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Information (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullInformationWithRevision")]
        public virtual SampleSystem.Generated.DTO.InformationFullDTO GetFullInformationWithRevision(GetFullInformationWithRevisionAutoRequest getFullInformationWithRevisionAutoRequest)
        {
            long revision = getFullInformationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity = getFullInformationWithRevisionAutoRequest.InformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullInformationWithRevisionInternal(informationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.InformationFullDTO GetFullInformationWithRevisionInternal(SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IInformationBLL bll = evaluateData.Context.Logics.InformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Information domainObject = bll.GetObjectByRevision(informationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Information Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetInformationPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetInformationPropertyRevisionByDateRange(GetInformationPropertyRevisionByDateRangeAutoRequest getInformationPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getInformationPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getInformationPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity = getInformationPropertyRevisionByDateRangeAutoRequest.InformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetInformationPropertyRevisionByDateRangeInternal(informationIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetInformationPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IInformationBLL bll = evaluateData.Context.Logics.InformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Information>(informationIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get Information Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetInformationPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetInformationPropertyRevisions(GetInformationPropertyRevisionsAutoRequest getInformationPropertyRevisionsAutoRequest)
        {
            string propertyName = getInformationPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity = getInformationPropertyRevisionsAutoRequest.InformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetInformationPropertyRevisionsInternal(informationIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetInformationPropertyRevisionsInternal(SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IInformationBLL bll = evaluateData.Context.Logics.InformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Information>(informationIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get Information revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetInformationRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetInformationRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetInformationRevisionsInternal(informationIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetInformationRevisionsInternal(SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IInformationBLL bll = evaluateData.Context.Logics.InformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(informationIdentity.Id));
        }
        
        /// <summary>
        /// Get Information (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichInformationWithRevision")]
        public virtual SampleSystem.Generated.DTO.InformationRichDTO GetRichInformationWithRevision(GetRichInformationWithRevisionAutoRequest getRichInformationWithRevisionAutoRequest)
        {
            long revision = getRichInformationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity = getRichInformationWithRevisionAutoRequest.InformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichInformationWithRevisionInternal(informationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.InformationRichDTO GetRichInformationWithRevisionInternal(SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IInformationBLL bll = evaluateData.Context.Logics.InformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Information domainObject = bll.GetObjectByRevision(informationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Information (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleInformationWithRevision")]
        public virtual SampleSystem.Generated.DTO.InformationSimpleDTO GetSimpleInformationWithRevision(GetSimpleInformationWithRevisionAutoRequest getSimpleInformationWithRevisionAutoRequest)
        {
            long revision = getSimpleInformationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity = getSimpleInformationWithRevisionAutoRequest.InformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleInformationWithRevisionInternal(informationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.InformationSimpleDTO GetSimpleInformationWithRevisionInternal(SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IInformationBLL bll = evaluateData.Context.Logics.InformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Information domainObject = bll.GetObjectByRevision(informationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Information (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualInformationWithRevision")]
        public virtual SampleSystem.Generated.DTO.InformationVisualDTO GetVisualInformationWithRevision(GetVisualInformationWithRevisionAutoRequest getVisualInformationWithRevisionAutoRequest)
        {
            long revision = getVisualInformationWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity = getVisualInformationWithRevisionAutoRequest.InformationIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualInformationWithRevisionInternal(informationIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.InformationVisualDTO GetVisualInformationWithRevisionInternal(SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IInformationBLL bll = evaluateData.Context.Logics.InformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Information domainObject = bll.GetObjectByRevision(informationIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullInformationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.InformationIdentityDTO InformationIdentity
        {
            get
            {
                return this.informationIdentity;
            }
            set
            {
                this.informationIdentity = value;
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
    public partial class GetInformationPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.InformationIdentityDTO InformationIdentity
        {
            get
            {
                return this.informationIdentity;
            }
            set
            {
                this.informationIdentity = value;
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
    public partial class GetInformationPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.InformationIdentityDTO InformationIdentity
        {
            get
            {
                return this.informationIdentity;
            }
            set
            {
                this.informationIdentity = value;
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
    public partial class GetRichInformationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.InformationIdentityDTO InformationIdentity
        {
            get
            {
                return this.informationIdentity;
            }
            set
            {
                this.informationIdentity = value;
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
    public partial class GetSimpleInformationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.InformationIdentityDTO InformationIdentity
        {
            get
            {
                return this.informationIdentity;
            }
            set
            {
                this.informationIdentity = value;
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
    public partial class GetVisualInformationWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.InformationIdentityDTO informationIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.InformationIdentityDTO InformationIdentity
        {
            get
            {
                return this.informationIdentity;
            }
            set
            {
                this.informationIdentity = value;
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
