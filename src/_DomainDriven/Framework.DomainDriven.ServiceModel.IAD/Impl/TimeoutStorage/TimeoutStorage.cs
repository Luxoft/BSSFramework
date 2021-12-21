using System;
using System.Collections.Generic;
using System.Linq;

using Framework.Core;

namespace Framework.DomainDriven.ServiceModel.IAD
{
    public class TimeoutStorage : IObjectStorage
    {
        private readonly IDateTimeService _dateTimeService;

        private readonly TimeSpan _liveTime;

        private readonly object _locker = new object();

        private readonly Dictionary<string, KeyValuePair<DateTime, object>> _cache = new Dictionary<string, KeyValuePair<DateTime, object>>();


        public TimeoutStorage(IDateTimeService dateTimeService, TimeSpan liveTime)
        {
            this._dateTimeService = dateTimeService;
            this._liveTime = liveTime;
        }


        public void ClearCache()
        {
            lock (this._locker)
            {
                var now = this._dateTimeService.Now;

                var timeoutKeys = from pair in this._cache

                                  where pair.Value.Key < now

                                  select pair.Key;

                timeoutKeys.ToList().Foreach(key => this._cache.Remove(key));
            }
        }


        public void Push<T>(string key, T value)
        {
            lock (this._locker)
            {
                this.ClearCache();

                this._cache.Add(key, (this._dateTimeService.Now + this._liveTime).ToKeyValuePair((object)value));
            }
        }

        public T Pop<T>(string key)
        {
            lock (this._locker)
            {
                this.ClearCache();

                var res = (T)this._cache[key].Value;

                this._cache.Remove(key);

                return res;
            }
        }
    }
}