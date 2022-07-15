using System;
using System.Collections.Generic;

using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent;

namespace Framework.DomainDriven.BLL
{
    public interface IDBSession : ICurrentRevisionService, IDisposable
    {
        DBSessionMode SessionMode { get; }


        /// <summary>
        /// Евент срабатывает циклично, пока происходят изменения доменных объектов
        /// </summary>
        event EventHandler<DALChangesEventArgs> Flushed;

        /// <summary>
        /// Евент срабатывает только 1 раз перед закрытием сессии, изменения в базу в этот момент разрешены
        /// </summary>
        event EventHandler<DALChangesEventArgs> BeforeTransactionCompleted;

        /// <summary>
        /// Евент срабатывает только 1 раз перед закрытием сессии, изменения в базу в этот момент запрещены
        /// </summary>
        event EventHandler<DALChangesEventArgs> AfterTransactionCompleted;

        event EventHandler Closed;

        IObjectStateService GetObjectStateService();

        IDALFactory<TPersistentDomainObjectBase, TIdent> GetDALFactory<TPersistentDomainObjectBase, TIdent>()
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;

        /// <summary>
        /// Мануальный флаш сессии, при его вызове срабатывают только Flushed-евенты, TransactionCompleted-евенты вызываются только при закрытие сессии
        /// </summary>
        void Flush();

        IEnumerable<ObjectModification> GetModifiedObjectsFromLogic();

        IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>();

        /// <summary>
        /// Закрывает текущую транзакцию без применения изменений.
        /// </summary>
        void ManualFault();

        /// <summary>
        /// Gets the maximum audit revision.
        /// </summary>
        /// <returns>System.Int64.</returns>
        long GetMaxRevision();

        /// <summary>
        /// Перевод сессию в режим "только для чтения" (доступно только перед фактическим использованием сессии)
        /// </summary>
        void AsReadOnly();

        /// <summary>
        /// Переводит сессию в режим для записи (доступно только перед фактическим использованием сессии)
        /// </summary>
        void AsWritable();
    }
}
