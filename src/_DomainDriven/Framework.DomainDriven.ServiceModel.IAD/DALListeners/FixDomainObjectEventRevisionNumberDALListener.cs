using System;
using System.Linq;

using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class FixDomainObjectEventRevisionNumberDALListener : BLLContextContainer<IConfigurationBLLContext>, IBeforeTransactionCompletedDALListener
{
    public FixDomainObjectEventRevisionNumberDALListener(IConfigurationBLLContext context)
            : base(context)
    {
    }


    public void Process(DALChangesEventArgs eventArgs)
    {
        if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

        var eventDalChanges = eventArgs.Changes.GetSubset(typeof(DomainObjectEvent));

        if (!eventDalChanges.IsEmpty)
        {
            var revisionNumber = this.Context.GetCurrentRevision();

            var domainObjectEventBLL = this.Context.Logics.DomainObjectEvent;

            var request = from dalObject in eventDalChanges.CreatedItems

                          let eventObject = (DomainObjectEvent)dalObject.Object

                          where eventObject.Revision == 0

                          select eventObject;

            foreach (var eventObject in request)
            {
                eventObject.Revision = revisionNumber;

                domainObjectEventBLL.Save(eventObject);
            }
        }
    }
}
