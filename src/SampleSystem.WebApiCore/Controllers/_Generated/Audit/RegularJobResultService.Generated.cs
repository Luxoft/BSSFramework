namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class RegularJobResultController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public RegularJobResultController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get RegularJobResult (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullRegularJobResultWithRevision")]
        public virtual SampleSystem.Generated.DTO.RegularJobResultFullDTO GetFullRegularJobResultWithRevision(GetFullRegularJobResultWithRevisionAutoRequest getFullRegularJobResultWithRevisionAutoRequest)
        {
            long revision = getFullRegularJobResultWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity = getFullRegularJobResultWithRevisionAutoRequest.regularJobResultIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullRegularJobResultWithRevisionInternal(regularJobResultIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.RegularJobResultFullDTO GetFullRegularJobResultWithRevisionInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.RegularJobResult domainObject = bll.GetObjectByRevision(regularJobResultIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get RegularJobResult Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRegularJobResultPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetRegularJobResultPropertyRevisionByDateRange(GetRegularJobResultPropertyRevisionByDateRangeAutoRequest getRegularJobResultPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getRegularJobResultPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getRegularJobResultPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity = getRegularJobResultPropertyRevisionByDateRangeAutoRequest.regularJobResultIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRegularJobResultPropertyRevisionByDateRangeInternal(regularJobResultIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetRegularJobResultPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.RegularJobResult>(regularJobResultIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get RegularJobResult Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRegularJobResultPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetRegularJobResultPropertyRevisions(GetRegularJobResultPropertyRevisionsAutoRequest getRegularJobResultPropertyRevisionsAutoRequest)
        {
            string propertyName = getRegularJobResultPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity = getRegularJobResultPropertyRevisionsAutoRequest.regularJobResultIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRegularJobResultPropertyRevisionsInternal(regularJobResultIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetRegularJobResultPropertyRevisionsInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.RegularJobResult>(regularJobResultIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get RegularJobResult revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRegularJobResultRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetRegularJobResultRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRegularJobResultRevisionsInternal(regularJobResultIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetRegularJobResultRevisionsInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(regularJobResultIdentity.Id));
        }
        
        /// <summary>
        /// Get RegularJobResult (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichRegularJobResultWithRevision")]
        public virtual SampleSystem.Generated.DTO.RegularJobResultRichDTO GetRichRegularJobResultWithRevision(GetRichRegularJobResultWithRevisionAutoRequest getRichRegularJobResultWithRevisionAutoRequest)
        {
            long revision = getRichRegularJobResultWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity = getRichRegularJobResultWithRevisionAutoRequest.regularJobResultIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichRegularJobResultWithRevisionInternal(regularJobResultIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.RegularJobResultRichDTO GetRichRegularJobResultWithRevisionInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.RegularJobResult domainObject = bll.GetObjectByRevision(regularJobResultIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get RegularJobResult (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleRegularJobResultWithRevision")]
        public virtual SampleSystem.Generated.DTO.RegularJobResultSimpleDTO GetSimpleRegularJobResultWithRevision(GetSimpleRegularJobResultWithRevisionAutoRequest getSimpleRegularJobResultWithRevisionAutoRequest)
        {
            long revision = getSimpleRegularJobResultWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity = getSimpleRegularJobResultWithRevisionAutoRequest.regularJobResultIdentity;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleRegularJobResultWithRevisionInternal(regularJobResultIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.RegularJobResultSimpleDTO GetSimpleRegularJobResultWithRevisionInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.RegularJobResult domainObject = bll.GetObjectByRevision(regularJobResultIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullRegularJobResultWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRegularJobResultPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRegularJobResultPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetRichRegularJobResultWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleRegularJobResultWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
}
