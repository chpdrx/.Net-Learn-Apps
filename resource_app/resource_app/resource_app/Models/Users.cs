namespace resource_app.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
