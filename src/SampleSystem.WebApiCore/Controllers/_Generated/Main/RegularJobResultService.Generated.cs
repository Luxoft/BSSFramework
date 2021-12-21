namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class RegularJobResultController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext>, SampleSystem.BLL.ISampleSystemBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>>
    {
        
        public RegularJobResultController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<SampleSystem.BLL.ISampleSystemBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        /// <summary>
        /// Check RegularJobResult access
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("CheckRegularJobResultAccess")]
        public virtual void CheckRegularJobResultAccess(CheckRegularJobResultAccessAutoRequest checkRegularJobResultAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = checkRegularJobResultAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdent = checkRegularJobResultAccessAutoRequest.regularJobResultIdent;
            this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.CheckRegularJobResultAccessInternal(regularJobResultIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual void CheckRegularJobResultAccessInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResult;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.RegularJobResult domainObject = bll.GetById(regularJobResultIdent.Id, true);
            Framework.SecuritySystem.SecurityProviderExtensions.CheckAccess(evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.RegularJobResult>(securityOperationCode), domainObject);
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, SampleSystem.BLL.ISampleSystemBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>(session, context, new SampleSystemServerPrimitiveDTOMappingService(context));
        }
        
        /// <summary>
        /// Get RegularJobResult (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullRegularJobResult")]
        public virtual SampleSystem.Generated.DTO.RegularJobResultFullDTO GetFullRegularJobResult([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullRegularJobResultInternal(regularJobResultIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.RegularJobResultFullDTO GetFullRegularJobResultInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.RegularJobResult domainObject = bll.GetById(regularJobResultIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.RegularJobResult>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of RegularJobResults (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullRegularJobResults")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.RegularJobResultFullDTO> GetFullRegularJobResults()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullRegularJobResultsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get RegularJobResults (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullRegularJobResultsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.RegularJobResultFullDTO> GetFullRegularJobResultsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.RegularJobResultIdentityDTO[] regularJobResultIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetFullRegularJobResultsByIdentsInternal(regularJobResultIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.RegularJobResultFullDTO> GetFullRegularJobResultsByIdentsInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO[] regularJobResultIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(regularJobResultIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.RegularJobResult>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.RegularJobResultFullDTO> GetFullRegularJobResultsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.RegularJobResult>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get RegularJobResult (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichRegularJobResult")]
        public virtual SampleSystem.Generated.DTO.RegularJobResultRichDTO GetRichRegularJobResult([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetRichRegularJobResultInternal(regularJobResultIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.RegularJobResultRichDTO GetRichRegularJobResultInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.RegularJobResult domainObject = bll.GetById(regularJobResultIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.RegularJobResult>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get RegularJobResult (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleRegularJobResult")]
        public virtual SampleSystem.Generated.DTO.RegularJobResultSimpleDTO GetSimpleRegularJobResult([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleRegularJobResultInternal(regularJobResultIdentity, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.RegularJobResultSimpleDTO GetSimpleRegularJobResultInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            SampleSystem.Domain.RegularJobResult domainObject = bll.GetById(regularJobResultIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.RegularJobResult>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of RegularJobResults (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleRegularJobResults")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.RegularJobResultSimpleDTO> GetSimpleRegularJobResults()
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleRegularJobResultsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get RegularJobResults (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleRegularJobResultsByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.RegularJobResultSimpleDTO> GetSimpleRegularJobResultsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.RegularJobResultIdentityDTO[] regularJobResultIdents)
        {
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.GetSimpleRegularJobResultsByIdentsInternal(regularJobResultIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.RegularJobResultSimpleDTO> GetSimpleRegularJobResultsByIdentsInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO[] regularJobResultIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(regularJobResultIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.RegularJobResult>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.RegularJobResultSimpleDTO> GetSimpleRegularJobResultsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResultFactory.Create(Framework.SecuritySystem.BLLSecurityMode.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.RegularJobResult>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Check access for RegularJobResult
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("HasRegularJobResultAccess")]
        public virtual bool HasRegularJobResultAccess(HasRegularJobResultAccessAutoRequest hasRegularJobResultAccessAutoRequest)
        {
            SampleSystem.SampleSystemSecurityOperationCode securityOperationCode = hasRegularJobResultAccessAutoRequest.securityOperationCode;
            SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdent = hasRegularJobResultAccessAutoRequest.regularJobResultIdent;
            return this.Evaluate(Framework.DomainDriven.BLL.DBSessionMode.Read, evaluateData => this.HasRegularJobResultAccessInternal(regularJobResultIdent, securityOperationCode, evaluateData));
        }
        
        protected virtual bool HasRegularJobResultAccessInternal(SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdent, SampleSystem.SampleSystemSecurityOperationCode securityOperationCode, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IRegularJobResultBLL bll = evaluateData.Context.Logics.RegularJobResult;
            Framework.Security.TransferEnumHelper.Check(securityOperationCode);
            SampleSystem.Domain.RegularJobResult domainObject = bll.GetById(regularJobResultIdent.Id, true);
            return evaluateData.Context.SecurityService.GetSecurityProvider<SampleSystem.Domain.RegularJobResult>(securityOperationCode).HasAccess(domainObject);
        }
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class CheckRegularJobResultAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
    
    [System.Runtime.Serialization.DataContractAttribute()]
    [Framework.DomainDriven.ServiceModel.IAD.AutoRequestAttribute()]
    public partial class HasRegularJobResultAccessAutoRequest
    {
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=0)]
        public SampleSystem.Generated.DTO.RegularJobResultIdentityDTO regularJobResultIdent;
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        [Framework.DomainDriven.ServiceModel.IAD.AutoRequestPropertyAttribute(OrderIndex=1)]
        public SampleSystem.SampleSystemSecurityOperationCode securityOperationCode;
    }
}
