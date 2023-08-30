using Framework.Core;

namespace Framework.DomainDriven.BLL;

public static class BLLBaseOperationExtensions
{
    internal static TTargetOperation ToOperation<TSourceOperation, TTargetOperation>(this TSourceOperation source)
            where TSourceOperation : struct, Enum
            where TTargetOperation : struct, Enum
    {
        return source.ToOperationMaybe<TSourceOperation, TTargetOperation>().GetValue(() =>

                                                                                              new InvalidCastException($"Type \"{typeof(TTargetOperation).Name}\" must have \"{source.ToString()}\" field"));
    }

    public static Maybe<TTargetOperation> ToOperationMaybe<TSourceOperation, TTargetOperation>(this TSourceOperation source)
            where TSourceOperation : struct, Enum
            where TTargetOperation : struct, Enum
    {
        if (!typeof(TSourceOperation).IsEnum)
        {
            throw new InvalidCastException("Generic parameter TOperation of BLLBase class must be of enum type.");
        }

        if (!typeof(TTargetOperation).IsEnum)
        {
            throw new InvalidCastException("Generic parameter TOperation of BLLBase class must be of enum type.");
        }

        return Maybe.OfTryMethod(new TryMethod<string, TTargetOperation>(Enum.TryParse)) (source.ToString());
    }
}
