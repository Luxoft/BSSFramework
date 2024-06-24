namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]")]
    public partial class TestRestrictionObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestRestrictionObject (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestRestrictionObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO GetFullTestRestrictionObjectWithRevision(GetFullTestRestrictionObjectWithRevisionAutoRequest getFullTestRestrictionObjectWithRevisionAutoRequest)
        {
            long revision = getFullTestRestrictionObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getFullTestRestrictionObjectWithRevisionAutoRequest.TestRestrictionObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestRestrictionObjectWithRevisionInternal(testRestrictionObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO GetFullTestRestrictionObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestRestrictionObject domainObject = bll.GetObjectByRevision(testRestrictionObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRestrictionObject (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestRestrictionObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectRichDTO GetRichTestRestrictionObjectWithRevision(GetRichTestRestrictionObjectWithRevisionAutoRequest getRichTestRestrictionObjectWithRevisionAutoRequest)
        {
            long revision = getRichTestRestrictionObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getRichTestRestrictionObjectWithRevisionAutoRequest.TestRestrictionObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestRestrictionObjectWithRevisionInternal(testRestrictionObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRestrictionObjectRichDTO GetRichTestRestrictionObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestRestrictionObject domainObject = bll.GetObjectByRevision(testRestrictionObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRestrictionObject (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestRestrictionObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectSimpleDTO GetSimpleTestRestrictionObjectWithRevision(GetSimpleTestRestrictionObjectWithRevisionAutoRequest getSimpleTestRestrictionObjectWithRevisionAutoRequest)
        {
            long revision = getSimpleTestRestrictionObjectWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getSimpleTestRestrictionObjectWithRevisionAutoRequest.TestRestrictionObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestRestrictionObjectWithRevisionInternal(testRestrictionObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestRestrictionObjectSimpleDTO GetSimpleTestRestrictionObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestRestrictionObject domainObject = bll.GetObjectByRevision(testRestrictionObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestRestrictionObject Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestRestrictionObjectPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestRestrictionObjectPropertyRevisionByDateRange(GetTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest getTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest.TestRestrictionObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestRestrictionObjectPropertyRevisionByDateRangeInternal(testRestrictionObjectIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestRestrictionObjectPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestRestrictionObject>(testRestrictionObjectIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestRestrictionObject Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestRestrictionObjectPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestRestrictionObjectPropertyRevisions(GetTestRestrictionObjectPropertyRevisionsAutoRequest getTestRestrictionObjectPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestRestrictionObjectPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getTestRestrictionObjectPropertyRevisionsAutoRequest.TestRestrictionObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestRestrictionObjectPropertyRevisionsInternal(testRestrictionObjectIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestRestrictionObjectPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestRestrictionObject>(testRestrictionObjectIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestRestrictionObject revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestRestrictionObjectRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestRestrictionObjectRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestRestrictionObjectRevisionsInternal(testRestrictionObjectIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestRestrictionObjectRevisionsInternal(SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestRestrictionObjectBLL bll = evaluateData.Context.Logics.TestRestrictionObjectFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testRestrictionObjectIdentity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestRestrictionObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO TestRestrictionObjectIdentity
        {
            get
            {
                return this.testRestrictionObjectIdentity;
            }
            set
            {
                this.testRestrictionObjectIdentity = value;
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
    public partial class GetRichTestRestrictionObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO TestRestrictionObjectIdentity
        {
            get
            {
                return this.testRestrictionObjectIdentity;
            }
            set
            {
                this.testRestrictionObjectIdentity = value;
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
    public partial class GetSimpleTestRestrictionObjectWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO TestRestrictionObjectIdentity
        {
            get
            {
                return this.testRestrictionObjectIdentity;
            }
            set
            {
                this.testRestrictionObjectIdentity = value;
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
    public partial class GetTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO TestRestrictionObjectIdentity
        {
            get
            {
                return this.testRestrictionObjectIdentity;
            }
            set
            {
                this.testRestrictionObjectIdentity = value;
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
    public partial class GetTestRestrictionObjectPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO TestRestrictionObjectIdentity
        {
            get
            {
                return this.testRestrictionObjectIdentity;
            }
            set
            {
                this.testRestrictionObjectIdentity = value;
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
}
