namespace MemoryDb.Console
{
    public class KeyValueStore
    {
        private readonly Dictionary<string, object> _data;

        public KeyValueStore()
        {
            _data = [];
        }

        public void Set(string key, object value)
        {
            _data[key] = value;
        }

        public object Get(string key) => _data[key];
    }
}
