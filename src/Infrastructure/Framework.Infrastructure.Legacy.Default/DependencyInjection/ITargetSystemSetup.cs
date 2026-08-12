using Framework.BLL.Domain.TargetSystem;

namespace Framework.Infrastructure.DependencyInjection;

public interface ITargetSystemSetup
{
    bool RegisterBase { get; set; }

    bool RegisterAuthorization { get; set; }

    bool RegisterConfiguration { get; set; }

    ITargetSystemSetup AddTargetSystem(PersistentTargetSystemInfo targetSystemInfo);
}
