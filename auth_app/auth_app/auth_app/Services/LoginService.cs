using auth_app.Data;
using auth_app.Models;

namespace auth_app.Services
{
    public class LoginService: ILoginService
    {
        // возвращает true если пароль пользователя совпадает с хешем пароля в базе
        public bool Get(Login user)
        {
            string hash = PgData.GetPassword(user.Username);
            return BCrypt.Net.BCrypt.Verify(user.Password, hash);
        }
    }
}
