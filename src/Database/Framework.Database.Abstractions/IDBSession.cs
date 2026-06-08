using System.Data;

namespace Framework.Database;

public interface IDBSession : ICurrentRevisionService, IAsyncDisposable, IDisposable
{
    DBSessionMode SessionMode { get; }

    IDbTransaction Transaction { get; }

    Task FlushAsync(CancellationToken ct);

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

    Task CloseAsync(CancellationToken ct);

    void IDisposable.Dispose() => this.DisposeAsync().GetAwaiter().GetResult();
}

