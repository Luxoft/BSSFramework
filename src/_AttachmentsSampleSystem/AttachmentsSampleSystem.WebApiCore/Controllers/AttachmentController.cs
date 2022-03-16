using Framework.Attachments.BLL;
using Framework.DomainDriven.ServiceModel.Service;
using Framework.Exceptions;

namespace AttachmentsSampleSystem.WebApiCore.Controllers
{
    public class AttachmentController : Framework.Attachments.WebApi.AttachmentController
    {
        public AttachmentController(IServiceEnvironment<IAttachmentsBLLContext> serviceEnvironment, IExceptionProcessor exceptionProcessor) : base(serviceEnvironment, exceptionProcessor)
        {
        }
    }
}
