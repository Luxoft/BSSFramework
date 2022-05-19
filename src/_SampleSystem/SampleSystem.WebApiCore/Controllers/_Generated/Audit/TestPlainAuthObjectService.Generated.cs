namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestPlainAuthObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public TestPlainAuthObjectController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPlainAuthObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO GetFullTestPlainAuthObjectWithRevision(GetFullTestPlainAuthObjectWithRevisionAutoRequest getFullTestPlainAuthObjectWithRevisionAutoRequest)
        {
            long revision = getFullTestPlainAuthObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getFullTestPlainAuthObjectWithRevisionAutoRequest.testPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestPlainAuthObjectWithRevisionInternal(testPlainAuthObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectFullDTO GetFullTestPlainAuthObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetObjectByRevision(testPlainAuthObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestPlainAuthObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectRichDTO GetRichTestPlainAuthObjectWithRevision(GetRichTestPlainAuthObjectWithRevisionAutoRequest getRichTestPlainAuthObjectWithRevisionAutoRequest)
        {
            long revision = getRichTestPlainAuthObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getRichTestPlainAuthObjectWithRevisionAutoRequest.testPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestPlainAuthObjectWithRevisionInternal(testPlainAuthObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectRichDTO GetRichTestPlainAuthObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetObjectByRevision(testPlainAuthObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPlainAuthObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO GetSimpleTestPlainAuthObjectWithRevision(GetSimpleTestPlainAuthObjectWithRevisionAutoRequest getSimpleTestPlainAuthObjectWithRevisionAutoRequest)
        {
            long revision = getSimpleTestPlainAuthObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getSimpleTestPlainAuthObjectWithRevisionAutoRequest.testPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestPlainAuthObjectWithRevisionInternal(testPlainAuthObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectSimpleDTO GetSimpleTestPlainAuthObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetObjectByRevision(testPlainAuthObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestPlainAuthObjectPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPlainAuthObjectPropertyRevisionByDateRange(GetTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest getTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest.testPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestPlainAuthObjectPropertyRevisionByDateRangeInternal(testPlainAuthObjectIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPlainAuthObjectPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestPlainAuthObject>(testPlainAuthObjectIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestPlainAuthObjectPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPlainAuthObjectPropertyRevisions(GetTestPlainAuthObjectPropertyRevisionsAutoRequest getTestPlainAuthObjectPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestPlainAuthObjectPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getTestPlainAuthObjectPropertyRevisionsAutoRequest.testPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestPlainAuthObjectPropertyRevisionsInternal(testPlainAuthObjectIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPlainAuthObjectPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestPlainAuthObject>(testPlainAuthObjectIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestPlainAuthObject revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestPlainAuthObjectRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestPlainAuthObjectRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestPlainAuthObjectRevisionsInternal(testPlainAuthObjectIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestPlainAuthObjectRevisionsInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testPlainAuthObjectIdentity.Id));
        }
        
        /// <summary>
        /// Get TestPlainAuthObject (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPlainAuthObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO GetVisualTestPlainAuthObjectWithRevision(GetVisualTestPlainAuthObjectWithRevisionAutoRequest getVisualTestPlainAuthObjectWithRevisionAutoRequest)
        {
            long revision = getVisualTestPlainAuthObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity = getVisualTestPlainAuthObjectWithRevisionAutoRequest.testPlainAuthObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestPlainAuthObjectWithRevisionInternal(testPlainAuthObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPlainAuthObjectVisualDTO GetVisualTestPlainAuthObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPlainAuthObjectBLL bll = evaluateData.Context.Logics.TestPlainAuthObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPlainAuthObject domainObject = bll.GetObjectByRevision(testPlainAuthObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestPlainAuthObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestPlainAuthObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestPlainAuthObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestPlainAuthObjectPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestPlainAuthObjectPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestPlainAuthObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPlainAuthObjectIdentityDTO testPlainAuthObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
