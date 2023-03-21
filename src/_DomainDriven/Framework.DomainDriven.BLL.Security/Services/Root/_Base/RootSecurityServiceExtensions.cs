using System;

using Framework.Core;
using Framework.SecuritySystem;

namespace Framework.DomainDriven.BLL.Security;

public static class RootSecurityServiceExtensions
{
    /// <summary>
    /// Проверка доступа к доменному объекту
    /// </summary>
    /// <typeparam name="TBLLContext">Тип контекста</typeparam>
    /// <typeparam name="TPersistentDomainObjectBase">Типа базового персистентного объекта</typeparam>
    /// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
    /// <param name="rootSecurityService">Сервис доступа</param>
    /// <param name="securityMode">Режим доступа</param>
    /// <param name="domainObject">Проверяемый доменный объект</param>
    /// <returns></returns>
    public static bool HasAccess<TBLLContext, TPersistentDomainObjectBase, TDomainObject>(this IRootSecurityService<TBLLContext, TPersistentDomainObjectBase> rootSecurityService, BLLSecurityMode securityMode, TDomainObject domainObject)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (rootSecurityService == null) throw new ArgumentNullException(nameof(rootSecurityService));

        return rootSecurityService.GetSecurityProvider<TDomainObject>(securityMode).HasAccess(domainObject);
    }

    /// <summary>
    /// Проверка доступа к доменному объекту
    /// </summary>
    /// <typeparam name="TBLLContext">Тип контекста</typeparam>
    /// <typeparam name="TPersistentDomainObjectBase">Типа базового персистентного объекта</typeparam>
    /// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
    /// <typeparam name="TSecurityOperationCode">Тип операции</typeparam>
    /// <param name="rootSecurityService">Сервис доступа</param>
    /// <param name="securityOperation">Операция доступа</param>
    /// <param name="domainObject">Проверяемый доменный объект</param>
    /// <returns></returns>
    public static bool HasAccess<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TSecurityOperationCode>(this IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> rootSecurityService, SecurityOperation<TSecurityOperationCode> securityOperation, TDomainObject domainObject)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TSecurityOperationCode : struct, Enum
    {
        if (rootSecurityService == null) throw new ArgumentNullException(nameof(rootSecurityService));

        return rootSecurityService.GetSecurityProvider<TDomainObject>(securityOperation).HasAccess(domainObject);
    }

    /// <summary>
    /// Проверка доступа к доменному объекту
    /// </summary>
    /// <typeparam name="TBLLContext">Тип контекста</typeparam>
    /// <typeparam name="TPersistentDomainObjectBase">Типа базового персистентного объекта</typeparam>
    /// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
    /// <typeparam name="TSecurityOperationCode">Тип операции</typeparam>
    /// <param name="rootSecurityService">Сервис доступа</param>
    /// <param name="securityOperationCode">Код операции доступа</param>
    /// <param name="domainObject">Проверяемый доменный объект</param>
    /// <returns></returns>
    public static bool HasAccess<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TSecurityOperationCode>(this IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> rootSecurityService, TSecurityOperationCode securityOperationCode, TDomainObject domainObject)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TSecurityOperationCode : struct, Enum
    {
        if (rootSecurityService == null) throw new ArgumentNullException(nameof(rootSecurityService));

        return rootSecurityService.GetSecurityProvider<TDomainObject>(securityOperationCode).HasAccess(domainObject);
    }

    /// <summary>
    /// Получение списка принципалов имеющих доступ к объекту
    /// </summary>
    /// <typeparam name="TBLLContext">Тип контекста</typeparam>
    /// <typeparam name="TPersistentDomainObjectBase">Типа базового персистентного объекта</typeparam>
    /// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
    /// <param name="rootSecurityService">Сервис доступа</param>
    /// <param name="securityMode">Режим доступа</param>
    /// <param name="domainObject">Проверяемый доменный объект</param>
    /// <returns></returns>
    public static UnboundedList<string> GetAccessors<TBLLContext, TPersistentDomainObjectBase, TDomainObject>(this IRootSecurityService<TBLLContext, TPersistentDomainObjectBase> rootSecurityService, BLLSecurityMode securityMode, TDomainObject domainObject)
            where TDomainObject : class, TPersistentDomainObjectBase
    {
        if (rootSecurityService == null) throw new ArgumentNullException(nameof(rootSecurityService));

        return rootSecurityService.GetSecurityProvider<TDomainObject>(securityMode).GetAccessors(domainObject);
    }

    /// <summary>
    /// Получение списка принципалов имеющих доступ к объекту
    /// </summary>
    /// <typeparam name="TBLLContext">Тип контекста</typeparam>
    /// <typeparam name="TPersistentDomainObjectBase">Типа базового персистентного объекта</typeparam>
    /// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
    /// <typeparam name="TSecurityOperationCode">Тип операции</typeparam>
    /// <param name="rootSecurityService">Сервис доступа</param>
    /// <param name="securityOperation">Операция доступа</param>
    /// <param name="domainObject">Проверяемый доменный объект</param>
    /// <returns></returns>
    public static UnboundedList<string> GetAccessors<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TSecurityOperationCode>(this IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> rootSecurityService, SecurityOperation<TSecurityOperationCode> securityOperation, TDomainObject domainObject)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TSecurityOperationCode : struct, Enum
    {
        if (rootSecurityService == null) throw new ArgumentNullException(nameof(rootSecurityService));

        return rootSecurityService.GetSecurityProvider<TDomainObject>(securityOperation).GetAccessors(domainObject);
    }

    /// <summary>
    /// Получение списка принципалов имеющих доступ к объекту
    /// </summary>
    /// <typeparam name="TBLLContext">Тип контекста</typeparam>
    /// <typeparam name="TPersistentDomainObjectBase">Типа базового персистентного объекта</typeparam>
    /// <typeparam name="TDomainObject">Тип доменного объекта</typeparam>
    /// <typeparam name="TSecurityOperationCode">Тип операции</typeparam>
    /// <param name="rootSecurityService">Сервис доступа</param>
    /// <param name="securityOperationCode">Код операции доступа</param>
    /// <param name="domainObject">Проверяемый доменный объект</param>
    /// <returns></returns>
    public static UnboundedList<string> GetAccessors<TBLLContext, TPersistentDomainObjectBase, TDomainObject, TSecurityOperationCode>(this IRootSecurityService<TBLLContext, TPersistentDomainObjectBase, TSecurityOperationCode> rootSecurityService, TSecurityOperationCode securityOperationCode, TDomainObject domainObject)
            where TDomainObject : class, TPersistentDomainObjectBase
            where TSecurityOperationCode : struct, Enum
    {
        if (rootSecurityService == null) throw new ArgumentNullException(nameof(rootSecurityService));

        return rootSecurityService.GetSecurityProvider<TDomainObject>(securityOperationCode).GetAccessors(domainObject);
    }
}
