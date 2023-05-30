using Grim.Utils;
using System.Collections.Generic;

namespace Grim.Rules
{
    public class GameEventData
    {
        public string Event { get; set; }

        private Dictionary<string, object> entries;
        
        public GameEventData(string @event)
        {
            entries = new();
            Event = @event;
        }


        private string FullKey<T>(string id)
            => $"{typeof(T).ToString()}_{id}";


        public GameEventData Put<T>(string key, object value)
        {
            entries[FullKey<T>(key)] = value;
            return this;
        }

        public T Get<T>(string key)
            => entries.ContainsKey(FullKey<T>(key)) ? (T)entries[FullKey<T>(key)] : default(T);
    }
}
