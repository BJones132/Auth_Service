using Microsoft.EntityFrameworkCore;

namespace Auth_Service.Data.Models
{
    [Index(nameof(username), IsUnique = true)]
    public class User
    {
        public int id { get; set; }
        public required string username { get; set; }
        public required string password { get; set; }
        public string? access_token { get; set; }
        public DateOnly? token_expiry { get; set; }
        public int failed_attempts { get; set; }
        public bool locked_out { get; set; }
    }
}
