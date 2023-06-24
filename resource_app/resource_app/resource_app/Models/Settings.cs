namespace resource_app.Models
{
    public class Settings
    {
        public string JwtKey { get; set; }
        public string PgHost { get; set; }
        public string PgUsername { get; set; }
        public string PgPassword { get; set; }
        public string PgDatabase { get; set; }

    }
}
