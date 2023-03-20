using SampleSystem.Domain.Inline;

namespace SampleSystem.Domain;

public enum AribaStatusType
{
    /// <summary>
    /// Statuses of sending 2 BZ
    /// </summary>
    UnSynchronized = 0,
    SendingToAriba = 1,
    ErrorSending = 2,
    SentToAriba = 3,
    /// <summary>
    /// Statuses in ariba
    /// </summary>
    Processing = 4,
    Approved = 5,
    Paying = 6,
    Paid = 7,
    Rejected = 8,
    Invalid = 9,
    Manual = 10
}
