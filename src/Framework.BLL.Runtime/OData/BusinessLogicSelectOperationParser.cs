using System.Globalization;

using CommonFramework;

using Framework.BLL.Domain.Exceptions.BusinessLogic._Base;
using Framework.Core.Serialization;
using Framework.OData;

namespace Framework.BLL.OData;

public class BusinessLogicSelectOperationParser : SelectOperationParser
{
    public BusinessLogicSelectOperationParser(NumberFormatInfo numberFormatInfo)
            : base(numberFormatInfo)
    {

    }


    protected override Exception GetParsingError(string input)
    {
        return base.GetParsingError(input).Message.Pipe(baseMessage => new BusinessLogicException(baseMessage));
    }


    public new static readonly BusinessLogicSelectOperationParser Default = new BusinessLogicSelectOperationParser(SelectOperationParser.Default.NumberFormatInfo);

    public new static readonly IParser<string, SelectOperation> CachedDefault = Default.WithCache().WithLock();
}
