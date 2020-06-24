namespace DatingApp.API.Model
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PassworSalt { get; set; }
    }
}