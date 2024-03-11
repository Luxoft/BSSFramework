namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class SqlParserTestObjContainerController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get SqlParserTestObjContainer (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjContainerWithRevision")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO GetFullSqlParserTestObjContainerWithRevision(GetFullSqlParserTestObjContainerWithRevisionAutoRequest getFullSqlParserTestObjContainerWithRevisionAutoRequest)
        {
            long revision = getFullSqlParserTestObjContainerWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity = getFullSqlParserTestObjContainerWithRevisionAutoRequest.sqlParserTestObjContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjContainerWithRevisionInternal(sqlParserTestObjContainerIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO GetFullSqlParserTestObjContainerWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SqlParserTestObjContainer domainObject = bll.GetObjectByRevision(sqlParserTestObjContainerIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObjContainer (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObjContainerWithRevision")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO GetSimpleSqlParserTestObjContainerWithRevision(GetSimpleSqlParserTestObjContainerWithRevisionAutoRequest getSimpleSqlParserTestObjContainerWithRevisionAutoRequest)
        {
            long revision = getSimpleSqlParserTestObjContainerWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity = getSimpleSqlParserTestObjContainerWithRevisionAutoRequest.sqlParserTestObjContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjContainerWithRevisionInternal(sqlParserTestObjContainerIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO GetSimpleSqlParserTestObjContainerWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SqlParserTestObjContainer domainObject = bll.GetObjectByRevision(sqlParserTestObjContainerIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObjContainer Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjContainerPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjContainerPropertyRevisionByDateRange(GetSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest getSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity = getSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest.sqlParserTestObjContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjContainerPropertyRevisionByDateRangeInternal(sqlParserTestObjContainerIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjContainerPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObjContainer>(sqlParserTestObjContainerIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get SqlParserTestObjContainer Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjContainerPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjContainerPropertyRevisions(GetSqlParserTestObjContainerPropertyRevisionsAutoRequest getSqlParserTestObjContainerPropertyRevisionsAutoRequest)
        {
            string propertyName = getSqlParserTestObjContainerPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity = getSqlParserTestObjContainerPropertyRevisionsAutoRequest.sqlParserTestObjContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjContainerPropertyRevisionsInternal(sqlParserTestObjContainerIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjContainerPropertyRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObjContainer>(sqlParserTestObjContainerIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get SqlParserTestObjContainer revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjContainerRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetSqlParserTestObjContainerRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjContainerRevisionsInternal(sqlParserTestObjContainerIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetSqlParserTestObjContainerRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(sqlParserTestObjContainerIdentity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullSqlParserTestObjContainerWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleSqlParserTestObjContainerWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSqlParserTestObjContainerPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
}
