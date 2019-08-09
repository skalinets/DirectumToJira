using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace DirectumToJira
{
    public class Cache
    {
        readonly ObjectCache _objectCache;

        public Cache(ObjectCache objectCache)
        {
            _objectCache = objectCache ?? throw new ArgumentNullException(nameof(objectCache));
        }
        public T GetObjectFromCache<T>(string cacheItemName, TimeSpan cacheTime, Func<T> objectSettingFunction)
        {
            var cachedObject = (T)_objectCache[cacheItemName];
            if (Equals(cachedObject, default(T)))
            {
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTimeOffset.Now.Add(cacheTime);
                cachedObject = objectSettingFunction();
                _objectCache.Set(cacheItemName, cachedObject, policy);
            }
            return cachedObject;
        }
    }
}
