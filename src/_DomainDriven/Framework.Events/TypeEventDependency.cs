namespace Framework.Events;

public class TypeEventDependency
{
    public static TypeEventDependency FromSaveAndRemove<TFrom, TTo>(Func<TFrom, TTo> toPathFunc, Func<TFrom, bool> isSaveFunc = null, Func<TFrom, bool> isRemoveFunc = null)
    {

        return new TypeEventDependency(TypeEvent.SaveAndRemove(isSaveFunc, isRemoveFunc), TypeEvent.SaveAndRemove<TTo>(null, null), z => toPathFunc((TFrom)z));
    }

    private readonly TypeEvent sourceTypeEvent;

    private readonly TypeEvent targetTypeEvent;

    private readonly Func<object, object> getTargetObjectFunc;

    public TypeEventDependency(TypeEvent sourceTypeEvent, TypeEvent targetTypeEvent, Func<object, object> getTargetObjectFunc)
    {
        if (getTargetObjectFunc == null)
        {
            throw new ArgumentNullException("getToFunc");
        }

        this.sourceTypeEvent = sourceTypeEvent;
        this.targetTypeEvent = targetTypeEvent;
        this.getTargetObjectFunc = getTargetObjectFunc;
    }

    public TypeEvent TargetTypeEvent
    {
        get { return this.targetTypeEvent; }
    }

    public TypeEvent SourceTypeEvent
    {
        get { return this.sourceTypeEvent; }
    }

    public object GetTargetValue(object from)
    {
        return this.getTargetObjectFunc(from);
    }
}
