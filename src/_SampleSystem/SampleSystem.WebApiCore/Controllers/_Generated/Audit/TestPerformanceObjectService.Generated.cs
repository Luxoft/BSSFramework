namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestPerformanceObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestPerformanceObject (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectWithRevision(GetFullTestPerformanceObjectWithRevisionAutoRequest getFullTestPerformanceObjectWithRevisionAutoRequest)
        {
            long revision = getFullTestPerformanceObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getFullTestPerformanceObjectWithRevisionAutoRequest.TestPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectWithRevisionInternal(testPerformanceObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetObjectByRevision(testPerformanceObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectWithRevision(GetRichTestPerformanceObjectWithRevisionAutoRequest getRichTestPerformanceObjectWithRevisionAutoRequest)
        {
            long revision = getRichTestPerformanceObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getRichTestPerformanceObjectWithRevisionAutoRequest.TestPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestPerformanceObjectWithRevisionInternal(testPerformanceObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetObjectByRevision(testPerformanceObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectWithRevision(GetSimpleTestPerformanceObjectWithRevisionAutoRequest getSimpleTestPerformanceObjectWithRevisionAutoRequest)
        {
            long revision = getSimpleTestPerformanceObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getSimpleTestPerformanceObjectWithRevisionAutoRequest.TestPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectWithRevisionInternal(testPerformanceObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetObjectByRevision(testPerformanceObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPerformanceObjectPropertyRevisionByDateRange(GetTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest getTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest.TestPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestPerformanceObjectPropertyRevisionByDateRangeInternal(testPerformanceObjectIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPerformanceObjectPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestPerformanceObject>(testPerformanceObjectIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestPerformanceObject Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPerformanceObjectPropertyRevisions(GetTestPerformanceObjectPropertyRevisionsAutoRequest getTestPerformanceObjectPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestPerformanceObjectPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getTestPerformanceObjectPropertyRevisionsAutoRequest.TestPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestPerformanceObjectPropertyRevisionsInternal(testPerformanceObjectIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPerformanceObjectPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestPerformanceObject>(testPerformanceObjectIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestPerformanceObject revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestPerformanceObjectRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestPerformanceObjectRevisionsInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestPerformanceObjectRevisionsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testPerformanceObjectIdentity.Id));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectWithRevision(GetVisualTestPerformanceObjectWithRevisionAutoRequest getVisualTestPerformanceObjectWithRevisionAutoRequest)
        {
            long revision = getVisualTestPerformanceObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getVisualTestPerformanceObjectWithRevisionAutoRequest.TestPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectWithRevisionInternal(testPerformanceObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetObjectByRevision(testPerformanceObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestPerformanceObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO TestPerformanceObjectIdentity
        {
            get
            {
                return this.testPerformanceObjectIdentity;
            }
            set
            {
                this.testPerformanceObjectIdentity = value;
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
    public partial class GetRichTestPerformanceObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO TestPerformanceObjectIdentity
        {
            get
            {
                return this.testPerformanceObjectIdentity;
            }
            set
            {
                this.testPerformanceObjectIdentity = value;
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
    public partial class GetSimpleTestPerformanceObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO TestPerformanceObjectIdentity
        {
            get
            {
                return this.testPerformanceObjectIdentity;
            }
            set
            {
                this.testPerformanceObjectIdentity = value;
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
    public partial class GetTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO TestPerformanceObjectIdentity
        {
            get
            {
                return this.testPerformanceObjectIdentity;
            }
            set
            {
                this.testPerformanceObjectIdentity = value;
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
    public partial class GetTestPerformanceObjectPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO TestPerformanceObjectIdentity
        {
            get
            {
                return this.testPerformanceObjectIdentity;
            }
            set
            {
                this.testPerformanceObjectIdentity = value;
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
    public partial class GetVisualTestPerformanceObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO TestPerformanceObjectIdentity
        {
            get
            {
                return this.testPerformanceObjectIdentity;
            }
            set
            {
                this.testPerformanceObjectIdentity = value;
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
