using auth_app.Models;

namespace auth_app.Services
{
    // интерфейс с методами для User
    public interface IUserService
    {
        public User Create(User user);
    }
}
