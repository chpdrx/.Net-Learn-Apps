using apikeys_app.Model;
using Newtonsoft.Json.Linq;
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

        // Возвращает все apikeys для пользователя с user_id
        public static List<Apikey> GetAllKeys(string user_id)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();
            List<Apikey> keys = new();

            // Возвращает все записи таблицы apikeys, id которых принадлежит указанному user_id из таблицы users_apikeys
            var cmd = new NpgsqlCommand("SELECT * FROM apikeys ak inner join users_apikeys ua on ak.id = ua.apikey_id where ua.user_id = ($1)", conn)
            {
                Parameters =
                {
                    new() { Value = Guid.Parse(user_id) }
                }
            };
            var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Apikey key = new()
                {
                    Id = reader.GetGuid(0),
                    Key = reader.GetString(1),
                    Description = reader.GetString(2),
                    Created_on = reader.GetDateTime(3)
                };
                keys.Add(key);

            }
            return keys;
        }

        // Создаёт новую запись в таблице apikeys и связывает её с user_id в таблице users_apikeys
        public static Apikey CreateNewKey(string user_id, Apikey apikey)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();
            Guid key_id = Guid.NewGuid();

            var cmd = new NpgsqlCommand("INSERT INTO apikeys VALUES ($1, $2, $3, $4)", conn)
            {
                Parameters =
                {
                    new() { Value = key_id },
                    new() { Value = apikey.Key },
                    new() { Value = apikey.Description },
                    new() { Value = DateTime.UtcNow }
                }
            };
            cmd.ExecuteNonQuery();

            var cmd2 = new NpgsqlCommand("INSERT INTO users_apikeys VALUES ($1, $2, $3, $4)", conn)
            {
                Parameters =
                {
                    new() { Value = Guid.NewGuid() },
                    new() { Value = Guid.Parse(user_id) },
                    new() { Value = key_id },
                    new() { Value = DateTime.UtcNow }
                }
            };
            cmd2.ExecuteNonQuery();

            return GetKey(key_id.ToString());
        }

        // возвращает apikey из таблицы apikeys по его id
        public static Apikey GetKey(string key_id)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("SELECT * FROM apikeys WHERE id=($1)", conn)
            {
                Parameters =
                {
                    new() { Value = Guid.Parse(key_id) }
                }
            };
            var reader = cmd.ExecuteReader();

            reader.Read();
            Apikey apikey = new()
                {
                    Id = reader.GetGuid(0),
                    Key = reader.GetString(1),
                    Description = reader.GetString(2),
                    Created_on = reader.GetDateTime(3)
                };
            return apikey;
        }

        // меняет данные о apikey в базе на новые
        public static Apikey UpdateKey(Apikey apikey)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("UPDATE apikeys SET id = $1, apikey = $2, " +
                "description = $3, created_on = $4 WHERE id = ($1)", conn)
            {
                Parameters =
                {
                    new() { Value = apikey.Id },
                    new() { Value = apikey.Key },
                    new() { Value = apikey.Description },
                    new() { Value = apikey.Created_on }
                }
            };
            cmd.ExecuteNonQuery();
            return apikey;
        }

        // удаляет apikey и его связь с user_id
        public static bool DeleteKey(string key_id)
        {
            var conn = new NpgsqlConnection(conn_creds);
            conn.Open();

            var cmd = new NpgsqlCommand("DELETE FROM users_apikeys WHERE apikey_id = ($1)", conn)
            {
                Parameters =
                {
                    new() { Value = Guid.Parse(key_id) }
                }
            };
            cmd.ExecuteNonQuery();

            var cmd2 = new NpgsqlCommand("DELETE FROM apikeys WHERE id = ($1)", conn)
            {
                Parameters =
                {
                    new() { Value = Guid.Parse(key_id) }
                }
            };
            cmd2.ExecuteNonQuery();

            return true;
        }
    }
}
