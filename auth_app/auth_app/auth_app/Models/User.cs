namespace auth_app.Models
{
    public class User
    {
        // Модель данных User'а
        // Определяет какие поля с какими типами данных есть у User
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public DateTime Created_on { get; set; }
    }
}
