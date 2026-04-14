using Framework.Application.ApplicationVariable;
using Framework.Configuration.Domain;
using Framework.Tracking;

using SecuritySystem.Providers;

namespace Framework.Configuration.BLL;

public partial class SystemConstantBLL(
    IConfigurationBLLContext context,
    ISecurityProvider<SystemConstant> securityProvider,
    ITrackingService<PersistentDomainObjectBase> trackingService)
    : SecurityDomainBLLBase<SystemConstant>(context, securityProvider)
{
    public override void Save(SystemConstant systemConstant)
    {
        if (systemConstant == null) throw new ArgumentNullException(nameof(systemConstant));

        if (!systemConstant.IsManual && trackingService.GetChanges(systemConstant).HasChange(e => e.Value, e => e.Description))
        {
            systemConstant.IsManual = true;
        }

        base.Save(systemConstant);
    }

    public T GetValue<T>(ApplicationVariable<T> applicationVariable)
    {
        if (applicationVariable == null) throw new ArgumentNullException(nameof(applicationVariable));

        var systemConstant = this.GetObjectBy(sc => sc.Code == applicationVariable.Name, true)!;

        var serializer = this.Context.SystemConstantSerializerFactory.Create<T>();

        return serializer.Parse(systemConstant.Value);
    }
}
