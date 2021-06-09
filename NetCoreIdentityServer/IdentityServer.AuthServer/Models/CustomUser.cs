namespace IdentityServer.AuthServer.Models
{
    /// <summary>
    /// Custom üyelik sistemi
    /// </summary>
    public class CustomUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string City { get; set; }
    }
}
