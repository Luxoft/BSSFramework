namespace Framework.BLL.Events;

public readonly record struct TypeEventDependency(TypeEvent SourceTypeEvent, TypeEvent TargetTypeEvent, Func<object, object> GetTargetValue)
{
    public static TypeEventDependency FromSaveAndRemove<TFrom, TTo>(Func<TFrom, TTo> toPathFunc, Func<TFrom, bool>? isSaveFunc = null, Func<TFrom, bool>? isRemoveFunc = null)
        where TFrom : class
        where TTo : class =>
        new(TypeEvent.SaveAndRemove(isSaveFunc, isRemoveFunc), TypeEvent.SaveAndRemove<TTo>(), z => toPathFunc((TFrom)z));
}
