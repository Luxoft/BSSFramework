using Framework.DomainDriven;
using Framework.DomainDriven.Serialization;

namespace Framework.Configuration.Domain
{
    [DirectMode(DirectMode.Out | DirectMode.In)]
    public class RegularJobRevisionModel : DomainObjectBase, IDomainObjectExtendedModel<RegularJob>
    {
        private readonly RegularJob _regularJob;
        private readonly long _revisionNumber;
        private readonly ExecuteRegularJobResult _executionResult;

        public RegularJobRevisionModel()
        {
        }

        public RegularJobRevisionModel(RegularJob regularJob, long revisionNumber)
        {
            this._regularJob = regularJob;
            this._revisionNumber = revisionNumber;
            this._executionResult = regularJob.ExecutionResult;
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public RegularJob ExtendedObject
        {
            get { return this._regularJob; }
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public long RevisionNumber
        {
            get { return this._revisionNumber; }
        }

        [CustomSerialization(CustomSerializationMode.ReadOnly)]
        public ExecuteRegularJobResult ExecutionResult
        {
            get { return this._executionResult; }
        }
    }
}
