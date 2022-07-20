namespace Framework.DomainDriven.BLL
{
    /// <summary>
    /// Интерфейс объекта содержащего контейнер слушателей BLL-ных событий
    /// </summary>
    /// <typeparam name="TDomainObjectBase">Тип доменного объекта</typeparam>
    public interface IBLLOperationEventContext<in TDomainObjectBase>
        where TDomainObjectBase : class
    {
        /// <summary>
        /// Возвращается контейнер слушателей BLL-ных событий
        /// </summary>
        IOperationEventListenerContainer<TDomainObjectBase> OperationListeners { get; }
    }
}
