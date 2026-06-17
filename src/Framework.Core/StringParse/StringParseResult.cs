using System.Text.RegularExpressions;

namespace Framework.Core.StringParse;

[Obsolete("v10 This method will be protected in future")]
public class StringParseResult
{
    private readonly Match match;

    internal StringParseResult(Match match) => this.match = match;

    public string GetResultFor(MatchResultDescription matchResultDescription) => this.match.Groups[matchResultDescription.ResultIndex].Value;
}

