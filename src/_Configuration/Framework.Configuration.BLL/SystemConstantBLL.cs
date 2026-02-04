using Framework.ApplicationVariable;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;

namespace Framework.Configuration.BLL;

public partial class SystemConstantBLL
{
    public override void Save(SystemConstant systemConstant)
    {
        if (systemConstant == null) throw new ArgumentNullException(nameof(systemConstant));

        if (!systemConstant.IsManual && this.Context.TrackingService.GetChanges(systemConstant).HasChange(e => e.Value, e => e.Description))
        {
            systemConstant.IsManual = true;
        }

        base.Save(systemConstant);
    }

    public T GetValue<T>(ApplicationVariable<T> applicationVariable)
    {
        if (applicationVariable == null) throw new ArgumentNullException(nameof(applicationVariable));

        var systemConstant = this.GetByCode(applicationVariable.Name, true);

        var serializer = this.Context.SystemConstantSerializerFactory.Create<T>();

        return serializer.Parse(systemConstant.Value);
    }

}
