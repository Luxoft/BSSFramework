using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.DomainDriven.BLL;

namespace Framework.DomainDriven.ServiceModel.IAD;

public class FixDomainObjectEventRevisionNumberDALListener(IConfigurationBLLContext context) : BLLContextContainer<IConfigurationBLLContext>(context),
                                                                                               IBeforeTransactionCompletedDALListener
{
    public async Task Process(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
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