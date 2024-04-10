namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestSecuritySubObjItemController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItemWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemFullDTO GetFullTestSecuritySubObjItemWithRevision(GetFullTestSecuritySubObjItemWithRevisionAutoRequest getFullTestSecuritySubObjItemWithRevisionAutoRequest)
        {
            long revision = getFullTestSecuritySubObjItemWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getFullTestSecuritySubObjItemWithRevisionAutoRequest.testSecuritySubObjItemIdentity;
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecuritySubObjItemWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemRichDTO GetRichTestSecuritySubObjItemWithRevision(GetRichTestSecuritySubObjItemWithRevisionAutoRequest getRichTestSecuritySubObjItemWithRevisionAutoRequest)
        {
            long revision = getRichTestSecuritySubObjItemWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getRichTestSecuritySubObjItemWithRevisionAutoRequest.testSecuritySubObjItemIdentity;
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItemWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemSimpleDTO GetSimpleTestSecuritySubObjItemWithRevision(GetSimpleTestSecuritySubObjItemWithRevisionAutoRequest getSimpleTestSecuritySubObjItemWithRevisionAutoRequest)
        {
            long revision = getSimpleTestSecuritySubObjItemWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getSimpleTestSecuritySubObjItemWithRevisionAutoRequest.testSecuritySubObjItemIdentity;
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItemPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItemPropertyRevisionByDateRange(GetTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest getTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest.testSecuritySubObjItemIdentity;
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItemPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItemPropertyRevisions(GetTestSecuritySubObjItemPropertyRevisionsAutoRequest getTestSecuritySubObjItemPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestSecuritySubObjItemPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getTestSecuritySubObjItemPropertyRevisionsAutoRequest.testSecuritySubObjItemIdentity;
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItemRevisions")]
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
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItemWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItemVisualDTO GetVisualTestSecuritySubObjItemWithRevision(GetVisualTestSecuritySubObjItemWithRevisionAutoRequest getVisualTestSecuritySubObjItemWithRevisionAutoRequest)
        {
            long revision = getVisualTestSecuritySubObjItemWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity = getVisualTestSecuritySubObjItemWithRevisionAutoRequest.testSecuritySubObjItemIdentity;
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
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestSecuritySubObjItemWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestSecuritySubObjItemWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecuritySubObjItemPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecuritySubObjItemPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestSecuritySubObjItemWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItemIdentityDTO testSecuritySubObjItemIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
