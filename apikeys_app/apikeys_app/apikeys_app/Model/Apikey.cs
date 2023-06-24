namespace apikeys_app.Model
{
    public class Apikey
    {
        // Модель данных Apikey
        // Определяет какие поля с какими типами данных есть у Apikey
        public Guid Id { get; set; }
        public string? Key { get; set; }
        public string? Description { get; set; }
        public DateTime Created_on { get; set; }
    }
}
