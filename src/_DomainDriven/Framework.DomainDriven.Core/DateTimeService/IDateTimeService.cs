using Framework.Core;

namespace Framework.DomainDriven;

public interface IDateTimeService
{
    /// <summary>
    /// Return Now value
    /// </summary>
    DateTime Now { get; }

    /// <summary>
    /// Return Now value without time
    /// </summary>
    DateTime Today { get; }

    DateTime UtcNow { get; }

    Period CurrentFinancialYear { get; }

    Period CurrentMonth { get; }

    Period CurrentYear { get; }

    Period NextMonth { get; }

    Period PrevMonth { get; }
}
