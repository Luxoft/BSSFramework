using System.Data.SqlTypes;

using Framework.Core;

namespace Framework.BLL.Validation;

public static class AvailableValuesHelper
{
    private static readonly decimal DecimalLimit = (decimal)Math.Pow(10, 15) - 1M;

    public static readonly AvailableValues AvailableValues = new(
        new Range<decimal>(-DecimalLimit, DecimalLimit),
        new Range<DateTime>(SqlDateTime.MinValue.Value, SqlDateTime.MaxValue.Value),
        byte.MaxValue);
}
