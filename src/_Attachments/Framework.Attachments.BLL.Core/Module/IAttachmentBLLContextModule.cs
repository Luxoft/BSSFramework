using System.Collections.Generic;

using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.BLL.Configuration;
using Framework.DomainDriven.BLL.Security;

namespace Framework.Attachments.BLL;

public interface IAttachmentBLLContextModule : IConfigurationBLLContextContainer<Framework.Configuration.BLL.IConfigurationBLLContext>, IAuthorizationBLLContextContainer<IAuthorizationBLLContextBase>
{
    ITargetSystemService GetPersistentTargetSystemService(TargetSystem targetSystem);

    IEnumerable<ITargetSystemService> GetPersistentTargetSystemServices();
}
