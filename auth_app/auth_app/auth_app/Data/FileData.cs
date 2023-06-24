using apikeys_app.Model;
using Newtonsoft.Json.Linq;

namespace auth_app.Data
{
    public class FileData
    {
        public static Settings JsonParse()
        {
            string? myJsonString = File.ReadAllText("settings.json");
            JObject? myJObject = JObject.Parse(myJsonString);
            Settings settings = new()
            {
                JwtKey = myJObject.SelectToken("Jwt").SelectToken("Key").Value<string>(),
                PgHost = myJObject.SelectToken("Postgres").SelectToken("Host").Value<string>(),
                PgUsername = myJObject.SelectToken("Postgres").SelectToken("Username").Value<string>(),
                PgPassword = myJObject.SelectToken("Postgres").SelectToken("Password").Value<string>(),
                PgDatabase = myJObject.SelectToken("Postgres").SelectToken("Database").Value<string>()
            };
            return settings;
        }
    }
}
