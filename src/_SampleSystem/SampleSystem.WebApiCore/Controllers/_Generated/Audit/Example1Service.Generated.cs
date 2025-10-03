namespace SampleSystem.WebApiCore.Controllers.Audit
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("mainAuditApi/[controller]/[action]")]
    public partial class Example1Controller : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get Example1 Property Revisions by period
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetExample1PropertyRevisionByDateRange(GetExample1PropertyRevisionByDateRangeAutoRequest getExample1PropertyRevisionByDateRangeAutoRequest)
        {
            Framework.Core.Period? period = getExample1PropertyRevisionByDateRangeAutoRequest.Period;
            string propertyName = getExample1PropertyRevisionByDateRangeAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity = getExample1PropertyRevisionByDateRangeAutoRequest.Example1Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetExample1PropertyRevisionByDateRangeInternal(example1Identity, propertyName, period, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetExample1PropertyRevisionByDateRangeInternal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, string propertyName, Framework.Core.Period? period, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Example1>(example1Identity.Id, propertyName, period);
        }
        
        /// <summary>
        /// Get Example1 Property Revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetExample1PropertyRevisions(GetExample1PropertyRevisionsAutoRequest getExample1PropertyRevisionsAutoRequest)
        {
            string propertyName = getExample1PropertyRevisionsAutoRequest.PropertyName;
            SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity = getExample1PropertyRevisionsAutoRequest.Example1Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetExample1PropertyRevisionsInternal(example1Identity, propertyName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO GetExample1PropertyRevisionsInternal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, string propertyName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.Service.AuditService<System.Guid, SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.BLL.ISampleSystemBLLFactoryContainer, Framework.DomainDriven.BLL.Security.IRootSecurityService<SampleSystem.Domain.PersistentDomainObjectBase>, SampleSystem.Domain.PersistentDomainObjectBase, SampleSystem.Generated.DTO.SampleSystemDomainObjectPropertiesRevisionDTO, SampleSystem.Generated.DTO.SampleSystemPropertyRevisionDTO>(evaluateData.Context).GetPropertyChanges<SampleSystem.Domain.Example1>(example1Identity.Id, propertyName);
        }
        
        /// <summary>
        /// Get Example1 revisions
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetExample1Revisions([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetExample1RevisionsInternal(example1Identity, evaluateData));
        }
        
        protected virtual Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO GetExample1RevisionsInternal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            return new Framework.DomainDriven.ServiceModel.IAD.DefaultDomainObjectRevisionDTO(bll.GetObjectRevisions(example1Identity.Id));
        }
        
        /// <summary>
        /// Get Example1 (FullDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.Example1FullDTO GetFullExample1WithRevision(GetFullExample1WithRevisionAutoRequest getFullExample1WithRevisionAutoRequest)
        {
            long revision = getFullExample1WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity = getFullExample1WithRevisionAutoRequest.Example1Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullExample1WithRevisionInternal(example1Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1FullDTO GetFullExample1WithRevisionInternal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Example1 domainObject = bll.GetObjectByRevision(example1Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Example1 (RichDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.Example1RichDTO GetRichExample1WithRevision(GetRichExample1WithRevisionAutoRequest getRichExample1WithRevisionAutoRequest)
        {
            long revision = getRichExample1WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity = getRichExample1WithRevisionAutoRequest.Example1Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichExample1WithRevisionInternal(example1Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1RichDTO GetRichExample1WithRevisionInternal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Example1 domainObject = bll.GetObjectByRevision(example1Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get Example1 (SimpleDTO) by revision
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.Example1SimpleDTO GetSimpleExample1WithRevision(GetSimpleExample1WithRevisionAutoRequest getSimpleExample1WithRevisionAutoRequest)
        {
            long revision = getSimpleExample1WithRevisionAutoRequest.Revision;
            SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity = getSimpleExample1WithRevisionAutoRequest.Example1Identity;
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleExample1WithRevisionInternal(example1Identity, revision, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.Example1SimpleDTO GetSimpleExample1WithRevisionInternal(SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity, long revision, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IExample1BLL bll = evaluateData.Context.Logics.Example1Factory.Create(SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.Example1 domainObject = bll.GetObjectByRevision(example1Identity.Id, revision);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetExample1PropertyRevisionByDateRangeAutoRequest
    {
        
        private SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity;
        
        private string propertyName;
        
        private Framework.Core.Period? period;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.Example1IdentityDTO Example1Identity
        {
            get
            {
                return this.example1Identity;
            }
            set
            {
                this.example1Identity = value;
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
    public partial class GetExample1PropertyRevisionsAutoRequest
    {
        
        private SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity;
        
        private string propertyName;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.Example1IdentityDTO Example1Identity
        {
            get
            {
                return this.example1Identity;
            }
            set
            {
                this.example1Identity = value;
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
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class GetFullExample1WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.Example1IdentityDTO Example1Identity
        {
            get
            {
                return this.example1Identity;
            }
            set
            {
                this.example1Identity = value;
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
    public partial class GetRichExample1WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.Example1IdentityDTO Example1Identity
        {
            get
            {
                return this.example1Identity;
            }
            set
            {
                this.example1Identity = value;
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
    public partial class GetSimpleExample1WithRevisionAutoRequest
    {
        
        private SampleSystem.Generated.DTO.Example1IdentityDTO example1Identity;
        
        private long revision;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public virtual SampleSystem.Generated.DTO.Example1IdentityDTO Example1Identity
        {
            get
            {
                return this.example1Identity;
            }
            set
            {
                this.example1Identity = value;
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
}
