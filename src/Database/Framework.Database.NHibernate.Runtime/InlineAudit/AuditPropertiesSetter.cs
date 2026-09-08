using Framework.Database.InlineAudit;

namespace Framework.Database.NHibernate.InlineAudit
{
    public class AuditPropertiesSetter(IAuditValueResolver<object> auditValueResolver, int propertyIndex) : IAuditPropertiesSetter
    {
        public bool SetAuditFields(object[] state)
        {
            state[propertyIndex] = auditValueResolver.GetCurrentValue();

            return true;
        }
    }
}
