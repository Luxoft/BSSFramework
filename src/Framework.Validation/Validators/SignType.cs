using System;
using System.Linq;

namespace Framework.Validation;

[Flags]
public enum SignType
{
    Positive = 1,

    Zero = 2,

    Negative = 4,

    ZeroAndPositive = Positive + Zero
}

public static class SignTypeExtensions
{
    public static SignType Inverse(this SignType source)
    {
        var result = new[]
                     {
                             new {Source = SignType.Negative, Target = SignType.Positive},
                             new {Source = SignType.Positive, Target = SignType.Negative},
                             new {Source = SignType.Zero, Target = SignType.Zero},
                     }
                     .Where(z => source.HasFlag(z.Source))
                     .Select(z => z.Target)
                     .Aggregate((prev, current) => prev | current);

        return result;
    }

    public static decimal ToValue(this SignType source)
    {
        switch (source)
        {
            case SignType.Negative: return -1;
            case SignType.Positive: return 1;
            case SignType.Zero: return 0;
            default: throw new ArgumentException(source.ToString());
        }
    }
}
