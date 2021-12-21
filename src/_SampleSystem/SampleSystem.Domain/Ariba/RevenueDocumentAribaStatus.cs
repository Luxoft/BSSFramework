using System;
using System.Runtime.Serialization;

using Framework.Restriction;

namespace SampleSystem.Domain
{
    [DataContract(Namespace = "")]
    public struct RevenueDocumentAribaStatus : IEquatable<RevenueDocumentAribaStatus>
    {
        private AribaStatusType type;
        private string description;
        private DateTime? date;

        public RevenueDocumentAribaStatus(AribaStatusType type, string description, DateTime? date) : this()
        {
            this.type = type;
            this.description = description;
            this.date = date;
        }

        [DataMember]
        public AribaStatusType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        [DataMember]
        [MaxLength(int.MaxValue)]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [DataMember]
        public DateTime? Date
        {
            get { return this.date; }
            set { this.date = value; }
        }

        public static RevenueDocumentAribaStatus CreateErrorStatus(string description, DateTime? date)
        {
            return new RevenueDocumentAribaStatus(AribaStatusType.ErrorSending, description, date);
        }

        public static RevenueDocumentAribaStatus CreateProcessingStatus(DateTime? date)
        {
            return Create(AribaStatusType.Processing, date);
        }

        public static RevenueDocumentAribaStatus CreateSentStatus(DateTime? date)
        {
            return Create(AribaStatusType.SentToAriba, date);
        }

        public static RevenueDocumentAribaStatus CreateApprovedStatus(DateTime? date)
        {
            return Create(AribaStatusType.Approved, date);
        }

        public static RevenueDocumentAribaStatus CreatePaidStatus(DateTime? date)
        {
            return Create(AribaStatusType.Paid, date);
        }

        public static RevenueDocumentAribaStatus CreateRejectedStatus(DateTime? date)
        {
            return Create(AribaStatusType.Rejected, date);
        }

        public static RevenueDocumentAribaStatus CreateFailedStatus(DateTime? date)
        {
            return Create(AribaStatusType.Invalid, date);
        }

        public static RevenueDocumentAribaStatus Create(AribaStatusType type, DateTime? date)
        {
            return Create(type, string.Empty, date);
        }

        public static RevenueDocumentAribaStatus CreateDefault()
        {
            return Create(AribaStatusType.UnSynchronized, string.Empty, null);
        }

        public static RevenueDocumentAribaStatus Create(AribaStatusType status, string desctiption, DateTime? date)
        {
            return new RevenueDocumentAribaStatus(status, desctiption, date);
        }

        public static bool operator !=(RevenueDocumentAribaStatus arg1, RevenueDocumentAribaStatus arg2)
        {
            return !arg1.Equals(arg2);
        }

        public static bool operator ==(RevenueDocumentAribaStatus arg1, RevenueDocumentAribaStatus arg2)
        {
            return !(arg1 != arg2);
        }

        public override bool Equals(object obj)
        {
            return obj is RevenueDocumentAribaStatus && this.Equals((RevenueDocumentAribaStatus)obj);
        }

        public bool Equals(RevenueDocumentAribaStatus target)
        {
            return this.type == target.type && this.date == target.date && this.description == target.description;
        }

        public override int GetHashCode()
        {
            return this.type.GetHashCode() ^ this.date.GetHashCode() ^ this.description.GetHashCode();
        }
    }
}
