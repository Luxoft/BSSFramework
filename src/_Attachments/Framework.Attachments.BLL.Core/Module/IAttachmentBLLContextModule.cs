using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;

namespace Framework.Attachments.BLL;

public interface IAttachmentBLLContextModule : IBLLContextContainer<IConfigurationBLLContext>
{
    ITargetSystemService GetPersistentTargetSystemService(TargetSystem targetSystem);
}
