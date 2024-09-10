namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestSecuritySubObjItemController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemFullDTO GetFullTestSecuritySubObjItemWithRevision(GetFullTestSecuritySubObjItemWithRevisionAutoRequest getFullTestSecuritySubObjItemWithRevisionAutoRequest)
        {
            long revision = getFullTestSecuritySubObjItemWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getFullTestSecuritySubObjItemWithRevisionAutoRequest.TestSecuritySubObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItemWithRevisionInternal(testSecuritySubObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemFullDTO GetFullTestSecuritySubObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItemBLL bll = evaluateData.Context.Logics.TestSecuritySubObjItemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem domainObject = bll.GetObjectByRevision(testSecuritySubObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemRichDTO GetRichTestSecuritySubObjItemWithRevision(GetRichTestSecuritySubObjItemWithRevisionAutoRequest getRichTestSecuritySubObjItemWithRevisionAutoRequest)
        {
            long revision = getRichTestSecuritySubObjItemWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getRichTestSecuritySubObjItemWithRevisionAutoRequest.TestSecuritySubObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItemWithRevisionInternal(testSecuritySubObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemRichDTO GetRichTestSecuritySubObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItemBLL bll = evaluateData.Context.Logics.TestSecuritySubObjItemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem domainObject = bll.GetObjectByRevision(testSecuritySubObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemSimpleDTO GetSimpleTestSecuritySubObjItemWithRevision(GetSimpleTestSecuritySubObjItemWithRevisionAutoRequest getSimpleTestSecuritySubObjItemWithRevisionAutoRequest)
        {
            long revision = getSimpleTestSecuritySubObjItemWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getSimpleTestSecuritySubObjItemWithRevisionAutoRequest.TestSecuritySubObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItemWithRevisionInternal(testSecuritySubObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemSimpleDTO GetSimpleTestSecuritySubObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItemBLL bll = evaluateData.Context.Logics.TestSecuritySubObjItemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem domainObject = bll.GetObjectByRevision(testSecuritySubObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItemPropertyRevisionByDateRange(GetTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest getTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest.TestSecuritySubObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItemPropertyRevisionByDateRangeInternal(testSecuritySubObjItemIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItemPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItemBLL bll = evaluateData.Context.Logics.TestSecuritySubObjItemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecuritySubObjItem>(testSecuritySubObjItemIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItemPropertyRevisions(GetTestSecuritySubObjItemPropertyRevisionsAutoRequest getTestSecuritySubObjItemPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestSecuritySubObjItemPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getTestSecuritySubObjItemPropertyRevisionsAutoRequest.TestSecuritySubObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItemPropertyRevisionsInternal(testSecuritySubObjItemIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItemPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItemBLL bll = evaluateData.Context.Logics.TestSecuritySubObjItemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecuritySubObjItem>(testSecuritySubObjItemIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItemRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItemRevisionsInternal(testSecuritySubObjItemIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItemRevisionsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItemBLL bll = evaluateData.Context.Logics.TestSecuritySubObjItemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testSecuritySubObjItemIdentity.Id));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemVisualDTO GetVisualTestSecuritySubObjItemWithRevision(GetVisualTestSecuritySubObjItemWithRevisionAutoRequest getVisualTestSecuritySubObjItemWithRevisionAutoRequest)
        {
            long revision = getVisualTestSecuritySubObjItemWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getVisualTestSecuritySubObjItemWithRevisionAutoRequest.TestSecuritySubObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItemWithRevisionInternal(testSecuritySubObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemVisualDTO GetVisualTestSecuritySubObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItemBLL bll = evaluateData.Context.Logics.TestSecuritySubObjItemFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecuritySubObjItem domainObject = bll.GetObjectByRevision(testSecuritySubObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestSecuritySubObjItemWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO TestSecuritySubObjItemIdentity
        {
            get
            {
                return this.testSecuritySubObjItemIdentity;
            }
            set
            {
                this.testSecuritySubObjItemIdentity = value;
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
    public partial class GetRichTestSecuritySubObjItemWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO TestSecuritySubObjItemIdentity
        {
            get
            {
                return this.testSecuritySubObjItemIdentity;
            }
            set
            {
                this.testSecuritySubObjItemIdentity = value;
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
    public partial class GetSimpleTestSecuritySubObjItemWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO TestSecuritySubObjItemIdentity
        {
            get
            {
                return this.testSecuritySubObjItemIdentity;
            }
            set
            {
                this.testSecuritySubObjItemIdentity = value;
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
    public partial class GetTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO TestSecuritySubObjItemIdentity
        {
            get
            {
                return this.testSecuritySubObjItemIdentity;
            }
            set
            {
                this.testSecuritySubObjItemIdentity = value;
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
    public partial class GetTestSecuritySubObjItemPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO TestSecuritySubObjItemIdentity
        {
            get
            {
                return this.testSecuritySubObjItemIdentity;
            }
            set
            {
                this.testSecuritySubObjItemIdentity = value;
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
    public partial class GetVisualTestSecuritySubObjItemWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO TestSecuritySubObjItemIdentity
        {
            get
            {
                return this.testSecuritySubObjItemIdentity;
            }
            set
            {
                this.testSecuritySubObjItemIdentity = value;
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
