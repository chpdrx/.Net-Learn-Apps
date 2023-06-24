using auth_app.Models;

namespace auth_app.Services
{
    public interface ILoginService
    {
        public bool Get(Login userLogin);
    }
}
