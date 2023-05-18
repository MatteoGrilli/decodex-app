
using System.Collections.Generic;

namespace Grim.Utils
{
    public class Args
    {
        private Dictionary<string, object> entries;

        public Args()
        {
            entries = new();
        }

        private string FullKey<T>(string id)
            => $"{typeof(T).ToString()}_{id}";


        public void Put<T>(string key, object value)
            => entries[FullKey<T>(key)] = value;

        public T Get<T>(string key)
            => (T) entries[FullKey<T>(key)];
    }
}
