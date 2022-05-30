namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class TestPerformanceObjectController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public TestPerformanceObjectController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullTestPerformanceObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectWithRevision(GetFullTestPerformanceObjectWithRevisionAutoRequest getFullTestPerformanceObjectWithRevisionAutoRequest)
        {
            long revision = getFullTestPerformanceObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getFullTestPerformanceObjectWithRevisionAutoRequest.testPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullTestPerformanceObjectWithRevisionInternal(testPerformanceObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectFullDTO GetFullTestPerformanceObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetObjectByRevision(testPerformanceObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichTestPerformanceObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectWithRevision(GetRichTestPerformanceObjectWithRevisionAutoRequest getRichTestPerformanceObjectWithRevisionAutoRequest)
        {
            long revision = getRichTestPerformanceObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getRichTestPerformanceObjectWithRevisionAutoRequest.testPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichTestPerformanceObjectWithRevisionInternal(testPerformanceObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectRichDTO GetRichTestPerformanceObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetObjectByRevision(testPerformanceObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleTestPerformanceObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectWithRevision(GetSimpleTestPerformanceObjectWithRevisionAutoRequest getSimpleTestPerformanceObjectWithRevisionAutoRequest)
        {
            long revision = getSimpleTestPerformanceObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getSimpleTestPerformanceObjectWithRevisionAutoRequest.testPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleTestPerformanceObjectWithRevisionInternal(testPerformanceObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectSimpleDTO GetSimpleTestPerformanceObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetObjectByRevision(testPerformanceObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get TestPerformanceObject Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestPerformanceObjectPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPerformanceObjectPropertyRevisionByDateRange(GetTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest getTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest.testPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestPerformanceObjectPropertyRevisionByDateRangeInternal(testPerformanceObjectIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPerformanceObjectPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestPerformanceObject>(testPerformanceObjectIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get TestPerformanceObject Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestPerformanceObjectPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPerformanceObjectPropertyRevisions(GetTestPerformanceObjectPropertyRevisionsAutoRequest getTestPerformanceObjectPropertyRevisionsAutoRequest)
        {
            string propertyName = getTestPerformanceObjectPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getTestPerformanceObjectPropertyRevisionsAutoRequest.testPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestPerformanceObjectPropertyRevisionsInternal(testPerformanceObjectIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetTestPerformanceObjectPropertyRevisionsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.TestPerformanceObject>(testPerformanceObjectIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get TestPerformanceObject revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetTestPerformanceObjectRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestPerformanceObjectRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetTestPerformanceObjectRevisionsInternal(testPerformanceObjectIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetTestPerformanceObjectRevisionsInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(testPerformanceObjectIdentity.Id));
        }
        
        /// <summary>
        /// Get TestPerformanceObject (VisualDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualTestPerformanceObjectWithRevision")]
        public virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectWithRevision(GetVisualTestPerformanceObjectWithRevisionAutoRequest getVisualTestPerformanceObjectWithRevisionAutoRequest)
        {
            long revision = getVisualTestPerformanceObjectWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity = getVisualTestPerformanceObjectWithRevisionAutoRequest.testPerformanceObjectIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetVisualTestPerformanceObjectWithRevisionInternal(testPerformanceObjectIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.TestPerformanceObjectVisualDTO GetVisualTestPerformanceObjectWithRevisionInternal(SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ITestPerformanceObjectBLL bll = evaluateData.Context.Logics.TestPerformanceObjectFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.TestPerformanceObject domainObject = bll.GetObjectByRevision(testPerformanceObjectIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullTestPerformanceObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichTestPerformanceObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleTestPerformanceObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestPerformanceObjectPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetTestPerformanceObjectPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetVisualTestPerformanceObjectWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.TestPerformanceObjectIdentityDTO testPerformanceObjectIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
