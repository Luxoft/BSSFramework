namespace Framework.Attachments.Domain
{

    /// <summary>
    /// Константы, определяющие логический набор операций, которые могут быть применены BLL над объектом
    /// </summary>
    /// <remarks>
    /// С помощью логических операций можно реализовать оповещение BLL о производимых над объектом операций:
    /// Для этого необходимо вызвать метод RaiseOperationProcessed
    /// this.RaiseOperationProcessed(subscriptionLambda, ConfigurationOperationContext.PreSave);
    /// Также можно выполнить в BLL соответствующую валидацию:
    /// this.Context.Validator.Validate(subscriptionLambda, ConfigurationOperationContextC.PreSave);
    /// </remarks>
    public enum AttachmentsOperationContext
    {
        Save = 17,

        All = 31,
    }
}
