using System;
using System.Threading.Tasks;
using Insurance.Utilities.Cache;
using Newtonsoft.Json;

namespace Insurance.Tests.Utils
{
    public class FakeCache :ICache
    {
        public string Key { private set; get; }
        public string Value { private set; get; }
        public TimeSpan Expiration { private set; get; }

        public Task Set<T>(string key, T value, TimeSpan expirationTime)
        {
            Key = key;
            Value = JsonConvert.SerializeObject(value);
            Expiration = expirationTime;
            return Task.CompletedTask;
        }

        public Task<T> Get<T>(string key) => string.IsNullOrEmpty(Value) || key != Key
            ? Task.FromResult(default(T))
            : Task.FromResult(JsonConvert.DeserializeObject<T>(Value));

        public static FakeCache Create() => new();

        public FakeCache With<T>(string key, T value, int expireInMin)
        {
            Set(key, value, TimeSpan.FromMinutes(expireInMin));
            return this;
        }
    }
}
