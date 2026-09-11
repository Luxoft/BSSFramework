using Anch.Core;

namespace Framework.Database.NHibernate.InlineAudit;

public static class AuditPropertiesSetterMapExtensions
{
    public static IAuditPropertiesSetterMap Aggregate(this IEnumerable<IAuditPropertiesSetterMap> source) =>
        source.Match(() => EmptyAuditPropertiesSetterMap.Instance, single => single, multiple => new AggregateAuditPropertiesSetterMap(multiple));

    private class AggregateAuditPropertiesSetterMap(IAuditPropertiesSetterMap[] items) : IAuditPropertiesSetterMap
    {
        public IAuditPropertiesSetter CreateSetter(IServiceProvider serviceProvider) =>
            new AggregateAuditPropertiesSetter(items.Select(item => item.CreateSetter(serviceProvider)));
    }

    private class AggregateAuditPropertiesSetter(IEnumerable<IAuditPropertiesSetter> setters) : IAuditPropertiesSetter
    {
        public bool SetAuditFields(object[] state) =>

            setters.Select(setter => setter.SetAuditFields(state))
                   .ToArray() // Force side effects before evaluating the result
                   .Any(v => v);
    }
}
