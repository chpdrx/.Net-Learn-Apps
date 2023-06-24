using resource_app.Models;

namespace resource_app.Service
{
    public interface IUserService
    {
        public List<Users> GetAllUsers();
        public bool DeleteUser(string user_id);
        public Users GetUserByUsername(string username);
        public Users GetUserById(Guid user_id);
        public Users UserUpdate(string user_id, Users user);
    }
}
