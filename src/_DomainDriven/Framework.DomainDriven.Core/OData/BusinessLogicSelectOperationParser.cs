using System;
using System.Globalization;

using Framework.Core;
using Framework.Core.Serialization;
using Framework.OData;

namespace Framework.DomainDriven.BLL;

public class BusinessLogicSelectOperationParser : SelectOperationParser
{
    public BusinessLogicSelectOperationParser(NumberFormatInfo numberFormatInfo)
            : base(numberFormatInfo)
    {

    }


    protected override Exception GetParsingError(string input)
    {
        return base.GetParsingError(input).Message.Pipe(baseMessage =>

                                                                new Framework.Exceptions.BusinessLogicException(baseMessage));
    }


    public new static readonly BusinessLogicSelectOperationParser Default = new BusinessLogicSelectOperationParser(SelectOperationParser.Default.NumberFormatInfo);

    public new static readonly IParser<string, SelectOperation> CachedDefault = Default.WithCache().WithLock();
}
