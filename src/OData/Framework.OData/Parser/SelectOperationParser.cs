using System.Globalization;

using CommonFramework;

using Framework.Core.Serialization;
using Framework.OData.Parser.Parsing;

namespace Framework.OData.Parser;

public class SelectOperationParser(NumberFormatInfo numberFormatInfo) : IParser<string, SelectOperation>
{
    public readonly NumberFormatInfo NumberFormatInfo = numberFormatInfo;

    public SelectOperation Parse(string input) => this.SafeParse(input) ?? throw this.GetParsingError(input);

    protected virtual Exception GetParsingError(string input) => new($"Can't parse input: {input}");

    private SelectOperation? SafeParse(string text) => new SelectOperationInternalParser(this.NumberFormatInfo).MainParser(text ?? "").Maybe(v => v.Value);

    public static readonly SelectOperationParser Default = new SelectOperationParser(CultureInfo.CurrentCulture.NumberFormat);

    public static readonly IParser<string, SelectOperation> CachedDefault = Default.WithCache().WithLock();
}
