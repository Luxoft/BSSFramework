namespace Framework.Attachments.WebApi
{
    using Framework.Attachments.Generated.DTO;
    
    
    [Microsoft.AspNetCore.Mvc.ApiControllerAttribute()]
    [Microsoft.AspNetCore.Mvc.ApiVersionAttribute("1.0")]
    [Microsoft.AspNetCore.Mvc.RouteAttribute("AttachmentsApi/v{version:apiVersion}/[controller]")]
    public partial class AttachmentController : Framework.DomainDriven.WebApiNetCore.ApiControllerBase<Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Attachments.BLL.IAttachmentsBLLContext>, Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Generated.DTO.IAttachmentsDTOMappingService>>
    {
        
        public AttachmentController(Framework.DomainDriven.ServiceModel.Service.IServiceEnvironment<Framework.Attachments.BLL.IAttachmentsBLLContext> serviceEnvironment, Framework.Exceptions.IExceptionProcessor exceptionProcessor) : 
                base(serviceEnvironment, exceptionProcessor)
        {
        }
        
        protected override Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Generated.DTO.IAttachmentsDTOMappingService> GetEvaluatedData(Framework.DomainDriven.BLL.IDBSession session, Framework.Attachments.BLL.IAttachmentsBLLContext context)
        {
            return new Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Generated.DTO.IAttachmentsDTOMappingService>(session, context, new AttachmentsServerPrimitiveDTOMappingService(context));
        }
        
        protected virtual void RemoveAttachmentInternal(Framework.Attachments.Generated.DTO.AttachmentIdentityDTO attachmentIdent, Framework.DomainDriven.ServiceModel.Service.EvaluatedData<Framework.Attachments.BLL.IAttachmentsBLLContext, Framework.Attachments.Generated.DTO.IAttachmentsDTOMappingService> evaluateData, Framework.Attachments.BLL.IAttachmentBLL bll)
        {
            Framework.Attachments.Domain.Attachment domainObject = bll.GetById(attachmentIdent.Id, true);
            bll.Remove(domainObject);
        }
    }
}
