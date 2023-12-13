using System.Text.Json;

using Framework.DomainDriven;
using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Models;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Framework.RabbitMq.Consumer.Services;

public record RabbitMqSingleActiveConsumerSemaphore(
    IServiceProvider ServiceProvider,
    IOptions<RabbitMqConsumerSettings> ConsumerOptions,
    IDBSessionEvaluator SessionEvaluator)
    : IRabbitMqConsumerSemaphore
{
    private const string CacheKey = $"{nameof(RabbitMqSingleActiveConsumerSemaphore)}_Holder";

    private readonly IDistributedCache _cache = ServiceProvider.GetRequiredService<IDistributedCache>();

    private readonly DistributedCacheEntryOptions _cacheOptions =
        new() { SlidingExpiration = TimeSpan.FromMilliseconds(ConsumerOptions.Value.ActiveConsumerClaimTtlMilliseconds) };

    private readonly RabbitMqConsumerSettings _settings = ConsumerOptions.Value;

    public async Task<(bool IsSuccess, DateTime? ObtainedAt)> TryObtainAsync(Guid consumerId, CancellationToken cancellationToken) =>
        await this.SessionEvaluator.EvaluateAsync<(bool, DateTime?)>(
            DBSessionMode.Write,
            async sp =>
            {
                await sp.GetRequiredService<IRabbitMqConsumerLockService>().LockAsync(cancellationToken);

                var current = await this.GetCurrentEntryAsync(cancellationToken);

                if (current != null
                    && current.Value.ConsumerId != consumerId
                    && current.Value.ObtainedAt.AddMilliseconds(this._settings.ActiveConsumerClaimTtlMilliseconds) >= DateTime.Now)
                    return (false, null);

                var newEntry = new ConsumerSemaphoreData { ConsumerId = consumerId, ObtainedAt = DateTime.Now };
                await this._cache.SetStringAsync(CacheKey, JsonSerializer.Serialize(newEntry), this._cacheOptions, cancellationToken);

                return (true, newEntry.ObtainedAt);
            });

    public Task<bool> TryReleaseAsync(Guid consumerId, CancellationToken cancellationToken) =>
        this.SessionEvaluator.EvaluateAsync(
            DBSessionMode.Write,
            async sp =>
            {
                await sp.GetRequiredService<IRabbitMqConsumerLockService>().LockAsync(cancellationToken);

                var current = await this.GetCurrentEntryAsync(cancellationToken);

                if (current?.ConsumerId != consumerId) return false;

                await this._cache.RemoveAsync(CacheKey, cancellationToken);

                return true;
            });

    private async Task<ConsumerSemaphoreData?> GetCurrentEntryAsync(CancellationToken cancellationToken)
    {
        var currentStr = await this._cache.GetStringAsync(CacheKey, cancellationToken);
        return currentStr != null
                   ? JsonSerializer.Deserialize<ConsumerSemaphoreData>(currentStr)
                   : null;
    }
}
