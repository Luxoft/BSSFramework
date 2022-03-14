using Framework.Attachments.BLL;
using Framework.Configuration.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IAttachmentServiceEnvironmentModule
{
    IAttachmentBLLContextModule CreateContextModule(IConfigurationBLLContext configurationBllContext);
}
