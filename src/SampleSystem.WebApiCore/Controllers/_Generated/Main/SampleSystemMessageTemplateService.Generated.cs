namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class SampleSystemMessageTemplateController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public SampleSystemMessageTemplateController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check SampleSystemMessageTemplate access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckSampleSystemMessageTemplateAccess")]
        public virtual void CheckSampleSystemMessageTemplateAccess(CheckSampleSystemMessageTemplateAccessAutoRequest checkSampleSystemMessageTemplateAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkSampleSystemMessageTemplateAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdent = checkSampleSystemMessageTemplateAccessAutoRequest.sampleSystemMessageTemplateIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckSampleSystemMessageTemplateAccessInternal(sampleSystemMessageTemplateIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckSampleSystemMessageTemplateAccessInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplate;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.SampleSystemMessageTemplate domainObject = bll.GetById(sampleSystemMessageTemplateIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.SampleSystemMessageTemplate>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSampleSystemMessageTemplate")]
        public virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateFullDTO GetFullSampleSystemMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSampleSystemMessageTemplateInternal(sampleSystemMessageTemplateIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateFullDTO GetFullSampleSystemMessageTemplateInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SampleSystemMessageTemplate domainObject = bll.GetById(sampleSystemMessageTemplateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SampleSystemMessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SampleSystemMessageTemplates (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSampleSystemMessageTemplates")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SampleSystemMessageTemplateFullDTO> GetFullSampleSystemMessageTemplates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSampleSystemMessageTemplatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplates (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullSampleSystemMessageTemplatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SampleSystemMessageTemplateFullDTO> GetFullSampleSystemMessageTemplatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO[] sampleSystemMessageTemplateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullSampleSystemMessageTemplatesByIdentsInternal(sampleSystemMessageTemplateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SampleSystemMessageTemplateFullDTO> GetFullSampleSystemMessageTemplatesByIdentsInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO[] sampleSystemMessageTemplateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(sampleSystemMessageTemplateIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SampleSystemMessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SampleSystemMessageTemplateFullDTO> GetFullSampleSystemMessageTemplatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SampleSystemMessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichSampleSystemMessageTemplate")]
        public virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateRichDTO GetRichSampleSystemMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichSampleSystemMessageTemplateInternal(sampleSystemMessageTemplateIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateRichDTO GetRichSampleSystemMessageTemplateInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SampleSystemMessageTemplate domainObject = bll.GetById(sampleSystemMessageTemplateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SampleSystemMessageTemplate>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplate (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSampleSystemMessageTemplate")]
        public virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateSimpleDTO GetSimpleSampleSystemMessageTemplate([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSampleSystemMessageTemplateInternal(sampleSystemMessageTemplateIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.SampleSystemMessageTemplateSimpleDTO GetSimpleSampleSystemMessageTemplateInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.SampleSystemMessageTemplate domainObject = bll.GetById(sampleSystemMessageTemplateIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SampleSystemMessageTemplate>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of SampleSystemMessageTemplates (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSampleSystemMessageTemplates")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SampleSystemMessageTemplateSimpleDTO> GetSimpleSampleSystemMessageTemplates()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSampleSystemMessageTemplatesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get SampleSystemMessageTemplates (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleSampleSystemMessageTemplatesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SampleSystemMessageTemplateSimpleDTO> GetSimpleSampleSystemMessageTemplatesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO[] sampleSystemMessageTemplateIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleSampleSystemMessageTemplatesByIdentsInternal(sampleSystemMessageTemplateIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SampleSystemMessageTemplateSimpleDTO> GetSimpleSampleSystemMessageTemplatesByIdentsInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO[] sampleSystemMessageTemplateIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(sampleSystemMessageTemplateIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SampleSystemMessageTemplate>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.SampleSystemMessageTemplateSimpleDTO> GetSimpleSampleSystemMessageTemplatesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplateFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.SampleSystemMessageTemplate>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for SampleSystemMessageTemplate
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasSampleSystemMessageTemplateAccess")]
        public virtual bool HasSampleSystemMessageTemplateAccess(HasSampleSystemMessageTemplateAccessAutoRequest hasSampleSystemMessageTemplateAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasSampleSystemMessageTemplateAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdent = hasSampleSystemMessageTemplateAccessAutoRequest.sampleSystemMessageTemplateIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasSampleSystemMessageTemplateAccessInternal(sampleSystemMessageTemplateIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasSampleSystemMessageTemplateAccessInternal(SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.ISampleSystemMessageTemplateBLL bll = evaluateData.Context.Logics.SampleSystemMessageTemplate;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.SampleSystemMessageTemplate domainObject = bll.GetById(sampleSystemMessageTemplateIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.SampleSystemMessageTemplate>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckSampleSystemMessageTemplateAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasSampleSystemMessageTemplateAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.SampleSystemMessageTemplateIdentityDTO sampleSystemMessageTemplateIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
