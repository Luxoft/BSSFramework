namespace SampleSystem.WebApiCore.Controllers.Audit
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class SqlParserTestObjContainerController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get SqlParserTestObjContainer (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSqlParserTestObjContainerWithRevision")]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO GetFullSqlParserTestObjContainerWithRevision(GetFullSqlParserTestObjContainerWithRevisionAutoRequest getFullSqlParserTestObjContainerWithRevisionAutoRequest)
        {
            long revision = getFullSqlParserTestObjContainerWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity = getFullSqlParserTestObjContainerWithRevisionAutoRequest.SqlParserTestObjContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullSqlParserTestObjContainerWithRevisionInternal(sqlParserTestObjContainerIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerFullDTO GetFullSqlParserTestObjContainerWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            long revision = getSimpleSqlParserTestObjContainerWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity = getSimpleSqlParserTestObjContainerWithRevisionAutoRequest.SqlParserTestObjContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleSqlParserTestObjContainerWithRevisionInternal(sqlParserTestObjContainerIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerSimpleDTO GetSimpleSqlParserTestObjContainerWithRevisionInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            Framework.Core.Period? period = getSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity = getSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest.SqlParserTestObjContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjContainerPropertyRevisionByDateRangeInternal(sqlParserTestObjContainerIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjContainerPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.SqlParserTestObjContainer>(sqlParserTestObjContainerIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get SqlParserTestObjContainer Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSqlParserTestObjContainerPropertyRevisions")]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjContainerPropertyRevisions(GetSqlParserTestObjContainerPropertyRevisionsAutoRequest getSqlParserTestObjContainerPropertyRevisionsAutoRequest)
        {
            string propertyName = getSqlParserTestObjContainerPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity = getSqlParserTestObjContainerPropertyRevisionsAutoRequest.SqlParserTestObjContainerIdentity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSqlParserTestObjContainerPropertyRevisionsInternal(sqlParserTestObjContainerIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetSqlParserTestObjContainerPropertyRevisionsInternal(SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.SecurityRule.View);
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
            SampleSystem.BLL.ISqlParserTestObjContainerBLL bll = evaluateData.Context.Logics.SqlParserTestObjContainerFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(sqlParserTestObjContainerIdentity.Id));
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullSqlParserTestObjContainerWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO SqlParserTestObjContainerIdentity
        {
            get
            {
                return this.sqlParserTestObjContainerIdentity;
            }
            set
            {
                this.sqlParserTestObjContainerIdentity = value;
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
    public partial class GetSimpleSqlParserTestObjContainerWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO SqlParserTestObjContainerIdentity
        {
            get
            {
                return this.sqlParserTestObjContainerIdentity;
            }
            set
            {
                this.sqlParserTestObjContainerIdentity = value;
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
    public partial class GetSqlParserTestObjContainerPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO SqlParserTestObjContainerIdentity
        {
            get
            {
                return this.sqlParserTestObjContainerIdentity;
            }
            set
            {
                this.sqlParserTestObjContainerIdentity = value;
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
    public partial class GetSqlParserTestObjContainerPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO sqlParserTestObjContainerIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.SqlParserTestObjContainerIdentityDTO SqlParserTestObjContainerIdentity
        {
            get
            {
                return this.sqlParserTestObjContainerIdentity;
            }
            set
            {
                this.sqlParserTestObjContainerIdentity = value;
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
}
