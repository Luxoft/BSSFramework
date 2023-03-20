using System;
using System.Globalization;

using Framework.Core;
using Framework.Core.Serialization;

namespace Framework.OData;

public class SelectOperationParser : IParser<string, SelectOperation>
{
    public readonly NumberFormatInfo NumberFormatInfo;


    public SelectOperationParser(NumberFormatInfo numberFormatInfo)
    {
        if (numberFormatInfo == null) throw new ArgumentNullException(nameof(numberFormatInfo));

        this.NumberFormatInfo = numberFormatInfo;
    }


    public SelectOperation Parse(string input)
    {
        return this.SafeParse(input).FromMaybe(() => this.GetParsingError(input));
    }

    protected virtual Exception GetParsingError(string input)
    {
        return new Exception($"Can't parse input: {input}");
    }

    private SelectOperation SafeParse(string text)
    {
        return new SelectOperationInternalParser(this.NumberFormatInfo).MainParser(text ?? "").Maybe(v => v.Value);
    }



    public static readonly SelectOperationParser Default = new SelectOperationParser(CultureInfo.CurrentCulture.NumberFormat);

    public static readonly IParser<string, SelectOperation> CachedDefault = Default.WithCache().WithLock();
}
