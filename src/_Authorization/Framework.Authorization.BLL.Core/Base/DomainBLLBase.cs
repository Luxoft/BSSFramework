using System;

using Framework.Authorization.Domain;
using Framework.Validation;

namespace Framework.Authorization.BLL
{
    public partial class DomainBLLBase<TDomainObject, TOperation>
    {
        private void ExecuteBasePersist(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.Validate(domainObject, AuthorizationOperationContext.Save);
        }


        protected internal virtual void Validate(TDomainObject domainobject, AuthorizationOperationContext context)
        {
            this.Context.Validator.Validate(domainobject, (int)context);
        }

        internal void Save(TDomainObject domainobject, bool validate)
        {
            if (domainobject == null) throw new ArgumentNullException(nameof(domainobject));

            if (validate) { this.Save(domainobject); }
            else          { base.Save(domainobject); }
        }

        public override void Insert(TDomainObject domainobject, Guid id)
        {
            if (domainobject == null) throw new ArgumentNullException(nameof(domainobject));

            this.ExecuteBasePersist(domainobject);
            base.Insert(domainobject, id);
        }

        public override void Save(TDomainObject domainobject)
        {
            if (domainobject == null) throw new ArgumentNullException(nameof(domainobject));

            this.ExecuteBasePersist(domainobject);
            base.Save(domainobject);
        }
    }
}