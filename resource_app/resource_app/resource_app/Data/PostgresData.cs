using Npgsql;
using resource_app.Models;

namespace resource_app.Data
{
    public class PostgresData
    {
        // заполение кредов подключения к базе из json'а с настройками подключения
        static Settings pg_settings = FileData.JsonParse();
        static string conn_creds = "Host=" + pg_settings.PgHost + ";Username=" + pg_settings.PgUsername
            + ";Password=" + pg_settings.PgPassword + ";Database=" + pg_settings.PgDatabase;

        // метод возвращает из базы данные пользователей кроме пароля
        public static List<Users> GetAllUsers()
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();
            List<Users> users = new();

            var cmd = new NpgsqlCommand("SELECT id, name, username, created_on FROM users ORDER BY created_on", conn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Users user = new()
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Username = reader.GetString(2),
                    CreatedOn = reader.GetDateTime(3),
                };
                users.Add(user);
            };
            return users;
        }

        // метод удаляет пользователя по id
        public static bool DeleteUser(string user_id)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("DELETE FROM users WHERE id=$1", conn)
            {
                Parameters =
                {
                    new() { Value = Guid.Parse(user_id) }
                }
            };
            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // получение информации о пользователе по его username
        public static Users GetUserByUsername(string username)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT id, name, username, created_on FROM users WHERE username=$1", conn)
            {
                Parameters =
                {
                    new() { Value = username }
                }
            };
            var reader = cmd.ExecuteReader();

            try
            {
                reader.Read();
                Users user = new()
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Username = reader.GetString(2),
                    CreatedOn = reader.GetDateTime(3),
                };
                return user;
            }
            catch
            {
                return null;
            }
        }

        // получение информации о пользователе по его id
        public static Users GetUserById(Guid user_id)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT id, name, username, created_on FROM users WHERE id=$1", conn)
            {
                Parameters =
                {
                    new() { Value = user_id }
                }
            };
            var reader = cmd.ExecuteReader();

            try
            {
                reader.Read();
                Users user = new()
                {
                    Id = reader.GetGuid(0),
                    Name = reader.GetString(1),
                    Username = reader.GetString(2),
                    CreatedOn = reader.GetDateTime(3),
                };
                return user;
            }
            catch
            {
                return null;
            }
        }

        // изменение данных о пользователе по его id
        public static Users UserUpdate(Guid user_id, Users user)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("UPDATE users SET name=$1, username=$2, created_on=$3 WHERE id=$4", conn)
            {
                Parameters =
                {
                    new() { Value = user.Name },
                    new() { Value = user.Username },
                    new() { Value = user.CreatedOn },
                    new() { Value = user_id }
                }
            };
            try
            {
                cmd.ExecuteNonQuery();
                return user;
            }
            catch
            {
                return null;
            }
        }
    }
}
