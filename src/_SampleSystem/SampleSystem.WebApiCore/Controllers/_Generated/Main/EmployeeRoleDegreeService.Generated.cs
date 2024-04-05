namespace SampleSystem.WebApiCore.Controllers.Main
{
    using SampleSystem.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("api/v{version:apiVersion}/[controller]")]
    public partial class EmployeeRoleDegreeController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService>
    {
        
        /// <summary>
        /// Get EmployeeRoleDegree (FullDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegree")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegree([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreeInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (FullDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegreeByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRoleDegreeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreeByNameInternal(employeeRoleDegreeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeByNameInternal(string employeeRoleDegreeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRoleDegreeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO GetFullEmployeeRoleDegreeInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRoleDegrees (FullDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegrees")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO> GetFullEmployeeRoleDegrees()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegrees (FullDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetFullEmployeeRoleDegreesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO> GetFullEmployeeRoleDegreesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetFullEmployeeRoleDegreesByIdentsInternal(employeeRoleDegreeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO> GetFullEmployeeRoleDegreesByIdentsInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetListByIdents(employeeRoleDegreeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeFullDTO> GetFullEmployeeRoleDegreesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToFullDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (RichDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRoleDegree")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegree([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRoleDegreeInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (RichDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetRichEmployeeRoleDegreeByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRoleDegreeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetRichEmployeeRoleDegreeByNameInternal(employeeRoleDegreeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeByNameInternal(string employeeRoleDegreeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRoleDegreeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeRichDTO GetRichEmployeeRoleDegreeInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.FullDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToRichDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (SimpleDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegree")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegree([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreeInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (SimpleDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegreeByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRoleDegreeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreeByNameInternal(employeeRoleDegreeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeByNameInternal(string employeeRoleDegreeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRoleDegreeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO GetSimpleEmployeeRoleDegreeInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.SimpleDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRoleDegrees (SimpleDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegrees")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO> GetSimpleEmployeeRoleDegrees()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegrees (SimpleDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetSimpleEmployeeRoleDegreesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO> GetSimpleEmployeeRoleDegreesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetSimpleEmployeeRoleDegreesByIdentsInternal(employeeRoleDegreeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO> GetSimpleEmployeeRoleDegreesByIdentsInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetListByIdents(employeeRoleDegreeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeSimpleDTO> GetSimpleEmployeeRoleDegreesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToSimpleDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.SimpleDTO)), evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (VisualDTO) by identity
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegree")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegree([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreeInternal(employeeRoleDegreeIdentity, evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegree (VisualDTO) by name
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegreeByName")]
        public virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeByName([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] string employeeRoleDegreeName)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreeByNameInternal(employeeRoleDegreeName, evaluateData));
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeByNameInternal(string employeeRoleDegreeName, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = Framework.DomainDriven.BLL.DefaultDomainBLLBaseExtensions.GetByName(bll, employeeRoleDegreeName, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        protected virtual SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO GetVisualEmployeeRoleDegreeInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO employeeRoleDegreeIdentity, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            SampleSystem.Domain.EmployeeRoleDegree domainObject = bll.GetById(employeeRoleDegreeIdentity.Id, true, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.VisualDTO));
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTO(domainObject, evaluateData.MappingService);
        }
        
        /// <summary>
        /// Get full list of EmployeeRoleDegrees (VisualDTO)
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegrees")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO> GetVisualEmployeeRoleDegrees()
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreesInternal(evaluateData));
        }
        
        /// <summary>
        /// Get EmployeeRoleDegrees (VisualDTO) by idents
        /// </summary>
        [Microsoft.AspNetCore.Mvc.HttpPostAttribute()]
        [Microsoft.AspNetCore.Mvc.RouteAttribute("GetVisualEmployeeRoleDegreesByIdents")]
        public virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO> GetVisualEmployeeRoleDegreesByIdents([Microsoft.AspNetCore.Mvc.FromBodyAttribute()] SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents)
        {
            return this.Evaluate(Framework.DomainDriven.DBSessionMode.Read, evaluateData => this.GetVisualEmployeeRoleDegreesByIdentsInternal(employeeRoleDegreeIdents, evaluateData));
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO> GetVisualEmployeeRoleDegreesByIdentsInternal(SampleSystem.Generated.DTO.EmployeeRoleDegreeIdentityDTO[] employeeRoleDegreeIdents, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetListByIdents(employeeRoleDegreeIdents, evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
        
        protected virtual System.Collections.Generic.IEnumerable<SampleSystem.Generated.DTO.EmployeeRoleDegreeVisualDTO> GetVisualEmployeeRoleDegreesInternal(Framework.DomainDriven.ServiceModel.Service.EvaluatedData<SampleSystem.BLL.ISampleSystemBLLContext, SampleSystem.Generated.DTO.ISampleSystemDTOMappingService> evaluateData)
        {
            SampleSystem.BLL.IEmployeeRoleDegreeBLL bll = evaluateData.Context.Logics.EmployeeRoleDegreeFactory.Create(Framework.SecuritySystem.SecurityRule.View);
            return SampleSystem.Generated.DTO.LambdaHelper.ToVisualDTOList(bll.GetFullList(evaluateData.Context.FetchService.GetContainer<SampleSystem.Domain.EmployeeRoleDegree>(Framework.Transfering.ViewDTOType.VisualDTO)), evaluateData.MappingService);
        }
    }
}
