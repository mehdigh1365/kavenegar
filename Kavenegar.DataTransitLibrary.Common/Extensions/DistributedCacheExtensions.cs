using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Kavenegar.DataTransitLibrary.Common.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task SetRecordAsync<T>(this IDistributedCache cache, string key, T record,
            CancellationToken cancellationToken,
            TimeSpan? absoluteExpireTime = null, TimeSpan? unUsedExpiredTime = null)
        {
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromDays(60),
                SlidingExpiration = unUsedExpiredTime
            };

            var data = JsonConvert.SerializeObject(record, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            await cache.SetStringAsync(key, data, option, cancellationToken);
        }


        public static async Task<T> GetRecordAsync<T>(this IDistributedCache cache, string key,
            CancellationToken cancellationToken)
        {
            var data = await cache.GetStringAsync(key, cancellationToken);

            if (data is null)
                return default(T);

            return JsonConvert.DeserializeObject<T>(data);
        }

        public static async Task RemoveRecordAsync(this IDistributedCache cache, string key,
            CancellationToken cancellationToken)
        {
            await cache.RemoveAsync(key, cancellationToken);
        }
    }
}
