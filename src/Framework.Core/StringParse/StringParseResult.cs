using System;
using System.Text.RegularExpressions;

namespace Framework.Core.StringParse;

[Obsolete("v10 This method will be protected in future")]
public class StringParseResult
{
    private readonly Match _math;

    internal StringParseResult(Match math)
    {
        this._math = math;
    }

    public string GetResultFor(MatchResultDescription mathResultDescription)
    {
        return this._math.Groups[mathResultDescription.ResultIndex].Value;
    }
}
