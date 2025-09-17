using System.Reflection;

using CommonFramework;

using Framework.ApplicationVariable;
using Framework.Configuration.Domain;
using Framework.Core;
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

    public IList<SystemConstant> Initialize(Type systemConstantContainerType)
    {
        if (systemConstantContainerType == null) throw new ArgumentNullException(nameof(systemConstantContainerType));

        var currentConstants = this.GetFullList();

        var initMethod = new Func<ApplicationVariable<object>, IList<SystemConstant>, SystemConstant>(this.Initialize).Method.GetGenericMethodDefinition();

        var request = from field in systemConstantContainerType.GetFields(BindingFlags.Static | BindingFlags.Public)

                      let systemConstant = field.GetValue(null)

                      where systemConstant != null

                      let constType = systemConstant.GetType().GetGenericTypeImplementationArgument(typeof(ApplicationVariable<>))

                      where constType != null

                      select initMethod.MakeGenericMethod(constType).Invoke<SystemConstant>(this, systemConstant, currentConstants);


        return request.ToList();
    }

    private SystemConstant Initialize<T>(ApplicationVariable<T> typedSystemConstant, IList<SystemConstant> systemConstants)
    {
        if (typedSystemConstant == null) throw new ArgumentNullException(nameof(typedSystemConstant));
        if (systemConstants == null) throw new ArgumentNullException(nameof(systemConstants));

        var systemConstant = systemConstants.SingleOrDefault(sc => string.Equals(sc.Code, typedSystemConstant.Name, StringComparison.CurrentCultureIgnoreCase))
                             ?? new SystemConstant { Code = typedSystemConstant.Name, Type = this.Context.Logics.DomainType.GetByType(typeof(T)) };

        if (!systemConstant.IsManual)
        {
            var serializer = this.Context.SystemConstantSerializerFactory.Create<T>();

            var serializedValue = serializer.Format(typedSystemConstant.DefaultValue);

            if (systemConstant.IsNew || (systemConstant.Description != typedSystemConstant.Description || systemConstant.Value != serializedValue))
            {
                systemConstant.Description = typedSystemConstant.Description;
                systemConstant.Value = serializedValue;

                base.Save(systemConstant);
            }
        }

        return systemConstant;
    }
}
