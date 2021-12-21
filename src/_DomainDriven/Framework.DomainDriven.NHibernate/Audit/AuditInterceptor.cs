using System.Collections.Generic;

using Framework.DomainDriven.Audit;
using Framework.Persistent;

using NHibernate;
using NHibernate.Type;

namespace Framework.DomainDriven.NHibernate.Audit
{
    /// <summary> NHibernate Interceptor for setting Audit properties (<seealso cref="IAuditProperty"/>) on insert\update domain object
    /// </summary>
    internal sealed class AuditInterceptor : EmptyInterceptor
    {
        private readonly AuditPropertiesSetter createSetter;
        private readonly AuditPropertiesSetter modifySetter;

        public AuditInterceptor(IEnumerable<IAuditProperty> createAuditProperties, IEnumerable<IAuditProperty> modifyAuditProperties)
        {
            this.createSetter = new AuditPropertiesSetter(createAuditProperties);
            this.modifySetter = new AuditPropertiesSetter(modifyAuditProperties);
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState, string[] propertyNames, IType[] types)
        {
            bool result = false;
            if (entity is IAuditObject)
            {
                result =
                    this.modifySetter.SetAuditFields(
                        AuditPropertiesSetter.DomainObjectDescription.Get(entity.GetType(), propertyNames),
                        ref currentState);
            }

            return result;
        }

        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            bool result = false;
            if (entity is IAuditObject)
            {
                var domainObjectDescription = AuditPropertiesSetter.DomainObjectDescription.Get(entity.GetType(), propertyNames);
                bool createSetterRes = this.createSetter.SetAuditFields(domainObjectDescription, ref state);
                bool modifySetterRes = this.modifySetter.SetAuditFields(domainObjectDescription, ref state);
                result = createSetterRes | modifySetterRes;
            }

            return result;
        }
    }
}
