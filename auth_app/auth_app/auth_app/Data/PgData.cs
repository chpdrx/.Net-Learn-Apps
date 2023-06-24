using apikeys_app.Model;
using auth_app.Models;
using Npgsql;

namespace auth_app.Data
{
    // Класс для запросов в postgres
    public class PgData
    {
        // заполение кредов подключения к базе из json'а с настройками подключения
        static Settings pg_settings = FileData.JsonParse();
        static string conn_creds = "Host=" + pg_settings.PgHost + ";Username=" + pg_settings.PgUsername
            + ";Password=" + pg_settings.PgPassword + ";Database=" + pg_settings.PgDatabase;
        //static string conn_creds = "Host=host.docker.internal;Username=root;Password=root;Database=wbanalytics_dev";

        // Проверка, есть ли в таблице users запись с переданным в метод username
        public static bool GetUsername(string username)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT username FROM users WHERE username=($1)", conn)
            {
                Parameters =
                {
                    new() { Value = username },
                }
            };
            var reader = cmd.ExecuteReader();
            // возвращает True, если был найден пользователь, иначе False
            return reader.Read();
        }

        // Вставка новой записи в таблицу users, из данных user, переданных в метод
        public static int InsertNewUser(User user)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("INSERT INTO users VALUES ($1, $2, $3, $4, $5)", conn)
            {
                Parameters =
                {
                    new() { Value = Guid.NewGuid() },
                    new() { Value = user.Name },
                    new() { Value = user.Username },
                    new() { Value = BCrypt.Net.BCrypt.HashPassword(user.Password) },
                    new() { Value = DateTime.UtcNow }
                }
            };
            // возвращает -1 если не было вставлено ни одной строки
            return cmd.ExecuteNonQuery();
        }

        // возвращает хеш пароля из базы, соответствующий переданному пользователю
        public static string GetPassword(string username)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT password_hash FROM users WHERE username=($1)", conn)
            {
                Parameters =
                {
                    new() { Value = username },
                }
            };
            var reader = cmd.ExecuteReader();
            reader.Read();
            // возвращает password_hash от username
            return reader.GetString(0);
        }

        // возвращает id пользователя по его username
        public static string GetId(string username)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT id FROM users WHERE username=($1)", conn)
            {
                Parameters =
                {
                    new() { Value = username },
                }
            };
            var reader = cmd.ExecuteReader();
            reader.Read();
            // возвращает id от username
            return reader.GetGuid(0).ToString();
        }

        // возвращает все данные о пользователе из бд
        public static User GetCurrentUser(string id)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT * FROM users WHERE id=($1)", conn)
            {
                Parameters =
                {
                    new() { Value = Guid.Parse(id) },
                }
            };
            var reader = cmd.ExecuteReader();
            reader.Read();
            User user = new User() { Id = reader.GetGuid(0), 
                                        Name = reader.GetString(1),
                                        Username = reader.GetString(2),
                                        Password = reader.GetString(3),
                                        Created_on = reader.GetDateTime(4) };
            return user;
        }
    }
}
