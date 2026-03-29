using System.Text.RegularExpressions;

namespace Framework.Core.StringParse;

[Obsolete("v10 This method will be protected in future")]
public class StringParseResult
{
    private readonly Match math;

    internal StringParseResult(Match math) => this.math = math;

    public string GetResultFor(MatchResultDescription mathResultDescription) => this.math.Groups[mathResultDescription.ResultIndex].Value;
}
