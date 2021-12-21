using System.Collections.Generic;

using Framework.Authorization.BLL;
using Framework.Authorization.Domain;

using Framework.Core;
using Framework.Events;

namespace Framework.Authorization.Events
{
    public class DefaultAuthDALListener : DependencyDetailEventDALListener<IAuthorizationBLLContext, PersistentDomainObjectBase>
    {
        public DefaultAuthDALListener(IAuthorizationBLLContext context, IList<TypeEvent> typeEvents, IMessageSender<IDomainOperationSerializeData<PersistentDomainObjectBase>> messageSender, IList<TypeEventDependency> dependencies)
            : base(context, typeEvents, messageSender, dependencies)
        {
        }
    }
}
