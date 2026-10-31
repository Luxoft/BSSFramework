namespace Framework.Subscriptions.Metadata;

/// <summary>
/// Enumeration of principles for combining mailing recipients
/// </summary>
/// <remarks>
/// Recipients Mode explains how the system should obtain recipients when both types are filled
/// </remarks>
public enum RecipientMergeType
{
    /// <summary>Union of the lists</summary>
    Union,

    /// <summary>Common intersection of the lists</summary>
    Intersect,

    /// <summary>The left list excluding the recipients specified in the right list.</summary>
    LeftExceptRight,

    /// <summary>The right list excluding the recipients specified in the left list.</summary>
    RightExceptLeft
}
