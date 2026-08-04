using Auth_Service.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Auth_Service.Data
{
    public class AuthDb : DbContext
    {
        public AuthDb(DbContextOptions<AuthDb> options) :
            base (options){ }

        public DbSet<User> Users => Set<User>();
    }
}
