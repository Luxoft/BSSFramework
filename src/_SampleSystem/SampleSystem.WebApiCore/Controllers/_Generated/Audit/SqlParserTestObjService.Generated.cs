namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/v{version:apiVersion}/[controller]")]
    public partial class SqlParserTestObjController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        /// <summary>
        /// Get SqlParserTestObj (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObjWithRevision(GetFullSqlParserTestObjWithRevisionAutoRequest getFullSqlParserTestObjWithRevisionAutoRequest)
        {
            long revision = getFullSqlParserTestObjWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getFullSqlParserTestObjWithRevisionAutoRequest.sqlParserTestObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjWithRevisionInternal(sqlParserTestObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjFullDTO GetFullSqlParserTestObjWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetObjectByRevision(sqlParserTestObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSqlParserTestObjWithRevision")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObjWithRevision(GetSimpleSqlParserTestObjWithRevisionAutoRequest getSimpleSqlParserTestObjWithRevisionAutoRequest)
        {
            long revision = getSimpleSqlParserTestObjWithRevisionAutoRequest.revision;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSimpleSqlParserTestObjWithRevisionAutoRequest.sqlParserTestObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjWithRevisionInternal(sqlParserTestObjIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjSimpleDTO GetSimpleSqlParserTestObjWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SqlParserTestObj domainObject = bll.GetObjectByRevision(sqlParserTestObjIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SqlParserTestObj Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjPropertyRevisionByDateRange")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionByDateRange(GetSqlParserTestObjPropertyRevisionByDateRangeAutoRequest getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.period;
            string propertyName = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.propertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSqlParserTestObjPropertyRevisionByDateRangeAutoRequest.sqlParserTestObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjPropertyRevisionByDateRangeInternal(sqlParserTestObjIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObj>(sqlParserTestObjIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get SqlParserTestObj Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisions(GetSqlParserTestObjPropertyRevisionsAutoRequest getSqlParserTestObjPropertyRevisionsAutoRequest)
        {
            string propertyName = getSqlParserTestObjPropertyRevisionsAutoRequest.propertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity = getSqlParserTestObjPropertyRevisionsAutoRequest.sqlParserTestObjIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjPropertyRevisionsInternal(sqlParserTestObjIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjPropertyRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, SampleSystem.BLL.ISampleSystemSecurityService, SampleSystem.SampleSystemSecurityOperationCode, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObj>(sqlParserTestObjIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get SqlParserTestObj revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjRevisions")]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetSqlParserTestObjRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjRevisionsInternal(sqlParserTestObjIdentity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetSqlParserTestObjRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjBLL bll = evaluateData.Context.Logics.SqlParserTestObjFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(sqlParserTestObjIdentity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullSqlParserTestObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSimpleSqlParserTestObjWithRevisionAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public long revision;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSqlParserTestObjPropertyRevisionByDateRangeAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=2)]
        public Framework.Core.Period? period;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetSqlParserTestObjPropertyRevisionsAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SqlParserTestObjIdentityDTO sqlParserTestObjIdentity;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public string propertyName;
    }
}
