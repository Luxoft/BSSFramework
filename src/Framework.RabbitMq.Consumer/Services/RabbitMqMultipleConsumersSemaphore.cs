using System.Text.Json;

using Framework.RabbitMq.Consumer.Interfaces;
using Framework.RabbitMq.Consumer.Models;
using Framework.RabbitMq.Consumer.Settings;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Framework.RabbitMq.Consumer.Services;

public class RabbitMqMultipleConsumersSemaphore(
    IServiceProvider ServiceProvider,
    IOptions<RabbitMqConsumerSettings> ConsumerOptions)
    : IRabbitMqConsumerSemaphore
{
    private const string CacheKey = $"{nameof(RabbitMqMultipleConsumersSemaphore)}_Holder";

    private readonly IDistributedCache _cache = ServiceProvider.GetRequiredService<IDistributedCache>();

    private readonly RabbitMqConsumerSettings _settings = ConsumerOptions.Value;

    private readonly object _lock = new object();

    public bool TryObtain(Guid consumerId, out DateTime? obtainedAt)
    {
        obtainedAt = null;

        lock (this._lock)
        {
            var current = this.GetCurrentEntry();

            if (current != null
                && current.ConsumerId != consumerId
                && current.ObtainedAt.AddMilliseconds(this._settings.ConsumerExpirationMilliseconds) >= DateTime.Now)
            {
                return false;
            }

            var newEntry = new ConsumerSemaphoreData { ConsumerId = consumerId, ObtainedAt = DateTime.Now };
            this._cache.SetString(CacheKey, JsonSerializer.Serialize(newEntry));

            obtainedAt = newEntry.ObtainedAt;
            return true;
        }
    }

    public void TryRelease(Guid consumerId)
    {
        lock (this._lock)
        {
            var current = this.GetCurrentEntry();

            if (current?.ConsumerId == consumerId)
            {
                this._cache.Remove(CacheKey);
            }
        }
    }

    private ConsumerSemaphoreData? GetCurrentEntry()
    {
        var currentStr = this._cache.GetString(CacheKey);
        var current = currentStr != null
                          ? JsonSerializer.Deserialize<ConsumerSemaphoreData>(currentStr)
                          : null;
        return current;
    }
}
