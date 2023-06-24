using resource_app.Data;
using resource_app.Models;

namespace resource_app.Service
{
    public class UserService : IUserService
    {
        public bool DeleteUser(string user_id)
        {
            bool isDeleted = PostgresData.DeleteUser(user_id);
            return isDeleted;
        }

        public List<Users> GetAllUsers()
        {
            List<Users> users = PostgresData.GetAllUsers();
            return users;
        }

        public Users GetUserById(Guid user_id)
        {
            Users user = PostgresData.GetUserById(user_id);
            return user;
        }

        public Users GetUserByUsername(string username)
        {
            Users user = PostgresData.GetUserByUsername(username);
            return user;
        }

        public Users UserUpdate(string user_id, Users user)
        {
            try
            {
                Guid userId = Guid.Parse(user_id);
                Users userInPg = PostgresData.GetUserById(userId);
                if (userInPg == null) return userInPg;
                else
                {
                    user.Id = userInPg.Id;
                    if (user.Name == null) user.Name = userInPg.Name;
                    if (user.Username == null) user.Username = userInPg.Username;
                    if (user.CreatedOn == default) user.CreatedOn = userInPg.CreatedOn;
                }
                Users updated_user = PostgresData.UserUpdate(userId, user);
                return updated_user;
            }
            catch
            {
                return null;
            } 
        }
    }
}
