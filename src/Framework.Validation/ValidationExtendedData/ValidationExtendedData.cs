using System.Collections.Generic;

using Framework.Core;

namespace Framework.Validation;

public class ValidationExtendedData : DynamicSource
{
    private readonly IAvailableValues _availableValues;

    public ValidationExtendedData(IAvailableValues availableValues)
    {
        this._availableValues = availableValues;
    }


    protected override IEnumerable<object> GetItems()
    {
        foreach (var item in base.GetItems())
        {
            yield return item;
        }

        yield return this._availableValues;
    }


    public static readonly ValidationExtendedData Infinity = new ValidationExtendedData(AvailableValues.Infinity);
}
