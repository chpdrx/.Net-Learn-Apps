using apikeys_app.Model;
using auth_app.Data;

namespace apikeys_app.Service
{
    public class ApikeyService : IApikeyService
    {
        public Apikey Create(string user_id, Apikey apikey)
        {
            Apikey db_key = PgData.CreateNewKey(user_id, apikey);
            return db_key;
        }

        public bool Delete(string key_id)
        {
            bool result = PgData.DeleteKey(key_id);
            return result;
        }

        public List<Apikey> GetAll(string user_id)
        {
            List<Apikey> keys = PgData.GetAllKeys(user_id);
            return keys;
        }

        public Apikey GetKey(string key_id)
        {
            Apikey apikey = PgData.GetKey(key_id);
            return apikey;
        }

        public Apikey Update(Apikey key)
        {
            Apikey result = PgData.UpdateKey(key);
            return result;
        }
    }
}
