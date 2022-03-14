using System.Collections.Generic;

using Framework.Attachments.BLL;
using Framework.Configuration.BLL;

using ITargetSystemService = Framework.Attachments.BLL.ITargetSystemService;

namespace Framework.DomainDriven.ServiceModel.IAD;

public interface IAttachmentServiceEnvironmentModule
{
    IAttachmentBLLContextModule CreateContextModule(IConfigurationBLLContext configurationBllContext);
}

public interface IAttachmentServiceEnvironmentModule<in TBLLContextContainer> : IAttachmentServiceEnvironmentModule
{

}

public class AttachmentServiceEnvironmentModule<TBLLContextContainer> : IAttachmentServiceEnvironmentModule
{
    public IAttachmentBLLContextModule CreateContextModule(IConfigurationBLLContext configurationBllContext)
    {
        return new AttachmentBLLContextModule()
    }

    protected virtual IEnumerable<ITargetSystemService> GetTargetSystemServices()
    {

    }
}
