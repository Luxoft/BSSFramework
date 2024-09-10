namespace SampleSystem.WebApiCore.Controllers.Main
{
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/[controller]/[action]")]
    public partial class EmployeeInformationController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeInformation (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationFullDTO GetFullEmployeeInformation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeInformationInternal(employeeInformationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeInformation (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationFullDTO GetFullEmployeeInformationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeInformationName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeInformationByNameInternal(employeeInformationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationFullDTO GetFullEmployeeInformationByNameInternal(string employeeInformationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeInformationName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationFullDTO GetFullEmployeeInformationInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = bll.GetById(employeeInformationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeInformations (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationFullDTO> GetFullEmployeeInformations()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeInformationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeInformations (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationFullDTO> GetFullEmployeeInformationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO[] employeeInformationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeInformationsByIdentsInternal(employeeInformationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationFullDTO> GetFullEmployeeInformationsByIdentsInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO[] employeeInformationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeeInformationIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationFullDTO> GetFullEmployeeInformationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeInformation (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationRichDTO GetRichEmployeeInformation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeInformationInternal(employeeInformationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeInformation (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationRichDTO GetRichEmployeeInformationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeInformationName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeInformationByNameInternal(employeeInformationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationRichDTO GetRichEmployeeInformationByNameInternal(string employeeInformationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeInformationName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationRichDTO GetRichEmployeeInformationInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = bll.GetById(employeeInformationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeInformation (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO GetSimpleEmployeeInformation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeInformationInternal(employeeInformationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeInformation (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO GetSimpleEmployeeInformationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeInformationName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeInformationByNameInternal(employeeInformationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO GetSimpleEmployeeInformationByNameInternal(string employeeInformationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeInformationName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO GetSimpleEmployeeInformationInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = bll.GetById(employeeInformationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeInformations (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO> GetSimpleEmployeeInformations()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeInformationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeInformations (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO> GetSimpleEmployeeInformationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO[] employeeInformationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeInformationsByIdentsInternal(employeeInformationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO> GetSimpleEmployeeInformationsByIdentsInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO[] employeeInformationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeeInformationIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationSimpleDTO> GetSimpleEmployeeInformationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeInformation (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationVisualDTO GetVisualEmployeeInformation([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeInformationInternal(employeeInformationIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeInformation (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual SampleSystem.Generated.DTO.EmployeeInformationVisualDTO GetVisualEmployeeInformationByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeInformationName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeInformationByNameInternal(employeeInformationName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationVisualDTO GetVisualEmployeeInformationByNameInternal(string employeeInformationName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeInformationName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeInformationVisualDTO GetVisualEmployeeInformationInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO employeeInformationIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeInformation domainObject = bll.GetById(employeeInformationIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeInformations (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationVisualDTO> GetVisualEmployeeInformations()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeInformationsInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeInformations (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationVisualDTO> GetVisualEmployeeInformationsByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO[] employeeInformationIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeInformationsByIdentsInternal(employeeInformationIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationVisualDTO> GetVisualEmployeeInformationsByIdentsInternal(SampleSystem.Generated.DTO.EmployeeInformationIdentityDTO[] employeeInformationIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(employeeInformationIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeInformationVisualDTO> GetVisualEmployeeInformationsInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeInformationBLL bll = evaluateData.Context.Logics.EmployeeInformationFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeInformation>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
