using Framework.BLL;
using Framework.Configuration.BLL;
using Framework.Configuration.Domain;
using Framework.Database;
using Framework.Database.DALListener;

namespace Framework.Infrastructure.DALListeners;

public class FixDomainObjectEventRevisionNumberDALListener(IConfigurationBLLContext context) : BLLContextContainer<IConfigurationBLLContext>(context),
                                                                                               IBeforeTransactionCompletedDALListener
{
    public async Task Process(DALChangesEventArgs eventArgs, CancellationToken cancellationToken)
    {
        if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

        var eventDALChanges = eventArgs.Changes.GetSubset(typeof(DomainObjectEvent));

        if (!eventDALChanges.IsEmpty)
        {
            var revisionNumber = this.Context.GetCurrentRevision();

            var domainObjectEventBLL = this.Context.Logics.DomainObjectEvent;

            var request = from dalObject in eventDALChanges.CreatedItems

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
