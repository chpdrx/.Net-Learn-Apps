using auth_app.Models;

namespace auth_app.Services
{
    public interface ITokenService
    {
        public Token GetToken(string token);
    }
}
