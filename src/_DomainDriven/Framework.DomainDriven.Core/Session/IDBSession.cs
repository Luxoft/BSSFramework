﻿using System.Data;

namespace Framework.DomainDriven;

public interface IDBSession : ICurrentRevisionService, IAsyncDisposable, IDisposable
{
    DBSessionMode SessionMode { get; }

    IDbTransaction Transaction { get; }

    /// <summary>
    /// Мануальный флаш сессии, при его вызове срабатывают только Flushed-евенты, TransactionCompleted-евенты вызываются только при закрытие сессии
    /// </summary>
    void Flush()
    {
        this.FlushAsync().GetAwaiter().GetResult();
    }

    Task FlushAsync(CancellationToken cancellationToken = default);

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
