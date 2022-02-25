using System;
using System.Linq;

using Framework.Core;
using Framework.Persistent;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;
using Framework.Validation;
using Framework.Exceptions;

namespace Framework.Configuration.BLL
{
    public partial class SubscriptionBLL : BLLContextContainer<IConfigurationBLLContext>
    {
        public SubscriptionBLL(IConfigurationBLLContext context)
                : base(context)
        {
        }

        public bool HasActiveSubscriptions(Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return this.Context.GetDomainType(type, false).Maybe(domainType => this.HasActiveSubscriptions(domainType));
        }

        public bool HasActiveSubscriptions(DomainType domainType)
        {
            if (domainType == null) throw new ArgumentNullException(nameof(domainType));

            return domainType.TargetSystem.SubscriptionEnabled;
        }
    }
}
