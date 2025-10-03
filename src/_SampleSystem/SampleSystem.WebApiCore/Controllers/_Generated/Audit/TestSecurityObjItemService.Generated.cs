namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class TestSecurityObjItemController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecurityObjItem (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO GetFullTestSecurityObjItemWithRevision(GetFullTestSecurityObjItemWithRevisionAutoRequest getFullTestSecurityObjItemWithRevisionAutoRequest)
        {
            long revision = getFullTestSecurityObjItemWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getFullTestSecurityObjItemWithRevisionAutoRequest.TestSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullTestSecurityObjItemWithRevisionInternal(testSecurityObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemFullDTO GetFullTestSecurityObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetObjectByRevision(testSecurityObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemRichDTO GetRichTestSecurityObjItemWithRevision(GetRichTestSecurityObjItemWithRevisionAutoRequest getRichTestSecurityObjItemWithRevisionAutoRequest)
        {
            long revision = getRichTestSecurityObjItemWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getRichTestSecurityObjItemWithRevisionAutoRequest.TestSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichTestSecurityObjItemWithRevisionInternal(testSecurityObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemRichDTO GetRichTestSecurityObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetObjectByRevision(testSecurityObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO GetSimpleTestSecurityObjItemWithRevision(GetSimpleTestSecurityObjItemWithRevisionAutoRequest getSimpleTestSecurityObjItemWithRevisionAutoRequest)
        {
            long revision = getSimpleTestSecurityObjItemWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getSimpleTestSecurityObjItemWithRevisionAutoRequest.TestSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecurityObjItemWithRevisionInternal(testSecurityObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemSimpleDTO GetSimpleTestSecurityObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetObjectByRevision(testSecurityObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecurityObjItemPropertyRevisionByDateRange(GetTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest getTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest.TestSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemPropertyRevisionByDateRangeInternal(testSecurityObjItemIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecurityObjItemPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecurityObjItem>(testSecurityObjItemIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecurityObjItemPropertyRevisions(GetTestSecurityObjItemPropertyRevisionsAutoRequest getTestSecurityObjItemPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestSecurityObjItemPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getTestSecurityObjItemPropertyRevisionsAutoRequest.TestSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemPropertyRevisionsInternal(testSecurityObjItemIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecurityObjItemPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecurityObjItem>(testSecurityObjItemIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestSecurityObjItem revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecurityObjItemRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetTestSecurityObjItemRevisionsInternal(testSecurityObjItemIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecurityObjItemRevisionsInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testSecurityObjItemIdentity.Id));
        }
        
        /// <summary>
        /// Get TestSecurityObjItem (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO GetVisualTestSecurityObjItemWithRevision(GetVisualTestSecurityObjItemWithRevisionAutoRequest getVisualTestSecurityObjItemWithRevisionAutoRequest)
        {
            long revision = getVisualTestSecurityObjItemWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity = getVisualTestSecurityObjItemWithRevisionAutoRequest.TestSecurityObjItemIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualTestSecurityObjItemWithRevisionInternal(testSecurityObjItemIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecurityObjItemVisualDTO GetVisualTestSecurityObjItemWithRevisionInternal(SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecurityObjItemBLL bll = evaluateData.Context.Logics.TestSecurityObjItemFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.TestSecurityObjItem domainObject = bll.GetObjectByRevision(testSecurityObjItemIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestSecurityObjItemWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO TestSecurityObjItemIdentity
        {
            get
            {
                return this.testSecurityObjItemIdentity;
            }
            set
            {
                this.testSecurityObjItemIdentity = value;
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
    public partial class GetRichTestSecurityObjItemWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO TestSecurityObjItemIdentity
        {
            get
            {
                return this.testSecurityObjItemIdentity;
            }
            set
            {
                this.testSecurityObjItemIdentity = value;
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
    public partial class GetSimpleTestSecurityObjItemWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO TestSecurityObjItemIdentity
        {
            get
            {
                return this.testSecurityObjItemIdentity;
            }
            set
            {
                this.testSecurityObjItemIdentity = value;
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
    public partial class GetTestSecurityObjItemPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO TestSecurityObjItemIdentity
        {
            get
            {
                return this.testSecurityObjItemIdentity;
            }
            set
            {
                this.testSecurityObjItemIdentity = value;
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
    public partial class GetTestSecurityObjItemPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO TestSecurityObjItemIdentity
        {
            get
            {
                return this.testSecurityObjItemIdentity;
            }
            set
            {
                this.testSecurityObjItemIdentity = value;
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
    public partial class GetVisualTestSecurityObjItemWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO testSecurityObjItemIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.TestSecurityObjItemIdentityDTO TestSecurityObjItemIdentity
        {
            get
            {
                return this.testSecurityObjItemIdentity;
            }
            set
            {
                this.testSecurityObjItemIdentity = value;
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
