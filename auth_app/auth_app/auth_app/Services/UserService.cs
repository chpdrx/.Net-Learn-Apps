using auth_app.Data;
using auth_app.Models;

namespace auth_app.Services
{
    // реализация интерфейса IUserService
    public class UserService : IUserService
    {
        // метод создания нового пользователя из данных user, пришедших по api
        public User Create(User user)
        {
            int db_response = PgData.InsertNewUser(user);
            if (db_response != -1) return user;
            else return new User();
        }
    }
}
