using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Framework.DomainDriven.BLL.Tracking;
using Framework.Persistent;

namespace Framework.DomainDriven
{
    public interface IDBSession : ICurrentRevisionService, IAsyncDisposable, IDisposable
    {
        DBSessionMode SessionMode { get; }

        IObjectStateService GetObjectStateService();

        IDALFactory<TPersistentDomainObjectBase, TIdent> GetDALFactory<TPersistentDomainObjectBase, TIdent>()
            where TPersistentDomainObjectBase : class, IIdentityObject<TIdent>;

        /// <summary>
        /// Мануальный флаш сессии, при его вызове срабатывают только Flushed-евенты, TransactionCompleted-евенты вызываются только при закрытие сессии
        /// </summary>
        void Flush()
        {
            this.FlushAsync().GetAwaiter().GetResult();
        }

        Task FlushAsync(CancellationToken cancellationToken = default);

        IEnumerable<ObjectModification> GetModifiedObjectsFromLogic();

        IEnumerable<ObjectModification> GetModifiedObjectsFromLogic<TPersistentDomainObjectBase>();

        /// <summary>
        /// Закрывает текущую транзакцию без применения изменений.
        /// </summary>
        void AsFault();

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

        public void Close()
        {
            this.CloseAsync().GetAwaiter().GetResult();
        }

        Task CloseAsync(CancellationToken cancellationToken = default);

        void IDisposable.Dispose()
        {
            this.DisposeAsync().GetAwaiter().GetResult();
        }
    }
}
