using MemoryDb.Console.Models;
using static MemoryDb.Console.Models.Enums;

namespace MemoryDb.Console
{
    public class KeyValueStore
    {
        private readonly Dictionary<string, object> _data;
        private readonly Dictionary<string, DateTime> _expirationTime;
        private readonly TimeSpan defaultTtl = TimeSpan.MaxValue;

        public KeyValueStore()
        {
            _data = [];
            _expirationTime = [];
        }

        public StatusModel Set(string key, object value, int ttl = -1)
        {
            var status = new StatusModel();

            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    status.Status = StatusEnum.Error;
                    status.Message = "Key is null or empty";
                    return status;
                }

                if (Exist(key))
                {
                    status.Status = StatusEnum.Error;
                    status.Message = "Key already exists";
                    return status;
                }

                var dateTime = (ttl == -1) ? DateTime.MaxValue : DateTime.UtcNow.Add(TimeSpan.FromSeconds(ttl));

                _data[key] = value;
                _expirationTime[key] = dateTime;
            }

            catch (Exception ex)
            {
                status.Status = StatusEnum.Error;
                status.Message = ex.Message;
            }

            status.Message = "Successfully Stored";
            return status;
        }

        public StatusModel Get(string key) 
        {
            var status = new StatusModel();

            if (string.IsNullOrEmpty(key))
            {
                status.Status = StatusEnum.Error;
                status.Message = "Key is null or empty";
                return status;
            }

            if (Exist(key))
            {
                if (_expirationTime[key] >= DateTime.UtcNow)
                {
                    status.Status = StatusEnum.Success;
                    status.Message = StatusEnum.Success.ToString();
                    status.Value = _data[key];
                    return status;
                }
                else
                {
                    _data.Remove(key);
                    _expirationTime.Remove(key);
                }
            }
            else
            {
                status.Status = StatusEnum.Error;
                status.Message = "Key does not exist";
            }

            status.Status = StatusEnum.Error;
            status.Message = StatusEnum.Error.ToString();

            return status;
        }

        public bool Exist(string key) => _data.ContainsKey(key) && _expirationTime.ContainsKey(key);
        
        public StatusModel Delete(string key)
        {
            var status = new StatusModel();

            if (string.IsNullOrEmpty(key))
            {
                status.Status = StatusEnum.Error;
                status.Message = "Key is null or empty";
                return status;
            }

            if (Exist(key))
            {
                _data.Remove(key);
                _expirationTime.Remove(key);
                status.Status = StatusEnum.Success;
                status.Message = "Key removed";
            }
            else
            {
                status.Status = StatusEnum.Error;
                status.Message = "Key does not exist";
            }

            return status;
        }

        // create update method here....
    }
}
