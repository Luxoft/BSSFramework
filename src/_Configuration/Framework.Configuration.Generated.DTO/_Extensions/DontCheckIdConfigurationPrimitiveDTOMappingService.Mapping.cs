using Framework.Configuration.BLL;
using Framework.DomainDriven;
using Framework.DomainDriven.Lock;

namespace Framework.Configuration.Generated.DTO;

public class DontCheckIdConfigurationPrimitiveDTOMappingService : ConfigurationServerPrimitiveDTOMappingServiceBase
{
    public DontCheckIdConfigurationPrimitiveDTOMappingService(IConfigurationBLLContext context)
            : base(context)
    {

    }


    public override TDomainObject GetById<TDomainObject>(Guid ident, IdCheckMode checkMode = IdCheckMode.SkipEmpty, LockRole lockRole = LockRole.None)
    {
        return base.GetById<TDomainObject>(ident, IdCheckMode.DontCheck);
    }
}
