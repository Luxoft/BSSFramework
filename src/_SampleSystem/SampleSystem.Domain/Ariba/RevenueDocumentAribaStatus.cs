using System.Runtime.Serialization;

using Framework.Restriction;

namespace SampleSystem.Domain.Ariba;

[DataContract(Namespace = "")]
public struct RevenueDocumentAribaStatus(AribaStatusType type, string description, DateTime? date) : IEquatable<RevenueDocumentAribaStatus>
{
    private AribaStatusType type = type;
    private string description = description;
    private DateTime? date = date;

    [DataMember]
    public AribaStatusType Type
    {
        get => this.type;
        set => this.type = value;
    }

    [DataMember]
    [MaxLength(int.MaxValue)]
    public string Description
    {
        get => this.description;
        set => this.description = value;
    }

    [DataMember]
    public DateTime? Date
    {
        get => this.date;
        set => this.date = value;
    }

    public static RevenueDocumentAribaStatus CreateErrorStatus(string description, DateTime? date) => new(AribaStatusType.ErrorSending, description, date);

    public static RevenueDocumentAribaStatus CreateProcessingStatus(DateTime? date) => Create(AribaStatusType.Processing, date);

    public static RevenueDocumentAribaStatus CreateSentStatus(DateTime? date) => Create(AribaStatusType.SentToAriba, date);

    public static RevenueDocumentAribaStatus CreateApprovedStatus(DateTime? date) => Create(AribaStatusType.Approved, date);

    public static RevenueDocumentAribaStatus CreatePaidStatus(DateTime? date) => Create(AribaStatusType.Paid, date);

    public static RevenueDocumentAribaStatus CreateRejectedStatus(DateTime? date) => Create(AribaStatusType.Rejected, date);

    public static RevenueDocumentAribaStatus CreateFailedStatus(DateTime? date) => Create(AribaStatusType.Invalid, date);

    public static RevenueDocumentAribaStatus Create(AribaStatusType type, DateTime? date) => Create(type, string.Empty, date);

    public static RevenueDocumentAribaStatus CreateDefault() => Create(AribaStatusType.UnSynchronized, string.Empty, null);

    public static RevenueDocumentAribaStatus Create(AribaStatusType status, string desctiption, DateTime? date) => new(status, desctiption, date);

    public static bool operator !=(RevenueDocumentAribaStatus arg1, RevenueDocumentAribaStatus arg2) => !arg1.Equals(arg2);

    public static bool operator ==(RevenueDocumentAribaStatus arg1, RevenueDocumentAribaStatus arg2) => !(arg1 != arg2);

    public override bool Equals(object obj) => obj is RevenueDocumentAribaStatus && this.Equals((RevenueDocumentAribaStatus)obj);

    public bool Equals(RevenueDocumentAribaStatus target) => this.type == target.type && this.date == target.date && this.description == target.description;

    public override int GetHashCode() => this.type.GetHashCode() ^ this.date.GetHashCode() ^ this.description.GetHashCode();
}
