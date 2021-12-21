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


        protected internal virtual void PreValidate(TDomainObject domainobject, AuthorizationOperationContext operationContext)
        {
            this.Context.Validator.Validate(domainobject, (int)operationContext);
        }

        protected internal virtual void PostValidate(TDomainObject domainobject, AuthorizationOperationContext operationContext)
        {

        }

        protected internal virtual void PreRecalculate(TDomainObject domainobject)
        {

        }

        protected internal virtual void PostRecalculate(TDomainObject domainobject)
        {

        }

        internal protected void Save(TDomainObject domainobject, bool validate)
        {
            if (domainobject == null) throw new ArgumentNullException(nameof(domainobject));

            if (validate) { this.Save(domainobject); }
            else { base.Save(domainobject); }
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