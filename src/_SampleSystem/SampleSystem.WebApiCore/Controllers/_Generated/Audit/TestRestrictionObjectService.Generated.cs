namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestRestrictionObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get TestRestrictionObject (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestRestrictionObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestRestrictionObjectFullDTO GetFullTestRestrictionObjectWithRevision(GetFullTestRestrictionObjectWithRevisionAutoRequest getFullTestRestrictionObjectWithRevisionAutoRequest)
        {
            long revision = getFullTestRestrictionObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getFullTestRestrictionObjectWithRevisionAutoRequest.testRestrictionObjectIdentity;
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
            long revision = getRichTestRestrictionObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getRichTestRestrictionObjectWithRevisionAutoRequest.testRestrictionObjectIdentity;
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
            long revision = getSimpleTestRestrictionObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getSimpleTestRestrictionObjectWithRevisionAutoRequest.testRestrictionObjectIdentity;
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
            Framework.Core.Period? period = getTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest.testRestrictionObjectIdentity;
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
            string propertyName = getTestRestrictionObjectPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity = getTestRestrictionObjectPropertyRevisionsAutoRequest.testRestrictionObjectIdentity;
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
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestRestrictionObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestRestrictionObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestRestrictionObjectPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestRestrictionObjectPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestRestrictionObjectIdentityDTO testRestrictionObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
}
