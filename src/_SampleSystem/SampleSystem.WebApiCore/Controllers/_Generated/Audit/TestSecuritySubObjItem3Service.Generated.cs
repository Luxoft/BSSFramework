namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestSecuritySubObjItem3Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestSecuritySubObjItem3WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3WithRevision(GetFullTestSecuritySubObjItem3WithRevisionAutoRequest getFullTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getFullTestSecuritySubObjItem3WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getFullTestSecuritySubObjItem3WithRevisionAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3FullDTO GetFullTestSecuritySubObjItem3WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestSecuritySubObjItem3WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3WithRevision(GetRichTestSecuritySubObjItem3WithRevisionAutoRequest getRichTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getRichTestSecuritySubObjItem3WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getRichTestSecuritySubObjItem3WithRevisionAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3RichDTO GetRichTestSecuritySubObjItem3WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestSecuritySubObjItem3WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3WithRevision(GetSimpleTestSecuritySubObjItem3WithRevisionAutoRequest getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getSimpleTestSecuritySubObjItem3WithRevisionAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3SimpleDTO GetSimpleTestSecuritySubObjItem3WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3PropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionByDateRange(GetTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3PropertyRevisionByDateRangeInternal(testSecuritySubObjItem3Identity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecuritySubObjItem3>(testSecuritySubObjItem3Identity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3PropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisions(GetTestSecuritySubObjItem3PropertyRevisionsAutoRequest getTestSecuritySubObjItem3PropertyRevisionsAutoRequest)
        {
            string propertyName = getTestSecuritySubObjItem3PropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getTestSecuritySubObjItem3PropertyRevisionsAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3PropertyRevisionsInternal(testSecuritySubObjItem3Identity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestSecuritySubObjItem3PropertyRevisionsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestSecuritySubObjItem3>(testSecuritySubObjItem3Identity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestSecuritySubObjItem3Revisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItem3Revisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestSecuritySubObjItem3RevisionsInternal(testSecuritySubObjItem3Identity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestSecuritySubObjItem3RevisionsInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testSecuritySubObjItem3Identity.Id));
        }
        
        /// <summary>
        /// Get TestSecuritySubObjItem3 (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestSecuritySubObjItem3WithRevision")]
        public virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3WithRevision(GetVisualTestSecuritySubObjItem3WithRevisionAutoRequest getVisualTestSecuritySubObjItem3WithRevisionAutoRequest)
        {
            long revision = getVisualTestSecuritySubObjItem3WithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity = getVisualTestSecuritySubObjItem3WithRevisionAutoRequest.testSecuritySubObjItem3Identity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestSecuritySubObjItem3WithRevisionInternal(testSecuritySubObjItem3Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestSecuritySubObjItem3VisualDTO GetVisualTestSecuritySubObjItem3WithRevisionInternal(SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestSecuritySubObjItem3BLL bll = evaluateData.Context.Logics.TestSecuritySubObjItem3Factory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestSecuritySubObjItem3 domainObject = bll.GetObjectByRevision(testSecuritySubObjItem3Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecuritySubObjItem3PropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestSecuritySubObjItem3PropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestSecuritySubObjItem3WithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestSecuritySubObjItem3IdentityDTO testSecuritySubObjItem3Identity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
