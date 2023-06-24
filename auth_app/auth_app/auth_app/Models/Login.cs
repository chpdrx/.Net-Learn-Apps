namespace auth_app.Models
{
    public class Login
    {
        // Модель данных, необходимых для авторизации пользователя
        // Определяет какие поля нужно запрашивать для проверки существования пользователя
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
