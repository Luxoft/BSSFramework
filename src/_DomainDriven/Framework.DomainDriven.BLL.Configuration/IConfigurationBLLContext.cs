using System;

using Framework.Core.Serialization;
using Framework.Notification;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL.Configuration
{
    public interface IConfigurationBLLContext : IExceptionSenderContainer
    {
        bool DisplayInternalError { get; }

        IExceptionService ExceptionService { get; }


        IBLLSimpleQueryBase<IEmployee> GetEmployeeSource();

        bool HasAttachment<TDomainObject>(TDomainObject domainObject)
            where TDomainObject : IIdentityObject<Guid>;


        IMessageTemplate GetMessageTemplate(Guid messageTemplateId, IdCheckMode idCheckMode);

        /// <summary>
        /// Получение текущей ревизии из аудита (пока возвращает 0, если вызван до флаша сессии)
        /// </summary>
        /// <returns></returns>
        long GetCurrentRevision();
    }
}
