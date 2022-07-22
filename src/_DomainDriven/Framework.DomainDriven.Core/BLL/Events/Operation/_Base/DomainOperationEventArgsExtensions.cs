using System;

using Framework.Core;

namespace Framework.DomainDriven.BLL;

public static class DomainOperationEventArgsExtensions
{
    public static void ProcessOtherEventListener<TDomainObject, TOperation, TOtherDomainObject, TOtherOperation>(this IDomainOperationEventArgs<TDomainObject, TOperation> eventArgs,  OperationEventSender<TOtherDomainObject, TOtherOperation> otherListener)
            where TOtherOperation : struct, Enum
            where TOtherDomainObject : class
            where TDomainObject : class, TOtherDomainObject
            where TOperation : struct, Enum
    {
        OperationConverter<TOperation, TOtherOperation>.Map.GetMaybeValue(eventArgs.Operation).Match(otherOperation =>
        {
            var newEventArgs = new DomainOperationEventArgs<TOtherDomainObject, TOtherOperation>(eventArgs.DomainObject, typeof(TDomainObject), otherOperation);

            otherListener.SendEvent(newEventArgs, false);
        });
    }
}
