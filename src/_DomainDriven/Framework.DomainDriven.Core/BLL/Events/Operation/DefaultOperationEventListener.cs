namespace Framework.DomainDriven.BLL;

internal sealed class DefaultOperationEventSender<TDomainObject> : OperationEventSender<TDomainObject, BLLBaseOperation>

        where TDomainObject : class
{
    public DefaultOperationEventSender(IEnumerable<IOperationEventListener<TDomainObject>> eventListeners, Dictionary<Type, Dictionary<Type, OperationEventSender>> cache)
            : base(eventListeners, cache)
    {
    }

    internal override IEnumerable<KeyValuePair<Type, KeyValuePair<Type, OperationEventSender>>> GetOtherEventListeners()
    {
        return this.GetDefaultOtherEventListeners();
    }
}
