using apikeys_app.Model;

namespace apikeys_app.Service
{
    public interface IApikeyService
    {
        public Apikey Create(string user_id, Apikey apikey);
        public Apikey Update(Apikey key);
        public bool Delete(string key_id);
        public Apikey GetKey(string key_id);
        public List<Apikey> GetAll(string user_id);
    }
}
