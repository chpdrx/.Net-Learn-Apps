namespace apikeys_app.Service
{
    public interface ITokenVerifyService
    {
        public string GetToken(string token);
    }
}
