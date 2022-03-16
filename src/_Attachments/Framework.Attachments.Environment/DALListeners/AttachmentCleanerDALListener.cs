using System;
using System.Linq;

using Framework.Attachments.BLL;
using Framework.Core;
using Framework.DomainDriven.BLL;

using JetBrains.Annotations;

namespace Framework.Attachments.Environment
{
    public class AttachmentCleanerDALListener : IDALListener
    {
        private readonly ITargetSystemService _targetSystemService;


        public AttachmentCleanerDALListener([NotNull] ITargetSystemService targetSystemService)
        {
            this._targetSystemService = targetSystemService ?? throw new ArgumentNullException(nameof(targetSystemService));
        }


        public void Process(DALChangesEventArgs eventArgs)
        {
            if (eventArgs == null) throw new ArgumentNullException(nameof(eventArgs));

            if (eventArgs.Changes.RemovedItems.Any())
            {
                foreach (var pair in eventArgs.Changes.GroupByType().Where(pair => this._targetSystemService.IsAssignable(pair.Key)))
                {
                    var removeObjects = pair.Value.RemovedItems.ToArray(pair.Key);

                    this._targetSystemService.TryRemoveAttachments(removeObjects);
                }
            }
        }
    }
}
