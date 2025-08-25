namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestPlainAuthObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestPlainAuthObject (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO GetFullTestPlainAuthObjectWithRevision(GetFullTestPlainAuthObjectWithRevisionAutoRequest getFullTestPlainAuthObjectWithRevisionAutoRequest)
        {
            long revision = getFullTestPlainAuthObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getFullTestPlainAuthObjectWithRevisionAutoRequest.TestPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPlainAuthObjectWithRevisionInternal(testPlainAuthObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO GetFullTestPlainAuthObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetObjectByRevision(testPlainAuthObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectRichDTO GetRichTestPlainAuthObjectWithRevision(GetRichTestPlainAuthObjectWithRevisionAutoRequest getRichTestPlainAuthObjectWithRevisionAutoRequest)
        {
            long revision = getRichTestPlainAuthObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getRichTestPlainAuthObjectWithRevisionAutoRequest.TestPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestPlainAuthObjectWithRevisionInternal(testPlainAuthObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectRichDTO GetRichTestPlainAuthObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetObjectByRevision(testPlainAuthObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO GetSimpleTestPlainAuthObjectWithRevision(GetSimpleTestPlainAuthObjectWithRevisionAutoRequest getSimpleTestPlainAuthObjectWithRevisionAutoRequest)
        {
            long revision = getSimpleTestPlainAuthObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getSimpleTestPlainAuthObjectWithRevisionAutoRequest.TestPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPlainAuthObjectWithRevisionInternal(testPlainAuthObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO GetSimpleTestPlainAuthObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetObjectByRevision(testPlainAuthObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPlainAuthObjectPropertyRevisionByDateRange(GetTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest getTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest.TestPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestPlainAuthObjectPropertyRevisionByDateRangeInternal(testPlainAuthObjectIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPlainAuthObjectPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestPlainAuthObject>(testPlainAuthObjectIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPlainAuthObjectPropertyRevisions(GetTestPlainAuthObjectPropertyRevisionsAutoRequest getTestPlainAuthObjectPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestPlainAuthObjectPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getTestPlainAuthObjectPropertyRevisionsAutoRequest.TestPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestPlainAuthObjectPropertyRevisionsInternal(testPlainAuthObjectIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPlainAuthObjectPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestPlainAuthObject>(testPlainAuthObjectIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestPlainAuthObjectRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestPlainAuthObjectRevisionsInternal(testPlainAuthObjectIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestPlainAuthObjectRevisionsInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testPlainAuthObjectIdentity.Id));
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO GetVisualTestPlainAuthObjectWithRevision(GetVisualTestPlainAuthObjectWithRevisionAutoRequest getVisualTestPlainAuthObjectWithRevisionAutoRequest)
        {
            long revision = getVisualTestPlainAuthObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getVisualTestPlainAuthObjectWithRevisionAutoRequest.TestPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPlainAuthObjectWithRevisionInternal(testPlainAuthObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO GetVisualTestPlainAuthObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetObjectByRevision(testPlainAuthObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestPlainAuthObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO TestPlainAuthObjectIdentity
        {
            get
            {
                return this.testPlainAuthObjectIdentity;
            }
            set
            {
                this.testPlainAuthObjectIdentity = value;
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
    public partial class GetRichTestPlainAuthObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO TestPlainAuthObjectIdentity
        {
            get
            {
                return this.testPlainAuthObjectIdentity;
            }
            set
            {
                this.testPlainAuthObjectIdentity = value;
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
    public partial class GetSimpleTestPlainAuthObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO TestPlainAuthObjectIdentity
        {
            get
            {
                return this.testPlainAuthObjectIdentity;
            }
            set
            {
                this.testPlainAuthObjectIdentity = value;
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
    public partial class GetTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO TestPlainAuthObjectIdentity
        {
            get
            {
                return this.testPlainAuthObjectIdentity;
            }
            set
            {
                this.testPlainAuthObjectIdentity = value;
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
    public partial class GetTestPlainAuthObjectPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO TestPlainAuthObjectIdentity
        {
            get
            {
                return this.testPlainAuthObjectIdentity;
            }
            set
            {
                this.testPlainAuthObjectIdentity = value;
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
    public partial class GetVisualTestPlainAuthObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO TestPlainAuthObjectIdentity
        {
            get
            {
                return this.testPlainAuthObjectIdentity;
            }
            set
            {
                this.testPlainAuthObjectIdentity = value;
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
