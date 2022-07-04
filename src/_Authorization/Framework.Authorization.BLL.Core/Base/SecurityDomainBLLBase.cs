using System;

using Framework.Authorization.Domain;
using Framework.Validation;

namespace Framework.Authorization.BLL
{
    public abstract partial class SecurityDomainBLLBase<TDomainObject, TOperation>
    {
        private void ExecuteBasePersist(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.PreValidate(domainObject, AuthorizationOperationContext.Save);
            this.PreRecalculate(domainObject);

            this.PostValidate(domainObject, AuthorizationOperationContext.Save);
            this.PostRecalculate(domainObject);
        }


        protected internal virtual void PreValidate(TDomainObject domainObject, AuthorizationOperationContext operationContext)
        {
            this.Context.Validator.Validate(domainObject, (int)operationContext);
        }

        protected internal virtual void PostValidate(TDomainObject domainObject, AuthorizationOperationContext operationContext)
        {

        }

        protected internal virtual void PreRecalculate(TDomainObject domainObject)
        {

        }

        protected internal virtual void PostRecalculate(TDomainObject domainObject)
        {

        }

        internal protected void Save(TDomainObject domainObject, bool validate)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            if (validate) { this.Save(domainObject); }
            else { base.Save(domainObject); }
        }

        public override void Insert(TDomainObject domainObject, Guid id)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.ExecuteBasePersist(domainObject);
            base.Insert(domainObject, id);
        }

        public override void Save(TDomainObject domainObject)
        {
            if (domainObject == null) throw new ArgumentNullException(nameof(domainObject));

            this.ExecuteBasePersist(domainObject);
            base.Save(domainObject);
        }
    }
}
