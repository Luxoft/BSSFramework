namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class PrincipalController : Framework.Infrastructure.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Principal (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalFullDTO GetFullPrincipalWithRevision(GetFullPrincipalWithRevisionAutoRequest getFullPrincipalWithRevisionAutoRequest)
        {
            long revision = getFullPrincipalWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity = getFullPrincipalWithRevisionAutoRequest.PrincipalIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetFullPrincipalWithRevisionInternal(principalIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalFullDTO GetFullPrincipalWithRevisionInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ExternalPrincipal.Principal domainObject = bll.GetObjectByRevision(principalIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetPrincipalPropertyRevisionByDateRange(GetPrincipalPropertyRevisionByDateRangeAutoRequest getPrincipalPropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getPrincipalPropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getPrincipalPropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity = getPrincipalPropertyRevisionByDateRangeAutoRequest.PrincipalIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetPrincipalPropertyRevisionByDateRangeInternal(principalIdentity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetPrincipalPropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, string propertyName, Framework.Core.Period? period, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.Infrastructure.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.BLL.Services.IRootSecurityService, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ExternalPrincipal.Principal>(principalIdentity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get Principal Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetPrincipalPropertyRevisions(GetPrincipalPropertyRevisionsAutoRequest getPrincipalPropertyRevisionsAutoRequest)
        {
            string propertyName = getPrincipalPropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity = getPrincipalPropertyRevisionsAutoRequest.PrincipalIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetPrincipalPropertyRevisionsInternal(principalIdentity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetPrincipalPropertyRevisionsInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, string propertyName, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.Infrastructure.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.BLL.Services.IRootSecurityService, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.ExternalPrincipal.Principal>(principalIdentity.Id, propertyName);
        }
        
        /// <summary>
        /// Get Principal revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO GetPrincipalRevisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity)
        {
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetPrincipalRevisionsInternal(principalIdentity, evaluateData));
        }
        
        protected virtual Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO GetPrincipalRevisionsInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.BLL.DTOMapping.Domain.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(principalIdentity.Id));
        }
        
        /// <summary>
        /// Get Principal (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalRichDTO GetRichPrincipalWithRevision(GetRichPrincipalWithRevisionAutoRequest getRichPrincipalWithRevisionAutoRequest)
        {
            long revision = getRichPrincipalWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity = getRichPrincipalWithRevisionAutoRequest.PrincipalIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetRichPrincipalWithRevisionInternal(principalIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalRichDTO GetRichPrincipalWithRevisionInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ExternalPrincipal.Principal domainObject = bll.GetObjectByRevision(principalIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Principal (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalWithRevision(GetSimplePrincipalWithRevisionAutoRequest getSimplePrincipalWithRevisionAutoRequest)
        {
            long revision = getSimplePrincipalWithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity = getSimplePrincipalWithRevisionAutoRequest.PrincipalIdentity;
            return this.Evaluate(Framework.Database.DBSessionMode.Read, evaluateData => this.GetSimplePrincipalWithRevisionInternal(principalIdentity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.PrincipalSimpleDTO GetSimplePrincipalWithRevisionInternal(SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity, long revision, Framework.Infrastructure.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IPrincipalBLL bll = evaluateData.Context.Logics.PrincipalFactory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.ExternalPrincipal.Principal domainObject = bll.GetObjectByRevision(principalIdentity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetFullPrincipalWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO PrincipalIdentity
        {
            get
            {
                return this.principalIdentity;
            }
            set
            {
                this.principalIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetPrincipalPropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO PrincipalIdentity
        {
            get
            {
                return this.principalIdentity;
            }
            set
            {
                this.principalIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=2)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetPrincipalPropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO PrincipalIdentity
        {
            get
            {
                return this.principalIdentity;
            }
            set
            {
                this.principalIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetRichPrincipalWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO PrincipalIdentity
        {
            get
            {
                return this.principalIdentity;
            }
            set
            {
                this.principalIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
    [Framework.BLL.DTOMapping.Domain.AutoRequestAttribute()]
    public partial class GetSimplePrincipalWithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.PrincipalIdentityDTO principalIdentity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.PrincipalIdentityDTO PrincipalIdentity
        {
            get
            {
                return this.principalIdentity;
            }
            set
            {
                this.principalIdentity = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.BLL.DTOMapping.Domain.AutoRequestPropertyAttribute(OrderIndex=1)]
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
